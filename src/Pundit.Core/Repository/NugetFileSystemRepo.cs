﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Versioning;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace EBerzosa.Pundit.Core.Repository
{
   internal class NuGetFileSystemRepo : Repository, IRepository
   {
      private readonly SourceRepository _sourceRepository;

      public string ApiKey { get; set; }


      public NuGetFileSystemRepo(string rootPath, string name)
         : base(rootPath, name, RepositoryType.NuGet)
      {
         var providers = new List<Lazy<INuGetResourceProvider>>();
         providers.AddRange(NuGet.Protocol.Core.Types.Repository.Provider.GetCoreV3());

         _sourceRepository = new SourceRepository(new PackageSource(rootPath, Name), providers);
      }

      public void Publish(string filePath)
      {
         var packageUpdate = _sourceRepository.GetResource<PackageUpdateResource>();

         packageUpdate.Push(filePath, null, 10, true, 
            endpoint => ApiKey != null ? EncryptionUtility.DecryptString(ApiKey) : null, 
            symbolsEndpoint => null, 
            true, NullLogger.Instance).Wait();
      }

      public void Publish(Stream package) => throw new NotSupportedException();

      public Stream Download(PackageKey key)
      {
         var packageIdentity = new NuGet.Packaging.Core.PackageIdentity(key.PackageId, NuGet.Versioning.NuGetVersion.Parse(key.VersionString));

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

         return packageVersions.Result.Where(v => package.AllowedVersions.Satisfies(v)).ToArray();
      }

      public PackageManifest GetManifest(PackageKey key)
      {
         var packageIdentity = new NuGet.Packaging.Core.PackageIdentity(key.PackageId, NuGet.Versioning.NuGetVersion.Parse(key.VersionString));

         var packagesResource = _sourceRepository.GetResource<PackageMetadataResource>();
         var packageInfo = packagesResource.GetMetadataAsync(packageIdentity, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None).Result;

         var projectFramework = key.Framework;

         NuGet.Packaging.PackageDependencyGroup dependencies = null;
         if (packageInfo.DependencySets.Any())
         {
            dependencies = NuGetFrameworkUtility.GetNearest(packageInfo.DependencySets, projectFramework);

            if (dependencies == null)
               throw new ApplicationException($"Could not find compatible dependencies for '{packageInfo.Identity}' and framework '{projectFramework}'");
         }

         var manifest = new PackageManifest
         {
            PackageId = packageInfo.Identity.Id,
            Version = packageInfo.Identity.Version,
            Dependencies = new List<PackageDependency>(),
            Framework = dependencies?.TargetFramework
         };

         if (dependencies == null)
            return manifest;

         foreach (var dependency in dependencies.Packages)
            manifest.Dependencies.Add(new PackageDependency(dependency.Id, new VersionRangeExtended(dependency.VersionRange)) {Framework = manifest.Framework});

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