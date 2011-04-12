using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Pundit.Core.Model;

namespace Pundit.Core.Application
{
   public class DependencyResolution
   {
      private readonly IRepository[] _activeRepositories;
      private readonly DependencyNode _root;
      private readonly ILog _log = LogManager.GetLogger(typeof (DependencyResolution));

      public DependencyResolution(Package rootManifest, IRepository[] activeRepositories)
      {
         _activeRepositories = activeRepositories;

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
         while (!node.IsFull)
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
         if(_log.IsDebugEnabled) _log.Debug("resolving " + node.Path);

         if(!node.HasVersions)
         {
            var versions = new HashSet<Version>();

            foreach(IRepository repo in _activeRepositories)
            {
               Version[] vs = repo.GetVersions(node.PackageId, node.Platform, node.VersionPattern);

               if(vs != null)
               {
                  foreach(Version v in vs)
                  {
                     versions.Add(v);
                  }
               }
            }

            node.SetVersions(versions.ToArray());
         }
         else
         {
            if(_log.IsDebugEnabled) _log.Debug("node already has versions resolved");
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
         if(node.HasVersions && !node.HasManifest && node.ActiveVersion != null)
         {
            Package manifest = null;

            foreach(var repo in _activeRepositories)
            {
               manifest = repo.GetManifest(node.ActiveVersionKey);

               if (manifest != null) break;
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

      private VersionResolutionTable Flatten(DependencyNode rootNode)
      {
         var table = new VersionResolutionTable();

         return table;
      }
   }
}
