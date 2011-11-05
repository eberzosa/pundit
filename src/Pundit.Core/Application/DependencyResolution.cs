using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pundit.Core.Model;

namespace Pundit.Core.Application
{
   /// <summary>
   /// 
   /// </summary>
   public class DependencyResolution
   {
      private readonly ILocalRepository _repo;
      private readonly DependencyNode _root;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="rootManifest"></param>
      /// <param name="localRepository"></param>
      public DependencyResolution(Package rootManifest, ILocalRepository localRepository)
      {
         _repo = localRepository;
         _root = new DependencyNode(null, rootManifest.PackageId, rootManifest.Platform,
                                    new VersionPattern(rootManifest.Version.ToString()));
         _root.MarkAsRoot(rootManifest);
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
      public Tuple<VersionResolutionTable, DependencyNode> Resolve()
      {
         //steps 1-2-3
         ResolveAll(_root);

         //steps 4-5
         return Tuple.Create(Flatten(_root), _root);
      }

      private void ResolveAll(DependencyNode node)
      {
         while (!node.IsRecursivelyFull)
         {
            //first step: resolve version patterns
            //(at least root must have all the dependencies populated already)
            ResolveVersions(node);

            //second step: resolve manifests
            ResolveManifests(node);
         }
      }

      private void ResolveVersions(DependencyNode node)
      {
         //if(_log.IsDebugEnabled) _log.Debug("resolving " + node.Path);

         if (!node.HasVersions)
         {
            var versions = new HashSet<Version>();

            ICollection<Version> vs = _repo.GetVersions(node.UnresolvedPackage, node.VersionPattern);

            if (vs != null)
            {
               foreach (Version v in vs)
               {
                  versions.Add(v);
               }
            }

            node.SetVersions(versions.ToArray());
         }
         else
         {
            //if(_log.IsDebugEnabled) _log.Debug("node already has versions resolved");
         }

         if(node.HasManifest)
         {
            foreach(DependencyNode child in node.Children)
            {
               ResolveVersions(child);
            }
         }
      }

      private void ResolveManifests(DependencyNode node)
      {
         if(node.HasVersions && !node.HasManifest)
         {
            Package manifest = null;

            try
            {
               manifest = _repo.GetManifest(node.ActiveVersionKey);
            }
            catch(FileNotFoundException)
            {
                  
            }

            if(manifest == null)
               throw new ApplicationException("could not find manifest for node " + node.Path);

            node.SetManifest(manifest);
         }

         if(node.HasVersions)
         {
            foreach(var child in node.Children)
            {
               ResolveManifests(child);
            }
         }
      }

      private static void FlattenNode(DependencyNode node, VersionResolutionTable collector)
      {
         if(node != null)
         {
            if(!node.IsFull) throw new InvalidOperationException("Cannot flatten unresolved node");

            collector.Intersect(node.UnresolvedPackage, node.ActiveVersions);

            foreach(DependencyNode child in node.Children)
            {
               FlattenNode(child, collector);
            }
         }
      }

      /// <summary>
      /// Creates <see cref="VersionResolutionTable"/> from <see cref="DependencyNode"/>
      /// tree
      /// </summary>
      /// <param name="rootNode"></param>
      /// <returns></returns>
      private static VersionResolutionTable Flatten(DependencyNode rootNode)
      {
         var table = new VersionResolutionTable();

         foreach(var node in rootNode.Children)
            FlattenNode(node, table);

         return table;
      }

      private static void FindNodes(DependencyNode rootNode, UnresolvedPackage package, ICollection<DependencyNode> collector)
      {
         if(rootNode.UnresolvedPackage.Equals(package))
            collector.Add(rootNode);

         foreach(DependencyNode child in rootNode.Children)
            FindNodes(child, package, collector);
      }

      public string DescribeConflict(DependencyNode rootNode, UnresolvedPackage package)
      {
         var b = new StringBuilder();
         var found = new List<DependencyNode>();

         FindNodes(rootNode, package, found);

         if(found.Count > 0)
         {
            foreach (DependencyNode node in found)
            {
               if (b.Length != 0) b.AppendLine();

               b.Append("dependency: [");
               b.Append(node.Path);
               b.Append("], version: [");
               b.Append(node.VersionPattern);
               b.Append("], resolved to: [");

               bool isFirst = true;
               foreach (Version v in node.AllVersions)
               {
                  if (!isFirst)
                  {
                     b.Append(", ");
                  }
                  else
                  {
                     isFirst = false;
                  }

                  b.Append(v.ToString());
               }

               b.Append("]");
            }
         }

         return b.ToString();
      }
   }
}
