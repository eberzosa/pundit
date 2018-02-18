using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Resolvers;
using EBerzosa.Pundit.Core.Serializers;
using EBerzosa.Utils;
using Pundit.Core;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;

namespace EBerzosa.Pundit.Core.Services
{
   public class ResolveService
   {
      private readonly ManifestResolver _manifestResolver;
      private readonly PackageReaderFactory _packageReaderFactory;
      private readonly PackageSerializerFactory _packageSerializerFactory;
      private readonly DependencyResolution _dependencyResolution;

      private readonly RepositoryFactory _repositoryFactory;

      //private readonly IRepositoryManager _repositoryManager;
      private readonly IWriter _writer;

      private string _resolvedManifestPath;

      public string ManifestFileOrPath { get; set; }

      public BuildConfiguration Configuration { get; set; } = BuildConfiguration.Release;

      public bool Force { get; set; }

      public bool DryRun { get; set; }

      public bool LocalReposOnly { get; set; }

      public bool IncludeDeveloperPackages { get; set; }


      public ResolveService(ManifestResolver manifestResolver, RepositoryFactory repositoryFactory, PackageReaderFactory packageReaderFactory,
         PackageSerializerFactory packageSerializerFactory, DependencyResolution dependencyResolution, IWriter writer)
      {
         Guard.NotNull(manifestResolver, nameof(manifestResolver));
         Guard.NotNull(repositoryFactory, nameof(repositoryFactory));
         Guard.NotNull(packageReaderFactory, nameof(packageReaderFactory));
         Guard.NotNull(packageSerializerFactory, nameof(packageSerializerFactory));
         Guard.NotNull(dependencyResolution, nameof(dependencyResolution));
         Guard.NotNull(writer, nameof(writer));
         
         _manifestResolver = manifestResolver;
         _packageReaderFactory = packageReaderFactory;
         _packageSerializerFactory = packageSerializerFactory;
         _repositoryFactory = repositoryFactory;
         _dependencyResolution = dependencyResolution;
         _writer = writer;
      }

      public bool Resolve()
      {
         if (_resolvedManifestPath == null)
            _resolvedManifestPath = _manifestResolver.GetManifest(ManifestFileOrPath);

         PrintSettings();

         _writer.BeginWrite().Text("Reading manifest...");
         var packageSpecs = _packageSerializerFactory.GetPundit().DeserializePackageSpec(File.OpenRead(_resolvedManifestPath));
         packageSpecs.Validate();
         _writer.Success(" ok").EndWrite();

         _writer.BeginWrite().Text("Getting repositories...");
         var repos = _repositoryFactory.GetRepos(true, !LocalReposOnly).ToArray();
         _writer.Success(" ok").EndWrite();

         _writer.BeginWrite().Text("Resolving...");
         var resolutionResult = _dependencyResolution.Resolve(packageSpecs, repos, IncludeDeveloperPackages);

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
         {
            var packageIds = new List<string>();
            var versions = new List<string>();
            var platforms = new List<string>();
            foreach (var package in resolutionResult.Item1.GetPackages())
            {
               packageIds.Add(package.PackageId);
               versions.Add(package.VersionString);
               platforms.Add(package.Platform);
            }

            _writer.Title("Resolved packages")
               .BeginColumns(new int?[] { packageIds.Max(p => p.Length + 2), versions.Max(v => v.Length + 2), null });

            for (int i = 0; i < packageIds.Count; i++)
               _writer.Text(packageIds[i]).Text(versions[i]).Text(platforms[i]);

            _writer.EndColumns();

            return true;
         }

         Install(resolutionResult, repos, packageSpecs);

         return true;
      }

      private void Install(Tuple<VersionResolutionTable, DependencyNode> resolutionResult, IRepository[] repos, PackageSpec packageSpecSpecs)
      {
         var localRepo = new LocalRepository(_repositoryFactory.GetLocalRepo());
         localRepo.PackageDownloadToLocalRepositoryStarted += LocalRepository_PackageDownloadToLocalRepositoryStarted;
         localRepo.PackageDownloadToLocalRepositoryFinished += LocalRepositoryOnPackageDownloadToLocalRepositoryFinished;

         localRepo.DownloadLocally(resolutionResult.Item1.GetSatisfyingInfos());

         localRepo.PackageDownloadToLocalRepositoryStarted -= LocalRepository_PackageDownloadToLocalRepositoryStarted;
         localRepo.PackageDownloadToLocalRepositoryFinished -= LocalRepositoryOnPackageDownloadToLocalRepositoryFinished;

         using (var installer = new PackageInstaller(_packageReaderFactory, _manifestResolver.CurrentDirectory, resolutionResult.Item1, packageSpecSpecs, repos.First()))
         {
            installer.BeginInstallPackage += BeginInstallPackage;
            installer.FinishInstallPackage += FinishInstallPackage;

            if (Force)
            {
               _writer.Text($"Reinstalling {resolutionResult.Item1.GetPackages().Count()} packages... ");
               installer.Reinstall(Configuration);
            }
            else
            {
               var diff = installer.GetDiffWithCurrent(resolutionResult.Item1.GetPackages()).ToArray();

               int changed = PrintSuccess(diff);
               if (changed > 0)
                  installer.Upgrade(Configuration, diff);
               else
                  _writer.Success("No changes detected");
            }

            installer.BeginInstallPackage -= BeginInstallPackage;
            installer.FinishInstallPackage -= FinishInstallPackage;
         }
      }
      
      private void LocalRepository_PackageDownloadToLocalRepositoryStarted(object sender, PackageKeyEventArgs e)
      {
         _writer.BeginWrite().Text($"Downloading {e.PackageKey.PackageId}... ");
      }

      private void LocalRepositoryOnPackageDownloadToLocalRepositoryFinished(object sender, PackageKeyEventArgs e)
      {
         if (e.Succeeded)
            _writer.Success("ok");
         else
            _writer.Error("failed");

         _writer.EndWrite();
      }

      private void PrintSettings()
      {
         var names = new[] { " Manifest:", " Configuration:", " Local Repos Only:", " Force:", " Dry Run:" };

         _writer.BeginColumns(new int?[] { names.Max(n => n.Length) + 1, null });

         _writer.Reserved(names[0]).Text(_resolvedManifestPath);
         
         _writer.Reserved(names[1]);
         if (Configuration == BuildConfiguration.Debug)
            _writer.Warning(Configuration.ToString());
         else
            _writer.Success(Configuration.ToString());

         _writer.Reserved(names[2]);
         if (LocalReposOnly)
            _writer.Warning("yes");
         else
            _writer.Text("no");

         _writer.Reserved(names[3]);
         if (Force)
            _writer.Error("yes");
         else
            _writer.Text("no");

         _writer.BeginWrite().Reserved(names[4]);
         if (DryRun)
            _writer.Success("yes");
         else
            _writer.Text("no");

         _writer.EndColumns().Empty();
         _writer.EndColumns();

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
         _writer.Text(" done").EndWrite();
      }

   }
}
