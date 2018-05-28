using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Package;
using EBerzosa.Utils;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using EBerzosa.Pundit.Core.Versioning;

namespace EBerzosa.Pundit.Core.Repository
{
   internal class NuGetFileSystemRepo : Repository, IRepository
   {
      private readonly SourceRepository _sourceRepository;


      public string ApiKey { get; set; }

      public TimeSpan TimeOut { get; set; } = TimeSpan.FromSeconds(30);


      public NuGetFileSystemRepo(string rootPath, string name)
         : base(rootPath, name, RepositoryType.NuGet)
      {
         var providers = new List<Lazy<INuGetResourceProvider>>();
         providers.AddRange(NuGet.Protocol.Core.Types.Repository.Provider.GetCoreV3());
         
         _sourceRepository = new SourceRepository(new PackageSource(rootPath, Name), providers);
      }

      public void Publish(string filePath)
      {
         NuGet.Packaging.Core.PackageIdentity packageIdentity;

         using (var stream = File.OpenRead(filePath))
            packageIdentity = new NuGet.Packaging.PackageArchiveReader(stream, false).NuspecReader.GetIdentity();

         var packageSearch = _sourceRepository.GetResource<FindPackageByIdResource>();
         var versions = packageSearch.GetAllVersionsAsync(packageIdentity.Id, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None).Result;
         
         var packageUpdate = _sourceRepository.GetResource<PackageUpdateResource>();

         var comparer = new VersionComparerExtended(VersionComparer.Pundit);
         foreach (var version in versions)
         {
            if (comparer.Equals(packageIdentity.Version, version))
            {
               packageUpdate.Delete(packageIdentity.Id, version.ToString(),
                  endpoint => ApiKey != null ? EncryptionUtility.DecryptString(ApiKey) : null,
                  confirm => true, true, NullLogger.Instance).Wait();
            }
         }
         
         packageUpdate.Push(filePath, null, (int)TimeOut.TotalSeconds, true, 
            endpoint => ApiKey != null ? EncryptionUtility.DecryptString(ApiKey) : null, 
            symbolsEndpoint => null, 
            true, NullLogger.Instance).Wait();
      }

      public void Publish(Stream package) => throw new NotSupportedException();

      public Stream Download(PackageKey key)
      {
         var packageIdentity = new NuGet.Packaging.Core.PackageIdentity(key.PackageId, key.Version);

         var downloadResource = _sourceRepository.GetResource<DownloadResource>();
         
         var packageDownloadContext = new PackageDownloadContext(new SourceCacheContext {DirectDownload = true, NoCache = true}, Path.GetTempPath(), true);
         var result = downloadResource.GetDownloadResourceResultAsync(packageIdentity, packageDownloadContext, 
            null, new NullLogger(), CancellationToken.None).Result;

         return result.PackageStream;
      }
      
      public ICollection<NuGet.Versioning.NuGetVersion> GetVersions(UnresolvedPackage package)
      {
         var packagesResource = _sourceRepository.GetResource<FindPackageByIdResource>();
         var packageVersions = packagesResource.GetAllVersionsAsync(package.PackageId, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

         return packageVersions.Result.ToArray();
      }

      public PackageManifest GetManifest(PackageKey key, NuGet.Frameworks.NuGetFramework projectFramework)
      {
         Guard.NotNull(projectFramework, nameof(projectFramework));

         var packageIdentity = new NuGet.Packaging.Core.PackageIdentity(key.PackageId, key.Version);

         var packagesResource = _sourceRepository.GetResource<PackageMetadataResource>();
         var packageInfo = packagesResource.GetMetadataAsync(packageIdentity, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None).Result;
         
         NuGet.Packaging.PackageDependencyGroup dependencies = null;

         if (packageInfo.DependencySets.Count() == 1 && packageInfo.DependencySets.First().TargetFramework.IsUnsupported)
            dependencies = packageInfo.DependencySets.First();

         else if (packageInfo.DependencySets.Any())
         {
            dependencies = NuGetFrameworkUtility.GetNearest(packageInfo.DependencySets, projectFramework);

            if (dependencies == null)
               throw new ApplicationException($"Could not find compatible dependencies for '{packageInfo.Identity}' and framework '{projectFramework}'");
         }

         var manifest = new PackageManifest
         {
            PackageId = packageInfo.Identity.Id,
            Version = packageInfo.Identity.Version,
            Dependencies = new List<PackageDependency>()
         };

         if (dependencies == null)
            return manifest;

         foreach (var dependency in dependencies.Packages)
            manifest.Dependencies.Add(new PackageDependency(dependency.Id, dependency.VersionRange));

         return manifest;
      }

      public bool PackageExist(PackageKey package)
      {
         var findLocalPackagesResource = _sourceRepository.GetResource<PackageMetadataResource>();

         var packageIdentity = new NuGet.Packaging.Core.PackageIdentity(package.PackageId, package.Version);
         return findLocalPackagesResource.GetMetadataAsync(packageIdentity, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None).Result != null;
      }

      public IEnumerable<PackageKey> Search(string substring)
      {
         var searchResource = _sourceRepository.GetResource<PackageSearchResource>();
         var results = searchResource.SearchAsync(substring, new SearchFilter(true), 0, int.MaxValue, new NullLogger(), CancellationToken.None).Result;

         foreach (var result in results)
            yield return new PackageKey(result.Identity.Id, result.Identity.Version, null);
      }
   }
}