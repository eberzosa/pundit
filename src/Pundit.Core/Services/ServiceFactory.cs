using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Serializers;
using EBerzosa.Pundit.Core.Utils;
using EBerzosa.Utils;
using Pundit.Core;

namespace EBerzosa.Pundit.Core.Services
{
   public class ServiceFactory
   {
      private readonly LocalRepository _localRepository;
      private readonly RepositoryFactory _repositoryFactory;
      private readonly ManifestResolver _manifestResolver;

      private readonly PackageSerializerFactory _packageSerializerFactory;
      private readonly PackageReaderFactory _packageReaderFactory;

      //private readonly IRepositoryManager _repositoryManager;
      private readonly IWriter _writer;

      public ServiceFactory(LocalRepository localRepository, RepositoryFactory repositoryFactory, ManifestResolver manifestResolver, PackageSerializerFactory packageSerializerFactory, 
         PackageReaderFactory packageReaderFactory, IWriter writer)
      {
         Guard.NotNull(localRepository, nameof(localRepository));
         Guard.NotNull(repositoryFactory, nameof(repositoryFactory));
         Guard.NotNull(manifestResolver, nameof(manifestResolver));
         Guard.NotNull(packageReaderFactory, nameof(packageReaderFactory));
         Guard.NotNull(writer, nameof(writer));

         _localRepository = localRepository;
         _repositoryFactory = repositoryFactory;
         _manifestResolver = manifestResolver;
         _packageSerializerFactory = packageSerializerFactory;
         _packageReaderFactory = packageReaderFactory;
         //_repositoryManager = repositoryManager;
         _writer = writer;
      }

      public ResolveService GetResolveService() 
         => new ResolveService(_localRepository, _manifestResolver, _repositoryFactory, _packageReaderFactory, _packageSerializerFactory, _writer);

      public SpecService GetSpecService()
         => new SpecService(_manifestResolver.CurrentDirectory, _packageSerializerFactory, _writer);

      public PackService GetPackService() 
         => new PackService(_packageSerializerFactory, _manifestResolver, _writer);

      public SearchService GetSearchService() 
         => new SearchService(_localRepository, _repositoryFactory, _writer);

      public PublishService GetPublishService() 
         => new PublishService(_localRepository, _repositoryFactory, _writer);

      public UpdateService GetUpdateService() 
         => new UpdateService(_localRepository, _repositoryFactory, _packageReaderFactory, _writer);

      public ConvertService GetConvertService() 
         => new ConvertService(new PackService(_packageSerializerFactory, _manifestResolver, new NullWriter()), _packageSerializerFactory, _writer);
   }
}
