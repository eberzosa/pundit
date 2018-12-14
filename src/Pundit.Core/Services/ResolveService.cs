using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Resolvers;
using EBerzosa.Pundit.Core.Serializers;
using EBerzosa.Utils;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;

namespace EBerzosa.Pundit.Core.Services
{
   public class ResolveService
   {
      private readonly IPackageSerializer _packageSerializer;
      private readonly ManifestResolver _manifestResolver;
      private readonly DependencyResolver _dependencyResolver;
      private readonly PackageInstallerFactory _packageInstallerFactory;

      private readonly RepositoryFactory _repositoryFactory;

      //private readonly IRepositoryManager _repositoryManager;
      private readonly IWriter _writer;

      private string _resolvedManifestPath;

      public string ManifestFileOrPath { get; set; }

      public BuildConfiguration Configuration { get; set; } = BuildConfiguration.Release;

      public bool Force { get; set; }

      public bool DryRun { get; set; }

      public bool CacheReposOnly { get; set; }

      public string ReleaseLabel { get; set; }

      public string Repository { get; set; }


      public ResolveService(IPackageSerializer packageSerializer, ManifestResolver manifestResolver, RepositoryFactory repositoryFactory, DependencyResolver dependencyResolver, 
         PackageInstallerFactory packageInstallerFactory, IWriter writer)
      {
         Guard.NotNull(packageSerializer, nameof(packageSerializer));
         Guard.NotNull(manifestResolver, nameof(manifestResolver));
         Guard.NotNull(repositoryFactory, nameof(repositoryFactory));
         Guard.NotNull(dependencyResolver, nameof(dependencyResolver));
         Guard.NotNull(packageInstallerFactory, nameof(packageInstallerFactory));
         Guard.NotNull(writer, nameof(writer));

         _packageSerializer = packageSerializer;
         _manifestResolver = manifestResolver;
         _repositoryFactory = repositoryFactory;
         _dependencyResolver = dependencyResolver;
         _packageInstallerFactory = packageInstallerFactory;
         _writer = writer;
      }

      public bool Resolve()
      {
         if (_resolvedManifestPath == null)
            _resolvedManifestPath = _manifestResolver.GetManifest(ManifestFileOrPath);

         PrintSettings();

         _writer.BeginWrite().Text("Reading manifest...");

         PackageManifestRoot manifest;
         using (Stream stream = File.OpenRead(_resolvedManifestPath))
         {
            manifest = _packageSerializer.DeserializePackageManifestRoot(stream);
            manifest.Validate();
         }
         
         _writer.Success(" ok").EndWrite();

         _writer.BeginWrite().Text("Getting repositories...");

         var scope = CacheReposOnly ? RepositoryScope.Cache : RepositoryScope.Any;

         var repos = Repository == null
            ? _repositoryFactory.TryGetEnabledRepos(scope).ToArray()
            : new[] {_repositoryFactory.GetCustomRepository(Repository)};

         if (repos.Length == 0 || repos[0] == null)
         {
            _writer.Error(" no available repos");
            return false;
         }

         _writer.EndWrite();

         foreach (var repository in repos)
            _writer.Info("  " + repository);

         _writer.EndWrite();

         _writer.BeginWrite().Text("Resolving...");
         var resolutionResult = _dependencyResolver.Resolve(manifest, repos, ReleaseLabel);

         if (resolutionResult.ResolutionTable.HasConflicts)
         {
            _writer.Error(" failed").EndWrite()
               .Empty()
               .BeginWrite().Error("Could not resolve manifest due to conflicts...").EndWrite();

            PrintConflicts(_dependencyResolver, resolutionResult);

            return false;
         }

         _writer.Success(" ok").EndWrite();

         if (DryRun)
         {
            var packageIds = new List<string>();
            var versions = new List<string>();
            var platforms = new List<string>();
            var inRepo = new List<string>();
            foreach (var infos in resolutionResult.ResolutionTable.GetSatisfyingInfos())
            {
               packageIds.Add(infos.PackageId);
               versions.Add(infos.Version.ToString());
               platforms.Add(infos.Framework);
               inRepo.Add(infos.Repo.Name);
            }

            _writer.Title("Resolved packages")
               .BeginColumns(new int?[] {packageIds.Max(p => p.Length + 2), versions.Max(v => v.Length + 2), platforms.Max(p => (p?.Length ?? 0) + 2), null});

            _writer.Header("PackageId").Header("Version").Header("Fmwk").Header("Repository");

            for (int i = 0; i < packageIds.Count; i++)
               _writer.Text(packageIds[i]).Text(versions[i]).Text(platforms[i]).Text(inRepo[i]);

            _writer.EndColumns();

            return true;
         }

         Install(resolutionResult, manifest, scope);

         return true;
      }

      private void Install(IResolutionResult resolutionResult, PackageManifestRoot manifest, RepositoryScope scope)
      {
         if (scope != RepositoryScope.Cache)
         {
            var cacheRepo = new CacheRepository(_repositoryFactory.TryGetEnabledRepos(RepositoryScope.Cache));
            cacheRepo.PackageDownloadToCacheRepositoryStarted += CacheRepositoryPackageDownloadToCacheRepositoryStarted;
            cacheRepo.PackageDownloadToCacheRepositoryFinished += CacheRepositoryOnPackageDownloadToCacheRepositoryFinished;

            cacheRepo.DownloadLocally(resolutionResult.ResolutionTable.GetSatisfyingInfos());

            cacheRepo.PackageDownloadToCacheRepositoryStarted -= CacheRepositoryPackageDownloadToCacheRepositoryStarted;
            cacheRepo.PackageDownloadToCacheRepositoryFinished -= CacheRepositoryOnPackageDownloadToCacheRepositoryFinished;
         }

         using (var installer = _packageInstallerFactory.GetInstaller(_manifestResolver.CurrentDirectory, resolutionResult.ResolutionTable, manifest))
         {
            installer.BeginInstallPackage += BeginInstallPackage;
            installer.FinishInstallPackage += FinishInstallPackage;

            if (Force)
            {
               _writer.Text($"Reinstalling {resolutionResult.ResolutionTable.GetPackages().Count()} packages... ");
               installer.Reinstall(Configuration);
            }
            else
            {
               var diff = installer.GetDiffWithCurrent(resolutionResult.ResolutionTable.GetSatisfyingInfos()).ToArray();

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
      
      private void CacheRepositoryPackageDownloadToCacheRepositoryStarted(object sender, PackageKeyEventArgs e)
      {
         _writer.BeginWrite().Text($"Downloading {e.PackageKey.PackageId}... ");
      }

      private void CacheRepositoryOnPackageDownloadToCacheRepositoryFinished(object sender, PackageKeyEventArgs e)
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
         if (CacheReposOnly)
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

      private void PrintConflicts(DependencyResolver dependencyResolver, IResolutionResult resolutionResult)
      {
         ICollection<DependencyNode> allNodes = new List<DependencyNode>();

         foreach (UnresolvedPackage conflict in resolutionResult.ResolutionTable.GetConflictedPackages())
            _writer.Error(dependencyResolver.DescribeConflict(resolutionResult.DependencyNode, conflict, allNodes));

         _writer.Empty();
         _writer.Error(dependencyResolver.PrintDependencyNodes(allNodes.Distinct(DependencyNodeComparer.PackageId).OrderBy(n => n.PackageId)));
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
         _writer.Success(" done").EndWrite();
      }

   }
}
