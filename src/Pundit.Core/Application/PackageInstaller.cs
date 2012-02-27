using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;

namespace Pundit.Core.Application
{
   public class PackageInstaller : IDisposable
   {
      private readonly string _rootDirectory;
      private readonly VersionResolutionTable _versionTable;
      private readonly DevPackage _devManifest;
      private readonly ILocalRepository _localRepository;
      private InstalledPackagesIndex _index;
      private bool _indexCommitted;

      private readonly string _libFolderPath;
      private readonly string _includeFolderPath;
      private readonly string _toolsFolderPath;
      private readonly string _otherFolderPath;

      public event EventHandler<PackageKeyDiffEventArgs> BeginInstallPackage;
      public event EventHandler<PackageKeyDiffEventArgs> FinishInstallPackage;

      public PackageInstaller(string rootDirectory, VersionResolutionTable versionTable,
         DevPackage devManifest,
         ILocalRepository localRepository)
      {
         if (rootDirectory == null) throw new ArgumentNullException("rootDirectory");
         if (versionTable == null) throw new ArgumentNullException("versionTable");
         if (devManifest == null) throw new ArgumentNullException("devManifest");
         if (localRepository == null) throw new ArgumentNullException("localRepository");

         _rootDirectory = rootDirectory;
         _versionTable = versionTable;
         _devManifest = devManifest;
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
            var added = from rr in resolutionResult
                        where _index.InstalledPackages.FirstOrDefault(ip => ip.LooseEquals(rr)) == null
                        select new PackageKeyDiff(DiffType.Add, rr);

            diff.AddRange(added);

            var deleted = from ip in _index.InstalledPackages
                          where resolutionResult.FirstOrDefault(rr => rr.LooseEquals(ip)) == null
                          select new PackageKeyDiff(DiffType.Del, ip);

            diff.AddRange(deleted);

            var other = from rr in resolutionResult
                        let ip = _index.InstalledPackages.FirstOrDefault(ip => ip.LooseEquals(rr))
                        let noChange = ip != null && rr.Equals(ip)
                        where ip != null
                        select new PackageKeyDiff(
                           noChange ? DiffType.NoChange : DiffType.Mod,
                           rr,
                           noChange ? null : ip);

            diff.AddRange(other);
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
         if (!_index.IsInstalled(pck))
         {
            using (Stream s = _localRepository.Get(pck))
            {
               using (PackageReader reader = new PackageReader(s))
               {
                  if (BeginInstallPackage != null)
                     BeginInstallPackage(this, new PackageKeyDiffEventArgs(pck, true));

                  reader.InstallTo(_rootDirectory, originalDependency, configuration);
               }
            }

            _index.Install(pck);

            if (FinishInstallPackage != null)
               FinishInstallPackage(this, new PackageKeyDiffEventArgs(pck, true));
         }
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
            Install(new PackageKeyDiff(DiffType.Add, pck), _devManifest.GetPackageDependency(pck), configuration);
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
         bool hasDeletes = diffs.FirstOrDefault(d => d.DiffType == DiffType.Del) != null;

         if(hasDeletes)
         {
            Reinstall(configuration);
         }
         else
         {
            var installs = diffs.Where(d => (d.DiffType == DiffType.Add || d.DiffType == DiffType.Mod));

            foreach(PackageKeyDiff diff in installs)
            {
               Install(diff, _devManifest.GetPackageDependency(diff), configuration);
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
