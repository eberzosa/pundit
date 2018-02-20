using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Utils;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Package
{
   public class PackageInstallerFactory
   {
      private readonly PackageReaderFactory _packageReaderFactory;
      private readonly RepositoryFactory _repositoryFactory;

      public PackageInstallerFactory(PackageReaderFactory packageReaderFactory, RepositoryFactory repositoryFactory)
      {
         Guard.NotNull(packageReaderFactory, nameof(packageReaderFactory));
         Guard.NotNull(repositoryFactory, nameof(repositoryFactory));

         _packageReaderFactory = packageReaderFactory;
         _repositoryFactory = repositoryFactory;
      }

      public IPackageInstaller GetInstaller(string rootDirectory, VersionResolutionTable versionTable, PackageSpec manifest)
      {
         return new PackageInstaller(_packageReaderFactory, rootDirectory, versionTable, manifest, _repositoryFactory.GetCacheRepos());
      }
   }
}