using System;
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
            console.WriteLine(ConsoleColor.Red, DependencyResolution.DescribeConflict(rootNode, conflict));
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

      private int PrintSuccess(IEnumerable<PackageKeyDiff> diffs1)
      {
         var diffs = new List<PackageKeyDiff>(diffs1);
         PrintDiff(diffs, ConsoleColor.Yellow, DiffType.Add, "added");
         PrintDiff(diffs, ConsoleColor.Green, DiffType.Mod, "upgraded");
         PrintDiff(diffs, ConsoleColor.White, DiffType.NoChange, "no change for");
         PrintDiff(diffs, ConsoleColor.Red, DiffType.Del, "deleted");

         return diffs.Count(d => d.DiffType != DiffType.NoChange);
      }

      public override void Execute()
      {
         Initialize();

         new RepoConsoleCommand(console, CurrentDirectory, null).UpdateSnapshots(_forceResolve);
         console.WriteLine(null);

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


         console.Write("reading... ");
         DevPackage devPackage = DevPackage.FromStreamXml(File.OpenRead(manifestPath));
         devPackage.Validate();

         //resolve dependencies
         console.Write("resolving... ");

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
            LocalConfiguration.PackageDownloadToLocalRepository += PackageDownloadProgress;
            LocalConfiguration.DownloadLocally(forDownload);
         }

         //install all packages
         using(var installer = new PackageInstaller(projectRoot,
            resolutionResult.Item1,
            devPackage,
            LocalConfiguration.RepositoryManager.LocalRepository))
         {
            installer.BeginInstallPackage += BeginInstallPackage;
            installer.FinishInstallPackage += FinishInstallPackage;

            if (_forceResolve)
            {
               int n = resolutionResult.Item1.GetPackages().Count();
               console.WriteLine("reinstalling {0} packages...", n);
               installer.Reinstall(_buildConfiguration);
            }
            else
            {
               ICollection<PackageKeyDiff> diff = installer.GetDiffWithCurrent(resolutionResult.Item1.GetPackages());
               int changed = PrintSuccess(diff);
               if (changed > 0)
               {
                  installer.Upgrade(_buildConfiguration, diff);
               }
               else
               {
                  console.WriteLine(ConsoleColor.Green, "no changes detected");
               }
            }
         }
      }

      void FinishInstallPackage(object sender, PackageKeyDiffEventArgs e)
      {
         console.Write(true);
      }

      void BeginInstallPackage(object sender, PackageKeyDiffEventArgs e)
      {
         console.Write("installing {0}...", e.PackageKeyDiff);
      }

      private string _lastDownloadBytes;
      private int _lastDownloadPerc;
      void PackageDownloadProgress(object sender, PackageDownloadEventArgs e)
      {
         string progress = ByteFormat.ToString((ulong) e.DownloadedSize);
         double perc = (e.DownloadedSize * 100.0 / e.TotalSize);
         int iperc = e.DownloadedSize == e.TotalSize ? 100 : (int)perc;

         if (progress != _lastDownloadBytes || iperc != _lastDownloadPerc)
         {
            _lastDownloadBytes = progress;
            _lastDownloadPerc = iperc;
            console.ReturnCarriage();
            console.Write(ConsoleColor.White, e.PackageKey.ToString());
            console.Write(": ");
            console.Write(ConsoleColor.Yellow, progress);
            console.Write(ConsoleColor.White, " of ");
            console.Write(ConsoleColor.Yellow, ByteFormat.ToString((ulong) e.TotalSize));
            console.Write(ConsoleColor.White, " - {0}% - {1}/sec",
                           iperc,
                           ByteFormat.ToString((ulong) e.AvgSpeed));
            if (iperc == 100)
            {
               console.Write(true);
            }
            else
            {
               console.ClearToEnd();
            }
         }
      }
   }
}
