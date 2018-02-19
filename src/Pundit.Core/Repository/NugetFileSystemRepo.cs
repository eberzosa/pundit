using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using EBerzosa.Utils;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Pundit.Core.Model;
using PackageDependency = EBerzosa.Pundit.Core.Model.Package.PackageDependency;

namespace EBerzosa.Pundit.Core.Repository
{
    class NuGetFileSystemRepo : IRepository
    {
        private readonly SourceRepository _sourceRepository;

        public string Name { get; }

        public string RootPath { get; }

        public bool CanPublish { get; set; }


        public NuGetFileSystemRepo(string rootPath, string name)
        {
            Guard.NotNull(rootPath, nameof(rootPath));

            if (!Directory.Exists(rootPath))
                throw new ArgumentException($"Root directory '{rootPath}' does not exist");
            
            RootPath = rootPath;
            Name = name;

            var providers = new List<Lazy<INuGetResourceProvider>>();
            providers.AddRange(NuGet.Protocol.Core.Types.Repository.Provider.GetCoreV3());

            _sourceRepository = new SourceRepository(new PackageSource(rootPath), providers);
        }


        public void Publish(Stream packageStream)
        {
            throw new NotImplementedException();
        }

        public Stream Download(PackageKey key)
        {
            throw new NotImplementedException();
        }

        public NuGetVersion[] GetVersions(UnresolvedPackage package)
        {
            var findLocalPackagesResource = _sourceRepository.GetResource<FindLocalPackagesResource>();

            var packageInfos = findLocalPackagesResource.FindPackagesById(package.PackageId, new NullLogger(), CancellationToken.None);

            return packageInfos.Where(p => package.VersionPattern.Satisfies(p.Identity.Version))
                .Select(p => p.Identity.Version).ToArray();
        }

        public PackageManifest GetManifest(PackageKey key)
        {
            var packageIdentity = new PackageIdentity(key.PackageId, NuGetVersion.Parse(key.VersionString));

            var packagesResource = _sourceRepository.GetResource<PackageMetadataResource>();
            var packageInfo = packagesResource.GetMetadataAsync(packageIdentity, new NullLogger(), CancellationToken.None).Result;

            var projectFramework = NuGetFramework.ParseFolder(key.Platform);

            PackageDependencyGroup dependencies = null;
            if (packageInfo.DependencySets.Any())
            {
                dependencies = NuGetFrameworkUtility.GetNearest(packageInfo.DependencySets, projectFramework);

                if (dependencies == null)
                    throw new ApplicationException(
                        $"Could not find compatible dependencies for '{packageInfo.Identity}' and framework '{projectFramework}'");
            }

            var manifest = new PackageManifest
            {
                PackageId = packageInfo.Identity.Id,
                Version = packageInfo.Identity.Version,
                Dependencies = new List<Model.Package.PackageDependency>()
            };

            if (dependencies == null)
                return manifest;

            foreach (var dependency in dependencies.Packages)
                manifest.Dependencies.Add(
                    new PackageDependency(dependency.Id, dependency.VersionRange) { Platform = dependencies.TargetFramework.GetShortFolderName() });

            return manifest;
        }

        public bool[] PackagesExist(PackageKey[] packages)
        {
            throw new NotImplementedException();
        }

        public PackageKey[] Search(string substring)
        {
            throw new NotImplementedException();
        }

        private FileSystemRepository GetFsRepoOrDie(IRepository repo)
        {
            if (repo is FileSystemRepository fsRepo)
                return fsRepo;

            throw new ApplicationException("Only FileSystem repos are supported");
        }
    }
}
