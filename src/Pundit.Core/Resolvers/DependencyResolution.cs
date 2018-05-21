using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EBerzosa.Pundit.Core.Converters;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Versioning;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Resolvers
{
   public class DependencyResolution
   {
      private readonly IWriter _writer;

      private readonly IRepository[] _activeRepositories;
      private readonly DependencyNode _rootDependencyNode;

      public DependencyResolution(IWriter writer)
      {
         _writer = writer;
      }

      private DependencyResolution(IWriter writer, PackageManifest rootManifest, IRepository[] activeRepositories, string releaseLabel)
      {
         _writer = writer;
         _activeRepositories = activeRepositories;

         var version = new VersionRangeExtended(rootManifest.Version) {ReleaseLabel = releaseLabel};

         _rootDependencyNode = new DependencyNode(null, rootManifest.PackageId, rootManifest.Framework, version);
         _rootDependencyNode.MarkAsRoot(rootManifest);
      }

      public Tuple<VersionResolutionTable, DependencyNode> Resolve(PackageManifest rootManifest, IRepository[] activeRepositories,
         string releaseLabel)
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
         //steps 1-2-3
         ResolveAll();

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

            foreach (var repo in _activeRepositories)
            {
               var versions = repo.GetVersions(node.UnresolvedPackage);

               if (versions == null)
                  continue;

               foreach (var version in versions)
                  if (node.AllowedVersions.Satisfies(version) && !punditVersions.ContainsKey(version))
                     punditVersions.Add(version, new SatisfyingInfo(version, repo));
            }

            node.SetVersions(punditVersions.Values);
         }

         if (!node.HasManifest)
            return;

         _writer.Text("´");

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
               try
               {
                  manifest = node.ActiveRepository.GetManifest(node.ActiveVersionKey);

                  if (manifest != null)
                  {
                     node.SetManifest(manifest);
                     break;
                  }

                  if (node.ActiveSatisfayingData.Any())
                     node.RemoveActiveVersion();
                  else
                     throw new ApplicationException("could not find manifest for node " + node.Path);
               }
               catch
               {
               }
            }
         }

         if (!node.HasVersions)
            return;

         foreach (var child in node.Children)
            ResolveManifests(child);
      }

      private void FlattenNode(DependencyNode node, VersionResolutionTable collector)
      {
         _writer.Text("`");

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
   }
}