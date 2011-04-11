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

      public DependencyNode Resolve()
      {
         //int step = 0;

         //while(!_root.Resolved)
         //{
            //if (_log.IsDebugEnabled) _log.DebugFormat("resolving (step {0})...", ++step);

            ResolveNext(_root);
         //}

         if(_log.IsDebugEnabled) _log.Debug("dependencies resolved");

         return _root;
      }

      private void ResolveNext(DependencyNode node)
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
   }
}
