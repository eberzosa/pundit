using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Resolvers;
using EBerzosa.Pundit.Core.Serializers;
using LightInject;
using Mapster;
using Pundit.Core.Application;

namespace EBerzosa.Pundit.Core
{
   public class WiringModule : ICompositionRoot
   {
      public void Compose(IServiceRegistry serviceRegistry)
      {
         serviceRegistry.Register<ISerializer, XmlSerializer>(new PerRequestLifeTime());
         serviceRegistry.Register<CacheRepository>(new PerContainerLifetime());

         serviceRegistry.Register<RepositoryFactory>(new PerContainerLifetime());
         serviceRegistry.Register<PackageReaderFactory>(new PerContainerLifetime());
         
         serviceRegistry.Register<PackageInstallerFactory>(new PerContainerLifetime());

         serviceRegistry.Register<DependencyResolution>(new PerContainerLifetime());

         serviceRegistry.Register<IPackageSerializer>(f => new PackageSerializer(new XmlSerializer()));

         serviceRegistry.Register<InstalledPackagesIndexSerializer>();

         serviceRegistry.Register<InstalledPackagesManager>();

         TypeAdapterConfig.GlobalSettings.Compile();
      }
   }
}