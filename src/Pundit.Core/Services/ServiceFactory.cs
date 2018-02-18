using EBerzosa.Pundit.Core.Application;
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

      private readonly PackageSerializerFactory _packageSerializerFactory;
      private readonly PackageReaderFactory _packageReaderFactory;
      private readonly DependencyResolution _dependencyResolution;

      //private readonly IRepositoryManager _repositoryManager;
      private readonly IWriter _writer;

      public ServiceFactory(RepositoryFactory repositoryFactory, ManifestResolver manifestResolver, 
         PackageSerializerFactory packageSerializerFactory, PackageReaderFactory packageReaderFactory, DependencyResolution dependencyResolution,
         IWriter writer)
      {
         Guard.NotNull(repositoryFactory, nameof(repositoryFactory));
         Guard.NotNull(manifestResolver, nameof(manifestResolver));
         Guard.NotNull(packageReaderFactory, nameof(packageReaderFactory));
         Guard.NotNull(dependencyResolution, nameof(dependencyResolution));
         Guard.NotNull(writer, nameof(writer));
         
         _repositoryFactory = repositoryFactory;
         _manifestResolver = manifestResolver;
         _packageSerializerFactory = packageSerializerFactory;
         _packageReaderFactory = packageReaderFactory;
         _dependencyResolution = dependencyResolution;
         _writer = writer;
      }

      public ResolveService GetResolveService() 
         => new ResolveService(_manifestResolver, _repositoryFactory, _packageReaderFactory, _packageSerializerFactory, _dependencyResolution, _writer);

      public SpecService GetSpecService()
         => new SpecService(_manifestResolver.CurrentDirectory, _packageSerializerFactory, _writer);

      public PackService GetPackService() 
         => new PackService(_packageSerializerFactory, _manifestResolver, _writer);

      public SearchService GetSearchService() 
         => new SearchService(_repositoryFactory, _writer);

      public PublishService GetPublishService() 
         => new PublishService(_repositoryFactory, _writer);

      public UpdateService GetUpdateService() 
         => new UpdateService(_repositoryFactory, _packageReaderFactory, _dependencyResolution, _writer);

      public ConvertService GetConvertService() 
         => new ConvertService(new PackService(_packageSerializerFactory, _manifestResolver, new NullWriter()), _packageSerializerFactory, _writer);
   }
}
