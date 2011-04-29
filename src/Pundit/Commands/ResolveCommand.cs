using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NDesk.Options;
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

      private void ResolveParameters(out int depthIndex, out BuildConfiguration configuration, out bool force)
      {
         depthIndex = GetDepth();
         configuration = BuildConfiguration.Release;
         string sconfiguration = null;
         bool force1 = false;

         new OptionSet()
            .Add("c:|configuration:", c => sconfiguration = c)
            .Add("f|force", f => force1 = f != null)
            .Parse(GetCommandLine());

         force = force1;

         if (sconfiguration != null)
         {
            if (sconfiguration == "any")
               configuration = BuildConfiguration.Any;
            else if (sconfiguration == "debug")
               configuration = BuildConfiguration.Debug;
         }
      }

      private IEnumerable<IRepository> GetRepositories(int depth)
      {
         var names = LocalRepository.TakeFirstRegisteredNames(depth, true);

         return names.Select(n =>
                             RepositoryFactory.CreateFromUri(
                                LocalRepository.GetRepositoryUriFromName(n)));
      }

      private void PrintConflicts(DependencyResolution dr, VersionResolutionTable tbl, DependencyNode rootNode)
      {
         foreach(UnresolvedPackage conflict in tbl.GetConflictedPackages())
         {
            GlamTerm.WriteErrorLine(dr.DescribeConflict(rootNode, conflict));
         }
      }

      private void PrintSuccess(VersionResolutionTable tbl)
      {
         GlamTerm.WriteLine();

         foreach(var pck in tbl.GetPackages())
         {
            GlamTerm.Write(ConsoleColor.White, pck.PackageId);
            GlamTerm.Write(" ");
            GlamTerm.Write(ConsoleColor.Yellow, pck.Platform);
            GlamTerm.Write(" --> ");
            GlamTerm.WriteLine(ConsoleColor.Green, pck.Version.ToString());
         }

         //GlamTerm.WriteLine();
      }

      public override void Execute()
      {
         //parse command and read manifest
         string manifestPath = GetLocalManifest();
         string projectRoot = new FileInfo(manifestPath).Directory.FullName;
         int depth;
         bool force;
         BuildConfiguration configuration;
         ResolveParameters(out depth, out configuration, out force);

         GlamTerm.Write(ConsoleColor.White, "manifest:\t");
         GlamTerm.WriteLine(manifestPath);

         GlamTerm.Write(ConsoleColor.White, "depth:\t\t");
         GlamTerm.WriteLine(depth == int.MaxValue ? "max" : depth.ToString());

         GlamTerm.Write(ConsoleColor.White, "configuration:\t");
         GlamTerm.WriteLine(configuration == BuildConfiguration.Debug ? ConsoleColor.Yellow : ConsoleColor.Green,
                            configuration.ToString().ToLower());

         GlamTerm.Write("force:\t\t");
         if(force)
            GlamTerm.WriteLine(ConsoleColor.Red, "yes");
         else
            GlamTerm.WriteLine("no");
         GlamTerm.WriteLine();

         GlamTerm.Write("reading manifest...\t\t");
         DevPackage devPackage = DevPackage.FromStream(File.OpenRead(manifestPath));
         GlamTerm.WriteOk();

         //get the repository list
         IEnumerable<IRepository> repositories = GetRepositories(depth);

         //resolve dependencies
         GlamTerm.Write("resolving...\t\t\t");

         var dr = new DependencyResolution(devPackage, repositories.ToArray());
         var resolutionResult = dr.Resolve();

         GlamTerm.WriteBool(!resolutionResult.Item1.HasConflicts);

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
         LocalRepository.PackageDownloadToLocalRepositoryStarted += LocalRepository_PackageDownloadToLocalRepositoryStarted;
         LocalRepository.PackageDownloadToLocalRepositoryFinished += LocalRepository_PackageDownloadToLocalRepositoryFinished;
         LocalRepository.DownloadLocally(resolutionResult.Item1.GetPackages(),
            repositories.Skip(1));

         //install all packages
         var installer = new PackageInstaller(projectRoot,
            resolutionResult.Item1,
            repositories.First());
         installer.BeginInstallPackage += installer_BeginInstallPackage;
         installer.FinishInstallPackage += installer_FinishInstallPackage;

         bool changed = installer.InstallAll(configuration, force);

         if(!changed)
         {
            GlamTerm.WriteLine();
            GlamTerm.WriteLine(ConsoleColor.Green, "no changes");
         }
      }

      void installer_FinishInstallPackage(object sender, Core.Model.EventArguments.PackageKeyEventArgs e)
      {
         GlamTerm.Write("installing {0}...", e.PackageKey.PackageId);
      }

      void installer_BeginInstallPackage(object sender, Core.Model.EventArguments.PackageKeyEventArgs e)
      {
         GlamTerm.WriteBool(e.Succeeded);
      }

      void LocalRepository_PackageDownloadToLocalRepositoryFinished(object sender, Core.Model.EventArguments.PackageKeyEventArgs e)
      {
         GlamTerm.WriteBool(e.Succeeded);
      }

      void LocalRepository_PackageDownloadToLocalRepositoryStarted(object sender, Core.Model.EventArguments.PackageKeyEventArgs e)
      {
         GlamTerm.Write("downloading {0}...", e.PackageKey.PackageId);
      }
   }
}
