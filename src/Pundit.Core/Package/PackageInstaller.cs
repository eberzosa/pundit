using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Repository;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;

namespace Pundit.Core.Application
{
   public class PackageInstaller : IDisposable
   {
      private readonly PackageReaderFactory _packageReaderFactory;
      private readonly string _rootDirectory;
      private readonly VersionResolutionTable _versionTable;
      private readonly PackageSpec _manifest;
      private readonly IRepository _localRepository;
      private InstalledPackagesIndex _index;
      private bool _indexCommitted;

      private readonly string _libFolderPath;
      private readonly string _includeFolderPath;
      private readonly string _toolsFolderPath;
      private readonly string _otherFolderPath;

      public event EventHandler<PackageKeyDiffEventArgs> BeginInstallPackage;
      public event EventHandler<PackageKeyDiffEventArgs> FinishInstallPackage;

      public PackageInstaller(PackageReaderFactory packageReaderFactory, string rootDirectory, VersionResolutionTable versionTable,
         PackageSpec manifest,
         IRepository localRepository)
      {
         if (rootDirectory == null) throw new ArgumentNullException("rootDirectory");
         if (versionTable == null) throw new ArgumentNullException("versionTable");
         if (manifest == null) throw new ArgumentNullException("manifest");
         if (localRepository == null) throw new ArgumentNullException("localRepository");

         _packageReaderFactory = packageReaderFactory;
         _rootDirectory = rootDirectory;
         _versionTable = versionTable;
         _manifest = manifest;
         _localRepository = localRepository;

         _libFolderPath = Path.Combine(rootDirectory, "lib");
         _includeFolderPath = Path.Combine(rootDirectory, "include");
         _toolsFolderPath = Path.Combine(rootDirectory, "tools");
         _otherFolderPath = Path.Combine(rootDirectory, "other");

         _index = InstalledPackagesIndex.ReadFromFolder(_rootDirectory);
      }

      /// <summary>
      /// Given the resolution result as an input compares it to the current state of the solution.
      /// </summary>
      /// <param name="resolutionResult">Resolution result</param>
      /// <returns>Differences between the current state and the resolution result</returns>
      public IEnumerable<PackageKeyDiff> GetDiffWithCurrent(IEnumerable<PackageKey> resolutionResult)
      {
         if (resolutionResult == null) throw new ArgumentNullException("resolutionResult");

         var diff = new List<PackageKeyDiff>();

         bool indexEmpty = _index == null || _index.InstalledPackages == null || _index.InstalledPackages.Length == 0;

         if(indexEmpty)
         {
            diff.AddRange(resolutionResult.Select(rr => new PackageKeyDiff(DiffType.Add, rr)));
         }
         else
         {
            foreach (var rr in resolutionResult)
            {
               var pi = _index.InstalledPackages.FirstOrDefault(p => p.LooseEquals(rr));

               if (pi == null)
                  diff.Add(new PackageKeyDiff(DiffType.Add, rr));

               else if (rr.Equals(pi))
                  diff.Add(new PackageKeyDiff(DiffType.NoChange, rr, null));

               else if (rr.Version > pi.Version)
                  diff.Add(new PackageKeyDiff(DiffType.Upgrade, rr, pi));

               else if (rr.Version < pi.Version)
                  diff.Add(new PackageKeyDiff(DiffType.Downgrade, rr, pi));
            }

            var deleted = _index.InstalledPackages
               .Where(ip => resolutionResult.FirstOrDefault(rr => rr.LooseEquals(ip)) == null)
               .Select(ip => new PackageKeyDiff(DiffType.Delete, ip));

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
         //if(!_index.IsInstalled(pck))
         //{
            using (Stream s = _localRepository.Download(pck))
            {
               using (PackageReader reader = _packageReaderFactory.Get(s))
               {
                  if(BeginInstallPackage != null)
                     BeginInstallPackage(this, new PackageKeyDiffEventArgs(pck, true));

                  reader.InstallTo(_rootDirectory, originalDependency, configuration);
               }
            }

            _index.Install(pck);

            if(FinishInstallPackage != null)
               FinishInstallPackage(this, new PackageKeyDiffEventArgs(pck, true));
         //}
      }

      ///<summary>
      ///Forcibly reinstalls all the packages
      ///</summary>
      ///<param name="configuration"></param>
      public void Reinstall(BuildConfiguration configuration)
      {
         IEnumerable<PackageKey> currentDependencies = _versionTable.GetPackages();

         ResetFiles(configuration);

         foreach (PackageKey pck in currentDependencies)
         {
            Install(new PackageKeyDiff(DiffType.Add, pck), _manifest.GetPackageDependency(pck), configuration);
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
