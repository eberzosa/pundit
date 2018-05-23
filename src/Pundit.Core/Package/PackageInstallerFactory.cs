using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Utils;
using Pundit.Core.Application;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Package
{
   public class PackageInstallerFactory
   {
      private readonly PackageReaderFactory _packageReaderFactory;
      private readonly RepositoryFactory _repositoryFactory;
      private readonly InstalledPackagesManager _installedPackagesManager;

      public PackageInstallerFactory(PackageReaderFactory packageReaderFactory, RepositoryFactory repositoryFactory, InstalledPackagesManager installedPackagesManager)
      {
         Guard.NotNull(packageReaderFactory, nameof(packageReaderFactory));
         Guard.NotNull(repositoryFactory, nameof(repositoryFactory));

         _packageReaderFactory = packageReaderFactory;
         _repositoryFactory = repositoryFactory;
         _installedPackagesManager = installedPackagesManager;
      }

      public IPackageInstaller GetInstaller(string rootDirectory, VersionResolutionTable versionTable, PackageManifest manifest)
      {
         return new PackageInstaller(_packageReaderFactory, rootDirectory, versionTable, manifest, 
            _repositoryFactory.TryGetCacheRepos(), _installedPackagesManager);
      }
   }
}