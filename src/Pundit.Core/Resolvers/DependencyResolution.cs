﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Utils;
using EBerzosa.Pundit.Core.Versioning;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Resolvers
{
   internal class DependencyResolutionCache
   {
      private readonly Dictionary<string, List<SatisfyingInfo>> _versionCache;
      private readonly Dictionary<PackageKey, PackageManifest> _manifestCache;

      private readonly Func<DependencyNode, IEnumerable<SatisfyingInfo>> _fallbackSatisfyingInfos;
      private readonly NuGet.Frameworks.NuGetFramework _framework;

      public DependencyResolutionCache(Func<DependencyNode, IEnumerable<SatisfyingInfo>> fallbackSatisfyingInfos, NuGet.Frameworks.NuGetFramework framework)
      {
         _versionCache = new Dictionary<string, List<SatisfyingInfo>>();
         _manifestCache = new Dictionary<PackageKey, PackageManifest>();

         _fallbackSatisfyingInfos = fallbackSatisfyingInfos;
         _framework = framework;
      }

      public IEnumerable<SatisfyingInfo> GetSatisfyingInfos(DependencyNode node)
      {
         if (_versionCache.ContainsKey(node.PackageId))
            return _versionCache[node.PackageId];

         _versionCache.Add(node.PackageId, new List<SatisfyingInfo>());

         _versionCache[node.PackageId].AddRange(_fallbackSatisfyingInfos(node));

         return _versionCache[node.PackageId];
      }

      public PackageManifest GetManifest(DependencyNode node)
      {
         if (!_manifestCache.ContainsKey(node.ActiveVersionKey))
            _manifestCache[node.ActiveVersionKey] = node.ActiveRepository.GetManifest(node.ActiveVersionKey, _framework);

         return _manifestCache[node.ActiveVersionKey];
      }
   }

   public class DependencyResolution
   {
      private readonly IWriter _writer;
      private readonly DependencyResolutionCache _cache;

      private readonly IRepository[] _activeRepositories;
      private readonly DependencyNode _rootDependencyNode;
      

      public DependencyResolution(IWriter writer)
      {
         _writer = writer;
      }

      private DependencyResolution(IWriter writer, PackageManifestRoot rootManifest, IRepository[] activeRepositories, string releaseLabel)
      {
         _cache = new DependencyResolutionCache(GetAllAvailableVersions, rootManifest.Framework);

         _writer = writer;
         _activeRepositories = activeRepositories;

         var version = new VersionRangeExtended(rootManifest.Version) {ReleaseLabel = releaseLabel};

         _rootDependencyNode = new DependencyNode(null, rootManifest.PackageId, rootManifest.Framework.GetShortFolderName(), version);
         _rootDependencyNode.MarkAsRoot(rootManifest);
      }

      public Tuple<VersionResolutionTable, DependencyNode> Resolve(PackageManifestRoot rootManifest, IRepository[] activeRepositories, string releaseLabel)
      {
         return new DependencyResolution(_writer, rootManifest, activeRepositories, releaseLabel).Resolve();
      }

      /// <summary>
      /// Resolution algorithm in V1 is fairly simple. Every dependency has a
      /// version pattern and platform name. Steps:
      /// 
      /// 1) Starting from root, resolve version patterns to version array for every node
      /// in the tree recursively (the whole structure is not known yet)
      /// 
      /// 2) Starting from root, resolve latest version's manifest to get dependencies of
      /// dependencies.
      /// 
      /// 3) Repeat steps 1-2 until all the version patterns and manifests are resolved.
      /// 
      /// 4) Flatten the tree to array of (PackageId, Platform, Version[])
      /// 
      /// 5) Using (PackageId, Platform) as a key, merge array to a dictionary where
      /// value is Version[] and is an intersection of all Version[] for the key from
      /// array created in step 4.
      /// 
      /// Data structure received in step 5 is the result of dependency resolution.
      /// Empty Version[] indicates a conflict for that package which can be displayed
      /// to a user using information from DependencyNode tree.
      /// 
      /// </summary>
      /// <returns></returns>
      private Tuple<VersionResolutionTable, DependencyNode> Resolve()
      {
         using (new TimeMeasurer(_writer.Text))
         {
            //steps 1-2-3
            ResolveAll();
         }

         //steps 4-5
         return Tuple.Create(Flatten(), _rootDependencyNode);
      }

      private void ResolveAll()
      {
         while (!_rootDependencyNode.IsRecursivelyFull)
         {
            //first step: resolve version patterns
            //(at least root must have all the dependencies populated already)
            ResolveVersions(_rootDependencyNode);

            //second step: resolve manifests
            ResolveManifests(_rootDependencyNode);
         }
      }

      private void ResolveVersions(DependencyNode node)
      {
         if (!node.HasVersions)
         {
            var punditVersions = new Dictionary<NuGet.Versioning.NuGetVersion, SatisfyingInfo>();

            var versions = _cache.GetSatisfyingInfos(node)
               .Where(v => node.AllowedVersions.HasReleaseLabel || node.AllowedVersions.NuGetVersionRange.MinVersion.ReleaseLabels.Any() || !v.Version.IsPrerelease)
               .Where(v => node.AllowedVersions.Satisfies(v.Version))
               .Where(v => !punditVersions.ContainsKey(v.Version));

            foreach (var version in versions)
               punditVersions.Add(version.Version, new SatisfyingInfo(version.Version, version.Repo));
            
            node.SetVersions(punditVersions.Values);
         }

         if (!node.HasManifest)
            return;

         foreach (var child in node.Children)
            ResolveVersions(child);
      }

      private void ResolveManifests(DependencyNode node)
      {
         if (node.HasVersions && !node.HasManifest)
         {
            PackageManifest manifest = null;

            while (manifest == null && node.HasVersions)
            {
               //if (parentRepoType == RepositoryType.NuGet && node.ActiveRepository.Type != RepositoryType.NuGet)
               //   throw new NotSupportedException("NuGet packages can contain only NuGet packages");

               manifest = _cache.GetManifest(node);

               if (manifest != null)
               {
                  node.SetManifest(manifest);
                  break;
               }

               if (node.CanDowngrade)
                  node.RemoveActiveVersion();
               else
                  throw new ApplicationException("could not find manifest for node " + node.Path);
            }
         }

         if (!node.HasVersions)
            return;

         foreach (var child in node.Children)
            ResolveManifests(child);
      }

      private void FlattenNode(DependencyNode node, VersionResolutionTable collector)
      {
         if (node == null)
            return;

         if (!node.IsFull)
            throw new InvalidOperationException("Cannot flatten unresolved node");

         collector.Intersect(node.UnresolvedPackage, node.ActiveSatisfayingData);

         foreach (var child in node.Children)
            FlattenNode(child, collector);
      }

      private VersionResolutionTable Flatten()
      {
         var table = new VersionResolutionTable();

         foreach (var node in _rootDependencyNode.Children)
            FlattenNode(node, table);

         return table;
      }

      private static void FindNodes(DependencyNode rootNode, UnresolvedPackage package, ICollection<DependencyNode> collector)
      {
         if (rootNode.UnresolvedPackage.Equals(package))
            collector.Add(rootNode);

         foreach (DependencyNode child in rootNode.Children)
            FindNodes(child, package, collector);
      }

      public string DescribeConflict(DependencyNode rootNode, UnresolvedPackage package, ICollection<DependencyNode> collector)
      {
         var sb = new StringBuilder();
         var found = new List<DependencyNode>();

         FindNodes(rootNode, package, found);

         if (found.Count <= 0)
            return null;

         foreach (var node in found)
            collector.Add(node);

         AppendDependencyNode(found, sb);

         return sb.ToString();
      }

      public string PrintDependencyNodes(IEnumerable<DependencyNode> nodes)
      {
         var sb = new StringBuilder();
         AppendDependencyNode(nodes, sb);

         return sb.ToString();
      }

      private void AppendDependencyNode(IEnumerable<DependencyNode> nodes, StringBuilder sb)
      {
         foreach (DependencyNode node in nodes)
         {
            if (sb.Length != 0)
               sb.AppendLine();

            sb.Append("dependency: [");
            sb.Append(node.Path);
            sb.Append("], version: [");
            sb.Append(node.AllowedVersions);
            sb.Append("], resolved to: [");

            bool isFirst = true;
            foreach (NuGet.Versioning.NuGetVersion version in node.AllVersions)
            {
               if (!isFirst)
                  sb.Append(", ");
               else
                  isFirst = false;

               sb.Append(version);
            }

            sb.Append("]");
         }
      }

      private IEnumerable<SatisfyingInfo> GetAllAvailableVersions(DependencyNode node)
      {
         _writer.Text(".");

         return _activeRepositories.SelectMany(repo => repo.GetVersions(node.UnresolvedPackage).Select(v => new SatisfyingInfo(v, repo)));
      }
   }
}