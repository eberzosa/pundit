using System;
using System.Collections.Generic;
using System.Linq;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Utils;
using EBerzosa.Pundit.Core.Versioning;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Resolvers
{
   internal class DependencyResolverImplementation : IResolutionResult
   {
      private readonly IWriter _writer;
      private readonly DependencyResolverCache _cache;

      private readonly IRepository[] _activeRepositories;
      private readonly DependencyNode _rootDependencyNode;

      private bool _resolved;
      private VersionResolutionTable _table;


      public VersionResolutionTable ResolutionTable => _resolved ? _table : throw new InvalidOperationException($"Execute {nameof(Resolve)} first");

      public DependencyNode DependencyNode => _resolved ? _rootDependencyNode : throw new InvalidOperationException($"Execute {nameof(Resolve)} first");


      public DependencyResolverImplementation(IWriter writer, PackageManifestRoot rootManifest, IRepository[] activeRepositories, string releaseLabel)
      {
         _cache = new DependencyResolverCache(GetAllAvailableVersions, rootManifest.Framework);

         _writer = writer;
         _activeRepositories = activeRepositories;

         var version = new VersionRangeExtended(rootManifest.Version) { ReleaseLabel = releaseLabel };

         _rootDependencyNode = new DependencyNode(null, rootManifest.PackageId, rootManifest.Framework.GetShortFolderName(), version);
         _rootDependencyNode.MarkAsRoot(rootManifest);
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
      public void Resolve()
      {
         using (new TimeMeasurer(_writer.Text))
         {
            //steps 1-2-3
            ResolveAll();
         }

         //steps 4-5
         _table = Flatten();

         _resolved = true;
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
            while (node.HasVersions)
            {
               var manifest = _cache.GetManifest(node);

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
      
      private IEnumerable<SatisfyingInfo> GetAllAvailableVersions(DependencyNode node)
      {
         _writer.Text(".");

         return _activeRepositories.SelectMany(repo => repo.GetVersions(node.UnresolvedPackage).Select(v => new SatisfyingInfo(v, repo)));
      }
   }
}