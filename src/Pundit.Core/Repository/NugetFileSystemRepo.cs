using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Versioning;
using EBerzosa.Utils;
using Mapster;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using Pundit.Core.Model;
using PackageDependency = EBerzosa.Pundit.Core.Model.Package.PackageDependency;

namespace EBerzosa.Pundit.Core.Repository
{
   internal class NuGetFileSystemRepo : IRepository
   {
      private readonly SourceRepository _sourceRepository;

      public string Name { get; }

      public string RootPath { get; }

      public bool CanPublish { get; set; }

      public RepositoryType Type { get; }


      public NuGetFileSystemRepo(string rootPath, string name, RepositoryType type)
      {
         Guard.NotNull(rootPath, nameof(rootPath));

         if (!rootPath.StartsWith("http", StringComparison.OrdinalIgnoreCase) && !Directory.Exists(rootPath))
            throw new ArgumentException($"Root directory '{rootPath}' does not exist");

         RootPath = rootPath;
         Name = name;
         Type = type;
         
         var providers = new List<Lazy<INuGetResourceProvider>>();
         providers.AddRange(NuGet.Protocol.Core.Types.Repository.Provider.GetCoreV3());

         _sourceRepository = new SourceRepository(new PackageSource(rootPath), providers);
      }


      public void Publish(Stream packageStream)
      {
         if (!CanPublish)
            throw new Exception("Publish is not allowed");
         
         var packageUpdate = _sourceRepository.GetResource<PackageUpdateResource>();
         
         var fileStream = packageStream as FileStream;

         packageUpdate.Push(fileStream.Name, null, 200, false, s => null, s => null, true, NullLogger.Instance).Wait();

         packageStream.Dispose();
      }

      public Stream Download(PackageKey key)
      {
         var packageIdentity = new PackageIdentity(key.PackageId, PunditVersion.Parse(key.VersionString).Adapt<NuGet.Versioning.NuGetVersion>());

         var downloadResource = _sourceRepository.GetResource<DownloadResource>();
         
         var packageDownloadContext = new PackageDownloadContext(new SourceCacheContext {DirectDownload = true, NoCache = true}, Path.GetTempPath(), true);
         var result = downloadResource.GetDownloadResourceResultAsync(packageIdentity, packageDownloadContext, 
            null, new NullLogger(), CancellationToken.None).Result;

         return result.PackageStream;
      }

      public class xxxxx : LoggerBase
      {
         public override void Log(ILogMessage message)
         {
            var x = message;
         }

         public override Task LogAsync(ILogMessage message)
         {
            var x = message;
            return Task.CompletedTask;
         }
      }

      public ICollection<PunditVersion> GetVersions(UnresolvedPackage package)
      {
         var packagesResource = _sourceRepository.GetResource<FindPackageByIdResource>();
         var packageInfos = packagesResource.GetAllVersionsAsync(package.PackageId, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);
         
         // TODO: Finish this
         throw new NotImplementedException();
         //return packageInfos.Result.Where(p => package.AllowedVersions.IsFloating
         //      ? package.AllowedVersions.Float.Satisfies(p.Adapt<PunditVersion>())
         //      : package.AllowedVersions.Satisfies(p.Adapt<PunditVersion>()))
         //   .Adapt<IEnumerable<PunditVersion>>().ToArray();
      }

      public PackageManifest GetManifest(PackageKey key)
      {
         var packageIdentity = new PackageIdentity(key.PackageId, PunditVersion.Parse(key.VersionString).Adapt<NuGet.Versioning.NuGetVersion>());

         var packagesResource = _sourceRepository.GetResource<PackageMetadataResource>();
         var packageInfo = packagesResource.GetMetadataAsync(packageIdentity, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None).Result;

         var projectFramework = NuGetFramework.ParseFolder(key.Platform);

         PackageDependencyGroup dependencies = null;
         if (packageInfo.DependencySets.Any())
         {
            dependencies = NuGetFrameworkUtility.GetNearest(packageInfo.DependencySets, projectFramework);

            if (dependencies == null)
               throw new ApplicationException($"Could not find compatible dependencies for '{packageInfo.Identity}' and framework '{projectFramework}'");
         }

         var manifest = new PackageManifest
         {
            PackageId = packageInfo.Identity.Id,
            Version = packageInfo.Identity.Version.Adapt<PunditVersion>(),
            Dependencies = new List<Model.Package.PackageDependency>(),
            Platform = dependencies?.TargetFramework.GetShortFolderName()
         };

         if (dependencies == null)
            return manifest;

         //TODO: Finish this
         throw new NotImplementedException();
         //foreach (var dependency in dependencies.Packages)
         //   manifest.Dependencies.Add(new PackageDependency(dependency.Id, dependency.VersionRange.Adapt<VersionRange>()) {Platform = manifest.Platform});

         return manifest;
      }

      public bool PackageExist(PackageKey package)
      {
         var findLocalPackagesResource = _sourceRepository.GetResource<PackageMetadataResource>();

         var packageIdentity = new PackageIdentity(package.PackageId, package.Version.Adapt<NuGet.Versioning.NuGetVersion>());
         return findLocalPackagesResource.GetMetadataAsync(packageIdentity, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None).Result != null;
      }

      public IEnumerable<PackageKey> Search(string substring)
      {
         var searchResource = _sourceRepository.GetResource<PackageSearchResource>();
         var results = searchResource.SearchAsync(substring, new SearchFilter(true), 0, int.MaxValue, new NullLogger(), CancellationToken.None)
            .Result;

         foreach (var result in results)
            yield return new PackageKey(result.Identity.Id, result.Identity.Version.Adapt<PunditVersion>(), null);
      }

      private FileSystemRepository GetFsRepoOrDie(IRepository repo)
      {
         if (repo is FileSystemRepository fsRepo)
            return fsRepo;

         throw new ApplicationException("Only FileSystem repos are supported");
      }

      public override string ToString() => $"{Name} [{RootPath}]";
   }
}