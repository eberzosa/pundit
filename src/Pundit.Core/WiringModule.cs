using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Mappings;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Repository.Mappings;
using EBerzosa.Pundit.Core.Resolvers;
using EBerzosa.Pundit.Core.Serializers;
using LightInject;
using Mapster;

namespace EBerzosa.Pundit.Core
{
   public class WiringModule : ICompositionRoot
   {
      public void Compose(IServiceRegistry serviceRegistry)
      {
         serviceRegistry.Register<ISerializer, XmlSerializer>(new PerRequestLifeTime());
         serviceRegistry.Register<LocalRepository>(new PerContainerLifetime());

         serviceRegistry.Register<RepositoryFactory>(new PerContainerLifetime());
         serviceRegistry.Register<PackageReaderFactory>(new PerContainerLifetime());

         serviceRegistry.Register<PackageSerializerFactory>(new PerContainerLifetime());

         serviceRegistry.Register<IDependencyResolver, PunditDependencyResolver>("1", new PerRequestLifeTime());
         //serviceRegistry.Register<IDependencyResolver, NuGetDependencyResolver>("2", new PerRequestLifeTime());
         serviceRegistry.Register<DependencyResolution>(new PerContainerLifetime());

         FrameworkMappings.Initialise();

         PackageMappings.XmlMappings();
         NuGetv3PackageMappings.Initialise();
         RepositoryMappings.Initalise();
         TypeAdapterConfig.GlobalSettings.Compile();
      }
   }
}
