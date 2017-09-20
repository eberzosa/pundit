using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Repository;
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
      private readonly LocalRepository _localRepository;

      private readonly ManifestResolver _manifestResolver;
      private readonly PackageReaderFactory _packageReaderFactory;
      private readonly PackageSerializerFactory _packageSerializerFactory;

      private readonly RepositoryFactory _repositoryFactory;

      //private readonly IRepositoryManager _repositoryManager;
      private readonly IWriter _writer;

      private string _resolvedManifestPath;

      public string ManifestFileOrPath { get; set; }

      public BuildConfiguration Configuration { get; set; } = BuildConfiguration.Release;

      public bool Force { get; set; }

      public bool DryRun { get; set; }

      public bool LocalReposOnly { get; set; }


      public ResolveService(LocalRepository localRepository, ManifestResolver manifestResolver, RepositoryFactory repositoryFactory, PackageReaderFactory packageReaderFactory,
         PackageSerializerFactory packageSerializerFactory, IWriter writer) //IRepositoryManager repositoryManager,  IWriter writer)
      {
         Guard.NotNull(localRepository, nameof(localRepository));
         Guard.NotNull(manifestResolver, nameof(manifestResolver));
         Guard.NotNull(repositoryFactory, nameof(repositoryFactory));
         Guard.NotNull(packageReaderFactory, nameof(packageReaderFactory));
         Guard.NotNull(packageSerializerFactory, nameof(packageSerializerFactory));
         Guard.NotNull(writer, nameof(writer));

         _localRepository = localRepository;
         _manifestResolver = manifestResolver;
         _packageReaderFactory = packageReaderFactory;
         _packageSerializerFactory = packageSerializerFactory;
         _repositoryFactory = repositoryFactory;
         //_repositoryManager = repositoryManager;
         _writer = writer;
      }

      public bool Resolve()
      {
         if (_resolvedManifestPath == null)
            _resolvedManifestPath = _manifestResolver.GetManifest(ManifestFileOrPath);

         PrintSettings();

         _writer.BeginWrite().Text("Reading manifest... ");
         var packageSpecs = _packageSerializerFactory.GetPundit().DeserializePackageSpec(File.OpenRead(_resolvedManifestPath));
         packageSpecs.Validate();
         _writer.Success("ok").EndWrite();

         var repos = GetRepositories(LocalReposOnly ? 0 : int.MaxValue).ToArray();

         _writer.BeginWrite().Text("Resolving... ");
         var dependencyResolution = new DependencyResolution(packageSpecs, repos);
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

         Install(resolutionResult, repos, packageSpecs);

         return true;
      }

      private void Install(Tuple<VersionResolutionTable, DependencyNode> resolutionResult, IRepository[] repos, PackageSpec packageSpecSpecs)
      {
         _localRepository.PackageDownloadToLocalRepositoryStarted += LocalRepository_PackageDownloadToLocalRepositoryStarted;
         _localRepository.PackageDownloadToLocalRepositoryFinished += LocalRepositoryOnPackageDownloadToLocalRepositoryFinished;

         _localRepository.DownloadLocally(resolutionResult.Item1.GetPackages(), repos.Skip(1));

         _localRepository.PackageDownloadToLocalRepositoryStarted -= LocalRepository_PackageDownloadToLocalRepositoryStarted;
         _localRepository.PackageDownloadToLocalRepositoryFinished -= LocalRepositoryOnPackageDownloadToLocalRepositoryFinished;

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
            .Reserved("Manifest:")
            .Text(_resolvedManifestPath);

         _writer.Reserved("Configuration:");
         if (Configuration == BuildConfiguration.Debug)
            _writer.Warning(Configuration.ToString());
         else
            _writer.Success(Configuration.ToString());

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

         _writer.Empty();
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
         PrintDiff(diffs, _writer.Warning, DiffType.Add, "added");
         PrintDiff(diffs, _writer.Success, DiffType.Mod, "upgraded");
         PrintDiff(diffs, _writer.Reserved, DiffType.NoChange, "no change for");
         PrintDiff(diffs, _writer.Error, DiffType.Del, "deleted");

         return diffs.Count(d => d.DiffType != DiffType.NoChange);
      }

      private void PrintDiff(IEnumerable<PackageKeyDiff> diffs, Func<string, IWriter> writer, DiffType diffType, string diffWord)
      {
         bool isMod = diffType == DiffType.Mod;

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
         _writer.Text($"Installing {e.PackageKeyDiff}...");
      }

      void FinishInstallPackage(object sender, PackageKeyDiffEventArgs e)
      {
         _writer.Text("Done");
      }

   }
}
