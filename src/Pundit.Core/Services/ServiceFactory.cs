using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Resolvers;
using EBerzosa.Pundit.Core.Serializers;
using EBerzosa.Pundit.Core.Utils;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.Core.Services
{
   public class ServiceFactory
   {
      private readonly RepositoryFactory _repositoryFactory;
      private readonly ManifestResolver _manifestResolver;
      
      private readonly PackageReaderFactory _packageReaderFactory;
      private readonly DependencyResolution _dependencyResolution;
      private readonly IPackageSerializer _packageSerializer;
      private readonly PackageInstallerFactory _packageInstallerFactory;

      //private readonly IRepositoryManager _repositoryManager;
      private readonly IWriter _writer;

      public ServiceFactory(RepositoryFactory repositoryFactory, ManifestResolver manifestResolver, 
         PackageReaderFactory packageReaderFactory, DependencyResolution dependencyResolution,
         IPackageSerializer packageSerializer,
         PackageInstallerFactory packageInstallerFactory, IWriter writer)
      {
         Guard.NotNull(repositoryFactory, nameof(repositoryFactory));
         Guard.NotNull(manifestResolver, nameof(manifestResolver));
         Guard.NotNull(packageReaderFactory, nameof(packageReaderFactory));
         Guard.NotNull(dependencyResolution, nameof(dependencyResolution));
         Guard.NotNull(packageInstallerFactory, nameof(packageInstallerFactory));
         Guard.NotNull(writer, nameof(writer));

         Guard.NotNull(packageSerializer, nameof(packageSerializer));

         _repositoryFactory = repositoryFactory;
         _manifestResolver = manifestResolver;
         _packageReaderFactory = packageReaderFactory;
         _dependencyResolution = dependencyResolution;
         _packageSerializer = packageSerializer;
         _packageInstallerFactory = packageInstallerFactory;
         _writer = writer;
      }

      public ResolveService GetResolveService() 
         => new ResolveService(_packageSerializer, _manifestResolver, _repositoryFactory, _dependencyResolution, _packageInstallerFactory, _writer);

      public SpecService GetSpecService()
         => new SpecService(_packageSerializer, _manifestResolver.CurrentDirectory, _writer);

      public PackService GetPackService() 
         => new PackService(_packageSerializer, _manifestResolver, _writer);

      public SearchService GetSearchService() 
         => new SearchService(_repositoryFactory, _writer);

      public PublishService GetPublishService() 
         => new PublishService(_repositoryFactory, _writer);

      public UpdateService GetUpdateService() 
         => new UpdateService(_repositoryFactory, _packageInstallerFactory, _dependencyResolution, _writer);

      public ConvertService GetConvertService() 
         => new ConvertService(new PackService(_packageSerializer, _manifestResolver, new NullWriter()), _writer);

      public UtilService GetUtilService()
         => new UtilService();
   }
}
