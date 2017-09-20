using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Mappings;
using EBerzosa.Pundit.Core.Model.Xml;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Serializers;
using ExpressMapper;
using LightInject;
using Pundit.Core;

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

         PackageMappings.XmlMappings();
         NuGetv3PackageMappings.Initialise();
         Mapper.Compile();
      }
   }
}
