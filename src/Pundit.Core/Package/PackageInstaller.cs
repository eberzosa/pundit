using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Resolvers;
using EBerzosa.Pundit.Core.Versioning;
using EBerzosa.Utils;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;

namespace EBerzosa.Pundit.Core.Package
{
   public class PackageInstaller : IPackageInstaller
   {
      private readonly PackageReaderFactory _packageReaderFactory;
      private readonly string _rootDirectory;
      private readonly VersionResolutionTable _versionTable;
      private readonly PackageSpec _manifest;
      private readonly ICollection<IRepository> _cacheRepositories;
      private InstalledPackagesIndex _index;
      private bool _indexCommitted;

      private readonly string _libFolderPath;
      private readonly string _includeFolderPath;
      private readonly string _toolsFolderPath;
      private readonly string _otherFolderPath;

      public event EventHandler<PackageKeyDiffEventArgs> BeginInstallPackage;
      public event EventHandler<PackageKeyDiffEventArgs> FinishInstallPackage;

      public PackageInstaller(PackageReaderFactory packageReaderFactory, string rootDirectory, VersionResolutionTable versionTable,
         PackageSpec manifest, ICollection<IRepository> cacheRepositories)
      {
         Guard.NotNull(packageReaderFactory, nameof(packageReaderFactory));
         Guard.NotNull(rootDirectory, nameof(rootDirectory));
         Guard.NotNull(versionTable, nameof(rootDirectory));
         Guard.NotNull(manifest, nameof(rootDirectory));
         Guard.NotNull(cacheRepositories, nameof(rootDirectory));
         
         _packageReaderFactory = packageReaderFactory;
         _rootDirectory = rootDirectory;
         _versionTable = versionTable;
         _manifest = manifest;
         _cacheRepositories = cacheRepositories;

         _libFolderPath = Path.Combine(rootDirectory, "lib");
         _includeFolderPath = Path.Combine(rootDirectory, "include");
         _toolsFolderPath = Path.Combine(rootDirectory, "tools");
         _otherFolderPath = Path.Combine(rootDirectory, "other");

         _index = InstalledPackagesIndex.ReadFromFolder(_rootDirectory);

         foreach (var package in versionTable.GetPackages())
            _manifest.Dependencies.Add(new PackageDependency(package.PackageId, new VersionRangeExtended(package.Version)) {Framework = package.Framework});
      }
      
      /// <summary>
      /// Given the resolution result as an input compares it to the current state of the solution.
      /// </summary>
      /// <param name="resolutionResult">Resolution result</param>
      /// <returns>Differences between the current state and the resolution result</returns>
      public IEnumerable<PackageKeyDiff> GetDiffWithCurrent(IEnumerable<SatisfyingInfoExtended> resolutionResult)
      {
         if (resolutionResult == null)
            throw new ArgumentNullException(nameof(resolutionResult));

         var diff = new List<PackageKeyDiff>();

         bool indexEmpty = _index == null || _index.InstalledPackages == null || _index.InstalledPackages.Length == 0;

         if(indexEmpty)
         {
            diff.AddRange(resolutionResult.Select(rr => new PackageKeyDiff(DiffType.Add, rr.GetPackageKey(), rr.RepoType)));
         }
         else
         {
            foreach (var rr in resolutionResult)
            {
               var pi = _index.InstalledPackages.FirstOrDefault(p => p.LooseEquals(rr.GetPackageKey()));

               if (pi == null)
                  diff.Add(new PackageKeyDiff(DiffType.Add, rr.GetPackageKey(), rr.RepoType));

               else if (rr.Equals(pi))
                  diff.Add(new PackageKeyDiff(DiffType.NoChange, rr.GetPackageKey(), null, rr.RepoType));

               else if (rr.Version > pi.Version)
                  diff.Add(new PackageKeyDiff(DiffType.Upgrade, rr.GetPackageKey(), pi, rr.RepoType));

               else if (rr.Version < pi.Version)
                  diff.Add(new PackageKeyDiff(DiffType.Downgrade, rr.GetPackageKey(), pi, rr.RepoType));
            }

            var deleted = _index.InstalledPackages
               .Where(ip => resolutionResult.FirstOrDefault(rr => rr.GetPackageKey().LooseEquals(ip)) == null)
               .Select(ip => new PackageKeyDiff(DiffType.Delete, ip, RepositoryType.Pundit));

            diff.AddRange(deleted);
         }

         return diff;
      }

      private void CleanupFolder(string fullPath)
      {
         if(Directory.Exists(fullPath))
         {
            foreach(string file in Directory.GetFiles(fullPath, "*", SearchOption.TopDirectoryOnly))
            {
               try
               {
                  File.Delete(file);
               }
               catch
               {
                  
               }
            }
         }
      }

      private void ClearAllFolders()
      {
         CleanupFolder(_libFolderPath);
         CleanupFolder(_includeFolderPath);
         CleanupFolder(_toolsFolderPath);
         CleanupFolder(_otherFolderPath);
      }

      private bool DependenciesChanged(IEnumerable<PackageKey> currentDependencies, BuildConfiguration configuration)
      {
         if (configuration != _index.Configuration)
            return true;

         if (currentDependencies.Count() != _index.TotalPackagesCount)
            return true;

         return currentDependencies.Any(pck => !_index.IsInstalled(pck));
      }

      private void ResetFiles(BuildConfiguration configuration)
      {
         ClearAllFolders();

         _index = new InstalledPackagesIndex {Configuration = configuration};
      }

      private void Install(PackageKeyDiff pck, PackageDependency originalDependency, BuildConfiguration configuration)
      {
         var repository = _cacheRepositories.First(r => r.Type == pck.PackageType);

         using (var s = repository.Download(pck))
         using (var reader = _packageReaderFactory.Get(pck.PackageType, s))
         {
            BeginInstallPackage?.Invoke(this, new PackageKeyDiffEventArgs(pck, true));
            reader.InstallTo(_rootDirectory, originalDependency, configuration);
         }

         _index.Install(pck);

         FinishInstallPackage?.Invoke(this, new PackageKeyDiffEventArgs(pck, true));
      }

      ///<summary>
      ///Forcibly reinstalls all the packages
      ///</summary>
      ///<param name="configuration"></param>
      public void Reinstall(BuildConfiguration configuration)
      {
         IEnumerable<SatisfyingInfoExtended> currentDependencies = _versionTable.GetSatisfyingInfos();

         ResetFiles(configuration);

         foreach (var pck in currentDependencies)
         {
            Install(new PackageKeyDiff(DiffType.Add, pck.GetPackageKey(), pck.RepoType), _manifest.GetPackageDependency(pck.GetPackageKey()), configuration);
         }

         _indexCommitted = true;
      }

      /// <summary>
      /// Upgrades the current configuration
      /// </summary>
      /// <param name="configuration"></param>
      /// <param name="diffs"></param>
      public void Upgrade(BuildConfiguration configuration, IEnumerable<PackageKeyDiff> diffs)
      {
         bool hasDeletes = diffs.FirstOrDefault(d => d.DiffType == DiffType.Delete) != null;

         if(hasDeletes)
         {
            Reinstall(configuration);
         }
         else
         {
            var installs = diffs.Where(d => d.DiffType == DiffType.Add || d.DiffType == DiffType.Upgrade || d.DiffType == DiffType.Downgrade);

            foreach(PackageKeyDiff diff in installs)
            {
               Install(diff, _manifest.GetPackageDependency(diff), configuration);
            }
         }

         _indexCommitted = true;
      }

      public void Dispose()
      {
         if(_index != null && _indexCommitted)
         {
            _index.WriteToFolder(_rootDirectory);
         }
      }
   }
}
