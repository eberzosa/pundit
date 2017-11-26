using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Utils;
using Pundit.Core;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;

namespace EBerzosa.Pundit.Core.Services
{
   public class UpdateService
   {
      private readonly LocalRepository _localRepository;
      
      private readonly PackageReaderFactory _packageReaderFactory;

      private readonly RepositoryFactory _repositoryFactory;

      //private readonly IRepositoryManager _repositoryManager;
      private readonly IWriter _writer;
      
      public bool Force { get; set; }

      public bool DryRun { get; set; }

      public bool LocalReposOnly { get; set; }

      public bool IncludeDeveloperPackages { get; set; }


      public UpdateService(LocalRepository localRepository, RepositoryFactory repositoryFactory, PackageReaderFactory packageReaderFactory,
         IWriter writer)
      {
         Guard.NotNull(localRepository, nameof(localRepository));
         Guard.NotNull(repositoryFactory, nameof(repositoryFactory));
         Guard.NotNull(packageReaderFactory, nameof(packageReaderFactory));
         Guard.NotNull(writer, nameof(writer));

         _localRepository = localRepository;
         _packageReaderFactory = packageReaderFactory;
         _repositoryFactory = repositoryFactory;
         _writer = writer;
      }

      public bool Execute()
      {
         var assembly = Assembly.GetEntryAssembly();

         var packageId = "EBerzosa.Pundit";
         var assemblyVersion = assembly.GetName().Version;
         var netFramework = "net46";

         var packageSpec = new PackageSpec
         {
            PackageId = packageId,
            Platform = netFramework,
            Version = new PunditVersion(1, 0, 0, 0, false),
            Dependencies =
            {
               new PackageDependency(packageId, $"{assemblyVersion.Major}.{assemblyVersion.Minor}")
               {
                  Scope = DependencyScope.Normal,
                  Platform = netFramework
               }
            }
         };
         
         packageSpec.Validate();

         var repos = GetRepositories(LocalReposOnly ? 0 : int.MaxValue).ToArray();

         _writer.BeginWrite().Text("Resolving... ");
         var dependencyResolution = new DependencyResolution(packageSpec, repos, IncludeDeveloperPackages);
         var resolutionResult = dependencyResolution.Resolve();

         if (resolutionResult.Item1.HasConflicts)
         {
            _writer.Error("failed").EndWrite()
               .Empty()
               .BeginWrite().Error("Could not resolve manifest due to conflicts...").EndWrite();

            PrintConflicts(dependencyResolution, resolutionResult.Item1, resolutionResult.Item2);

            return false;
         }

         _writer.Success("ok").EndWrite();

         if (DryRun)
            return true;

         var currentPath = Path.GetDirectoryName(assembly.Location);

         if (!Install(resolutionResult, repos, packageSpec, currentPath))
            return true;

         var oldPath = Path.Combine(currentPath, "old");
         var updatePath = Path.Combine(currentPath, "lib");

         if (Directory.Exists(oldPath))
         {
            foreach (var file in Directory.EnumerateFiles(oldPath))
               File.Delete(file);
         }
         else
         {
            Directory.CreateDirectory(oldPath);
         }

         foreach (var originalFile in Directory.EnumerateFiles(currentPath))
         {
            if (Path.GetFileNameWithoutExtension(originalFile) == string.Empty)
               continue;

            File.Move(originalFile, Path.Combine(oldPath, Path.GetFileName(originalFile)));
         }

         foreach (var newFile in Directory.EnumerateFiles(updatePath))
            File.Move(newFile, Path.Combine(currentPath, Path.GetFileName(newFile)));

         Directory.Delete(updatePath, false);

         return true;
      }
      
      private bool Install(Tuple<VersionResolutionTable, DependencyNode> resolutionResult, IRepository[] repos, PackageSpec packageSpecSpecs, string folder)
      {
         var installed = false;

         _localRepository.PackageDownloadToLocalRepositoryStarted += LocalRepository_PackageDownloadToLocalRepositoryStarted;
         _localRepository.PackageDownloadToLocalRepositoryFinished += LocalRepositoryOnPackageDownloadToLocalRepositoryFinished;

         _localRepository.DownloadLocally(resolutionResult.Item1.GetPackages(), repos.Skip(1));

         _localRepository.PackageDownloadToLocalRepositoryStarted -= LocalRepository_PackageDownloadToLocalRepositoryStarted;
         _localRepository.PackageDownloadToLocalRepositoryFinished -= LocalRepositoryOnPackageDownloadToLocalRepositoryFinished;

         using (var installer = new PackageInstaller(_packageReaderFactory, folder, resolutionResult.Item1, packageSpecSpecs, repos.First()))
         {
            installer.BeginInstallPackage += BeginInstallPackage;
            installer.FinishInstallPackage += FinishInstallPackage;

            if (Force)
            {
               _writer.Text($"Reinstalling {resolutionResult.Item1.GetPackages().Count()} packages... ");
               installer.Reinstall(BuildConfiguration.Release);
               installed = true;
            }
            else
            {
               var diff = installer.GetDiffWithCurrent(resolutionResult.Item1.GetPackages()).ToArray();

               int changed = PrintSuccess(diff);
               if (changed > 0)
               {
                  installer.Upgrade(BuildConfiguration.Release, diff);
                  installed = true;
               }
               else
                  _writer.Success("No changes detected");
            }

            installer.BeginInstallPackage -= BeginInstallPackage;
            installer.FinishInstallPackage -= FinishInstallPackage;
         }

         return installed;
      }
      
      private void LocalRepository_PackageDownloadToLocalRepositoryStarted(object sender, PackageKeyEventArgs e)
      {
         _writer.Text($"Downloading {e.PackageKey.PackageId}... ");
      }

      private void LocalRepositoryOnPackageDownloadToLocalRepositoryFinished(object sender, PackageKeyEventArgs e)
      {
         if (e.Succeeded)
            _writer.Success("ok");
         else
            _writer.Error("failed");
      }

      private void PrintSettings()
      {
         _writer.BeginColumns(new int?[] {18, null})
            .Reserved("Configuration:");
         
         _writer.Reserved("Local Repos Only:");
         if (LocalReposOnly)
            _writer.Warning("yes");
         else
            _writer.Text("no");

         _writer.Reserved("Force:");
         if (Force)
            _writer.Error("yes");
         else
            _writer.Text("no");

         _writer.BeginWrite().Reserved("Dry Run:");
         if (DryRun)
            _writer.Success("yes");
         else
            _writer.Text("no");

         _writer.EndColumns();
      }

      private IEnumerable<IRepository> GetRepositories(int depth)
      {
         var names = _localRepository.TakeFirstRegisteredNames(depth, true);

         return names.Select(n => _repositoryFactory.CreateFromUri(_localRepository.GetRepositoryUriFromName(n)));
      }

      private void PrintConflicts(DependencyResolution dependencyResolution, VersionResolutionTable versionResolutionTable, DependencyNode dependencyNode)
      {
         foreach (UnresolvedPackage conflict in versionResolutionTable.GetConflictedPackages())
         {
            _writer.Error(dependencyResolution.DescribeConflict(dependencyNode, conflict));
         }
      }

      private int PrintSuccess(IEnumerable<PackageKeyDiff> diffs1)
      {
         var diffs = new List<PackageKeyDiff>(diffs1);
         PrintDiff(diffs, _writer.Warning, DiffType.Add, "Added");
         PrintDiff(diffs, _writer.Success, DiffType.Upgrade, "Upgraded");
         PrintDiff(diffs, _writer.Error, DiffType.Downgrade, "Downgraded");
         PrintDiff(diffs, _writer.Reserved, DiffType.NoChange, "No change");
         PrintDiff(diffs, _writer.Error, DiffType.Delete, "Deleted");

         return diffs.Count(d => d.DiffType != DiffType.NoChange);
      }

      private void PrintDiff(IEnumerable<PackageKeyDiff> diffs, Func<string, IWriter> writer, DiffType diffType, string diffWord)
      {
         bool isMod = diffType == DiffType.Upgrade || diffType == DiffType.Downgrade;

         foreach (PackageKeyDiff d in diffs.Where(pd => pd.DiffType == diffType))
         {
            _writer.BeginWrite();

            writer(diffWord + " ");
            _writer.Text($"{d.PackageId} v{(isMod ? d.OldPackageKey.Version : d.Version)} ({d.Platform})");

            if (isMod)
            {
               _writer.Text(" to v");
               _writer.Success(d.Version.ToString());
            }

            _writer.EndWrite();
         }
      }

      private void BeginInstallPackage(object sender, PackageKeyDiffEventArgs e)
      {
         _writer.BeginWrite().Text($"Installing {e.PackageKeyDiff}...");
      }

      void FinishInstallPackage(object sender, PackageKeyDiffEventArgs e)
      {
         _writer.Text(" Done").EndWrite();
      }

   }
}
