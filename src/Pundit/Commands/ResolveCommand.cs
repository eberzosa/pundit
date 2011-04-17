﻿using System;
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

      private void ResolveParameters(out int depthIndex, out BuildConfiguration configuration)
      {
         depthIndex = int.MaxValue;
         configuration = BuildConfiguration.Release;
         string sdepth = null;
         string sconfiguration = null;

         new OptionSet()
            .Add("d:|depth:", d => sdepth = d)
            .Add("c:|configuration:", c => sconfiguration = c)
            .Parse(GetCommandLine());

         if(sdepth != null)
         {
            if(sdepth == "local")
               depthIndex = 0;
            else if (!int.TryParse(sdepth, out depthIndex))
               throw new ArgumentException("wrong depth: " + sdepth);
         }

         if (sconfiguration != null && sconfiguration == "debug")
            configuration = BuildConfiguration.Debug;
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

      private void PrintSuccess(VersionResolutionTable tbl)
      {
         Log.Info("Dependencies resolved:");

         foreach(var pck in tbl.GetPackages())
         {
            Log.InfoFormat("  > [{0}] ({1}) --> v{2}", pck.PackageId, pck.Platform, pck.Version);
         }
      }

      public override void Execute()
      {
         //parse command and read manifest
         string manifestPath = GetLocalManifest();
         string projectRoot = new FileInfo(manifestPath).Directory.FullName;
         int depth;
         BuildConfiguration configuration;
         ResolveParameters(out depth, out configuration);

         Log.InfoFormat("manifest: {0}", manifestPath);
         Log.InfoFormat("depth: {0}", depth == int.MaxValue ? "max" : depth.ToString());
         Log.InfoFormat("configuration: {0}", configuration);

         Log.Info("reading manifest...");
         DevPackage devPackage = DevPackage.FromStream(File.OpenRead(manifestPath));

         //get the repository list
         Log.InfoFormat("getting repositories up to depth {0}", depth);
         IEnumerable<IRepository> repositories = GetRepositories(depth);

         //resolve dependencies
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
            PrintSuccess(resolutionResult.Item1);
         }

         //ensure that all packages exist in local repository
         LocalRepository.DownloadLocally(resolutionResult.Item1.GetPackages(),
            repositories.Skip(1));

         //install all packages
         Log.Info("Installing packages");
         var installer = new PackageInstaller(projectRoot,
            resolutionResult.Item1,
            repositories.First());
         installer.InstallAll(configuration);
      }
   }
}