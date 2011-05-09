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

      private void PrintDiff(IEnumerable<PackageKeyDiff> diffs, ConsoleColor diffColor, DiffType diffType, string diffWord)
      {
         bool isMod = diffType == DiffType.Mod;

         foreach(PackageKeyDiff d in diffs.Where(pd => pd.DiffType == diffType))
         {
            GlamTerm.Write(diffColor, diffWord + " ");
            GlamTerm.Write("{0} v{1} ({2})", d.PackageId,
               isMod ? d.OldPackageKey.Version : d.Version, d.Platform);

            if(isMod)
            {
               GlamTerm.Write(" to v");
               GlamTerm.Write(ConsoleColor.Green, d.Version.ToString());
            }

            GlamTerm.WriteLine();
         }
      }

      private void PrintSuccess(IEnumerable<PackageKeyDiff> diffs)
      {
         PrintDiff(diffs, ConsoleColor.Yellow, DiffType.Add, "added");
         PrintDiff(diffs, ConsoleColor.Green, DiffType.Mod, "changed");
         PrintDiff(diffs, GlamTerm.ForeNormalColor, DiffType.NoChange, "no change for");
         PrintDiff(diffs, ConsoleColor.Red, DiffType.Del, "deleted");
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

         //ensure that all packages exist in local repository
         LocalRepository.PackageDownloadToLocalRepositoryStarted += LocalRepository_PackageDownloadToLocalRepositoryStarted;
         LocalRepository.PackageDownloadToLocalRepositoryFinished += LocalRepository_PackageDownloadToLocalRepositoryFinished;
         LocalRepository.DownloadLocally(resolutionResult.Item1.GetPackages(),
            repositories.Skip(1));

         //install all packages
         using(PackageInstaller installer = new PackageInstaller(projectRoot,
            resolutionResult.Item1,
            repositories.First()))
         {
            installer.BeginInstallPackage += installer_BeginInstallPackage;
            installer.FinishInstallPackage += installer_FinishInstallPackage;

            if (force)
            {
               GlamTerm.WriteLine("reinstalling all packages");

               installer.Reinstall(configuration);
            }
            else
            {
               IEnumerable<PackageKeyDiff> diff = installer.GetDiffWithCurrent(resolutionResult.Item1.GetPackages());

               PrintSuccess(diff);

               installer.Upgrade(configuration, diff);
            }
         }
      }

      void installer_FinishInstallPackage(object sender, Core.Model.EventArguments.PackageKeyDiffEventArgs e)
      {
         GlamTerm.WriteBool(e.Succeeded);
      }

      void installer_BeginInstallPackage(object sender, Core.Model.EventArguments.PackageKeyDiffEventArgs e)
      {
         GlamTerm.Write("installing {0} v{1}...", e.PackageKeyDiff.PackageId, e.PackageKeyDiff.Version);
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
