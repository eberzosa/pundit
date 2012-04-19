using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;

namespace Pundit.Core.Application.Console.Commands
{
   class ResolveConsoleCommand : BaseConsoleCommand
   {
      private BuildConfiguration _buildConfiguration;
      private bool _forceResolve;
      private bool _pingOnly;
      private int _packagesDownloaded;
      private int _packagesInstalled;

      public ResolveConsoleCommand(IConsoleOutput console, string currentDirectory, string[] args)
         : base(console, currentDirectory, args)
      {
      }

      private void Initialize()
      {
         _buildConfiguration = BuildConfiguration.Release;
         _forceResolve = GetBoolParameter(false, "f|force");
         _pingOnly = GetBoolParameter(false, "p|ping");

         string sconfig = GetParameter("c:|configuration:");
         if (sconfig != null)
         {
            if (sconfig == "any")
               _buildConfiguration = BuildConfiguration.Any;
            else if (sconfig == "debug")
               _buildConfiguration = BuildConfiguration.Debug;
         }
      }

      private void PrintConflicts(DependencyResolution dr, VersionResolutionTable tbl, DependencyNode rootNode)
      {
         foreach(UnresolvedPackage conflict in tbl.GetConflictedPackages())
         {
            console.WriteLine(ConsoleColor.Red, dr.DescribeConflict(rootNode, conflict));
         }
      }

      private void PrintDiff(IEnumerable<PackageKeyDiff> diffs, ConsoleColor diffColor, DiffType diffType, string diffWord)
      {
         bool isMod = diffType == DiffType.Mod;

         foreach(PackageKeyDiff d in diffs.Where(pd => pd.DiffType == diffType))
         {
            console.Write(diffColor, diffWord + " ");
            console.Write("{0} v{1} ({2})", d.PackageId,
               isMod ? d.OldPackageKey.Version : d.Version, d.Platform);

            if(isMod)
            {
               console.Write(" to v");
               console.Write(ConsoleColor.Green, d.Version.ToString());
            }

            console.WriteLine("");
         }
      }

      private void PrintSuccess(IEnumerable<PackageKeyDiff> diffs)
      {
         PrintDiff(diffs, ConsoleColor.Yellow, DiffType.Add, "added");
         PrintDiff(diffs, ConsoleColor.Green, DiffType.Mod, "upgraded");
         PrintDiff(diffs, ConsoleColor.White, DiffType.NoChange, "no change for");
         PrintDiff(diffs, ConsoleColor.Red, DiffType.Del, "deleted");
      }

      public override void Execute()
      {
         Initialize();

         new RepoConsoleCommand(console, CurrentDirectory, null).UpdateSnapshots(_forceResolve);

         //parse command and read manifest
         string manifestPath = GetLocalManifest();
         string projectRoot = new FileInfo(manifestPath).Directory.FullName;

         console.Write(ConsoleColor.White, "manifest:\t");
         console.WriteLine(manifestPath);

         console.Write(ConsoleColor.White, "configuration:\t");
         console.WriteLine(_buildConfiguration == BuildConfiguration.Debug ? ConsoleColor.Yellow : ConsoleColor.Green,
                            _buildConfiguration.ToString().ToLower());

         console.Write("force:\t\t");
         if(_forceResolve)
            console.WriteLine(ConsoleColor.Red, "yes");
         else
            console.WriteLine("no");

         console.Write("ping only:\t");
         if(_pingOnly)
            console.WriteLine(ConsoleColor.Green, "yes");
         else
            console.WriteLine("no");

         console.WriteLine("");


         console.Write("reading manifest...\t\t");
         DevPackage devPackage = DevPackage.FromStream(File.OpenRead(manifestPath));
         console.Write(true);

         //resolve dependencies
         console.Write("resolving...\t\t\t");

         //System.Console.ReadKey();
         var dr = new DependencyResolution(devPackage, LocalConfiguration.RepositoryManager.LocalRepository);
         var resolutionResult = dr.Resolve();

         console.Write(!resolutionResult.Item1.HasConflicts);

         if (_pingOnly) return;

         if(resolutionResult.Item1.HasConflicts)
         {
            PrintConflicts(dr, resolutionResult.Item1, resolutionResult.Item2);

            throw new ApplicationException("could not resolve manifest because of conflicts");
         }

         //ensure that all packages exist in local repository
         ICollection<PackageKey> forDownload = LocalConfiguration.GetForDownload(resolutionResult.Item1.GetPackages());
         if (forDownload.Count > 0)
         {
            console.WriteLine("downloading {0} packages...", forDownload.Count);
            console.StartProgress(forDownload.Count);
            LocalConfiguration.PackageDownloadToLocalRepository += LocalRepository_PackageDownloadToLocalRepository;
            LocalConfiguration.DownloadLocally(forDownload);
            console.FinishProgress();
         }

         //install all packages
         using(PackageInstaller installer = new PackageInstaller(projectRoot,
            resolutionResult.Item1,
            devPackage,
            LocalConfiguration.RepositoryManager.LocalRepository))
         {
            installer.BeginInstallPackage += installer_BeginInstallPackage;
            installer.FinishInstallPackage += installer_FinishInstallPackage;

            if (_forceResolve)
            {
               int n = resolutionResult.Item1.GetPackages().Count();
               console.WriteLine("reinstalling {0} packages...", n);
               console.StartProgress(n);

               installer.Reinstall(_buildConfiguration);
            }
            else
            {
               IEnumerable<PackageKeyDiff> diff = installer.GetDiffWithCurrent(resolutionResult.Item1.GetPackages());

               PrintSuccess(diff);

               installer.Upgrade(_buildConfiguration, diff);
            }
            console.FinishProgress();
         }
      }

      void installer_FinishInstallPackage(object sender, PackageKeyDiffEventArgs e)
      {
         //console.Write(e.Succeeded);
         console.UpdateProgress(_packagesInstalled++);
      }

      void installer_BeginInstallPackage(object sender, PackageKeyDiffEventArgs e)
      {
         //console.WriteLine("installing {0} v{1} ({2})...", e.PackageKeyDiff.PackageId, e.PackageKeyDiff.Version, e.PackageKeyDiff.Platform);
         //console.UpdateProgress(_packagesInstalled++);
      }

      void LocalRepository_PackageDownloadToLocalRepository(object sender, PackageDownloadEventArgs e)
      {
         console.UpdateProgress(_packagesDownloaded++);
      }
   }
}
