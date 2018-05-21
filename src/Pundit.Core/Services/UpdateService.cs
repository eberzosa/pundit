using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using EBerzosa.Pundit.Core.Framework;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Resolvers;
using EBerzosa.Pundit.Core.Versioning;
using EBerzosa.Utils;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;

namespace EBerzosa.Pundit.Core.Services
{
   public class UpdateService
   {  
      private readonly PackageInstallerFactory _packageInstallerFactory;
      private readonly DependencyResolution _dependencyResolution;

      private readonly RepositoryFactory _repositoryFactory;

      //private readonly IRepositoryManager _repositoryManager;
      private readonly IWriter _writer;
      
      public bool Force { get; set; }

      public bool DryRun { get; set; }

      public bool LocalReposOnly { get; set; }


      public UpdateService(RepositoryFactory repositoryFactory, PackageInstallerFactory packageInstallerFactory,
         DependencyResolution dependencyResolution, IWriter writer)
      {
         Guard.NotNull(repositoryFactory, nameof(repositoryFactory));
         Guard.NotNull(packageInstallerFactory, nameof(packageInstallerFactory));
         Guard.NotNull(dependencyResolution, nameof(dependencyResolution));
         Guard.NotNull(writer, nameof(writer));

         _packageInstallerFactory = packageInstallerFactory;
         _dependencyResolution = dependencyResolution;
         _repositoryFactory = repositoryFactory;
         _writer = writer;
      }

      public bool Execute()
      {
         var assembly = Assembly.GetEntryAssembly();

         var packageId = "EBerzosa.Pundit";
         var assemblyVersion = assembly.GetName().Version;
         var netFramework = PunditFramework.Parse("net46");

         var packageSpec = new PackageSpec
         {
            PackageId = packageId,
            Framework = netFramework,
            Version = new NuGet.Versioning.NuGetVersion(1, 0, 0, 0),
            Dependencies =
            {
               new PackageDependency(packageId, new VersionRangeExtended(NuGet.Versioning.NuGetVersion.Parse($"{assemblyVersion.Major}.*")))
               {
                  Scope = DependencyScope.Normal,
                  Framework = netFramework
               }
            }
         };
         
         packageSpec.Validate();

         _writer.BeginWrite().Text("Getting repositories...");

         var repos = _repositoryFactory.TryGetEnabledRepos(true, !LocalReposOnly).ToArray();

         if (repos.Length == 0)
         {
            _writer.Error(" no available repos").EndWrite();
            return false;
         }

         _writer.Text(" using repos:");
         foreach (var repository in repos)
            _writer.Info(repository.ToString());

         _writer.EndWrite();

         _writer.BeginWrite().Text("Resolving...");
         var resolutionResult = _dependencyResolution.Resolve(packageSpec, repos, null);

         if (resolutionResult.Item1.HasConflicts)
         {
            _writer.Error(" failed").EndWrite()
               .Empty()
               .BeginWrite().Error("Could not resolve manifest due to conflicts...").EndWrite();

            PrintConflicts(_dependencyResolution, resolutionResult.Item1, resolutionResult.Item2);

            return false;
         }

         _writer.Success(" ok").EndWrite();

         if (DryRun)
            return true;

         var currentPath = Path.GetDirectoryName(assembly.Location);

         if (!Install(resolutionResult, packageSpec, currentPath))
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
      
      private bool Install(Tuple<VersionResolutionTable, DependencyNode> resolutionResult, PackageSpec packageSpecSpecs, string folder)
      {
         var installed = false;

         var cacheRepository = new CacheRepository(_repositoryFactory.TryGetCacheRepos());
         cacheRepository.PackageDownloadToCacheRepositoryStarted += CacheRepositoryPackageDownloadToCacheRepositoryStarted;
         cacheRepository.PackageDownloadToCacheRepositoryFinished += CacheRepositoryOnPackageDownloadToCacheRepositoryFinished;

         cacheRepository.DownloadLocally(resolutionResult.Item1.GetSatisfyingInfos());

         cacheRepository.PackageDownloadToCacheRepositoryStarted -= CacheRepositoryPackageDownloadToCacheRepositoryStarted;
         cacheRepository.PackageDownloadToCacheRepositoryFinished -= CacheRepositoryOnPackageDownloadToCacheRepositoryFinished;

         using (var installer = _packageInstallerFactory.GetInstaller(folder, resolutionResult.Item1, packageSpecSpecs))
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
               var diff = installer.GetDiffWithCurrent(resolutionResult.Item1.GetSatisfyingInfos()).ToArray();

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
      
      private void CacheRepositoryPackageDownloadToCacheRepositoryStarted(object sender, PackageKeyEventArgs e)
      {
         _writer.Text($"Downloading {e.PackageKey.PackageId}... ");
      }

      private void CacheRepositoryOnPackageDownloadToCacheRepositoryFinished(object sender, PackageKeyEventArgs e)
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

      private void PrintConflicts(DependencyResolution dependencyResolution, VersionResolutionTable versionResolutionTable, DependencyNode dependencyNode)
      {
         var allNodes = new List<DependencyNode>();

         foreach (UnresolvedPackage conflict in versionResolutionTable.GetConflictedPackages())
            _writer.Error(dependencyResolution.DescribeConflict(dependencyNode, conflict, allNodes));

         _writer.Empty();
         _writer.Error(dependencyResolution.PrintDependencyNodes(allNodes.Distinct(DependencyNodeComparer.PackageId).OrderBy(n => n.PackageId)));
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
            _writer.Text($"{d.PackageId} v{(isMod ? d.OldPackageKey.Version : d.Version)} ({d.Framework})");

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
         _writer.Text(" done").EndWrite();
      }

   }
}
