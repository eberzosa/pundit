using System.Linq.Expressions;
using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Mappings;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Serializers;
using LightInject;
using Mapster;
using Pundit.Core;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core
{
   public class WiringModule : ICompositionRoot
   {
      public void Compose(IServiceRegistry serviceRegistry)
      {
         serviceRegistry.Register<LocalRepository>(new PerContainerLifetime());

         serviceRegistry.Register<RepositoryFactory>(new PerContainerLifetime());
         serviceRegistry.Register<PackageReaderFactory>(new PerContainerLifetime());

         serviceRegistry.Register<PackageSerializerFactory>(new PerContainerLifetime());

         FrameworkMappings.Initialise();

         PackageMappings.XmlMappings();
         NuGetv3PackageMappings.Initialise();
         TypeAdapterConfig.GlobalSettings.Compile();
      }
   }
}
