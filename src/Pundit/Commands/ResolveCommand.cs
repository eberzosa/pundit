using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using NDesk.Options;
using Pundit.Console.Repository;
using Pundit.Core;
using Pundit.Core.Application;
using Pundit.Core.Model;

namespace Pundit.Console.Commands
{
   class ResolveCommand : BaseCommand
   {
      public ResolveCommand(string[] args) : base(args)
      {
      }

      private void ResolveParameters(out int depthIndex)
      {
         depthIndex = int.MaxValue;
         string sdepth = null;

         new OptionSet().Add("d:|depth:", d => sdepth = d).Parse(GetCommandLine());

         if(sdepth != null)
         {
            if(sdepth == "local")
               depthIndex = 0;
            else if (!int.TryParse(sdepth, out depthIndex))
               throw new ArgumentException("wrong depth: " + sdepth);
         }
      }

      private IEnumerable<IRepository> GetRepositories(int depth)
      {
         var names = LocalRepository.TakeFirstRegisteredNames(depth, true);

         Log.Info("participating repisitories:");
         foreach(string name in names) Log.Info("  " + name);

         return names.Select(n =>
                             RepositoryFactory.CreateFromUri(
                                LocalRepository.GetRepositoryUriFromName(n)));
      }

      private void PrintConflicts(DependencyResolution dr, VersionResolutionTable tbl, DependencyNode rootNode)
      {
         Log.Error("Found conflicts!!!");

         foreach(UnresolvedPackage conflict in tbl.GetConflictedPackages())
         {
            Log.Error(dr.DescribeConflict(rootNode, conflict));
         }
      }

      public override void Execute()
      {
         string manifestPath = GetLocalManifest();
         int depth;
         ResolveParameters(out depth);

         Log.InfoFormat("manifest: {0}", manifestPath);
         Log.InfoFormat("depth: {0}", depth == int.MaxValue ? "max" : depth.ToString());

         Log.Info("reading manifest...");
         DevPackage devPackage = DevPackage.FromStream(File.OpenRead(manifestPath));

         Log.InfoFormat("getting repositories up to depth {0}", depth);
         IEnumerable<IRepository> repositories = GetRepositories(depth);

         Log.Info("resolving dependencies...");
         DependencyResolution dr = new DependencyResolution(devPackage, repositories.ToArray());
         var resolutionResult = dr.Resolve();

         if(resolutionResult.Item1.HasConflicts)
         {
            PrintConflicts(dr, resolutionResult.Item1, resolutionResult.Item2);

            throw new ApplicationException("could not resolve manifest because of conflicts");
         }
         else
         {
            
         }
      }
   }
}
