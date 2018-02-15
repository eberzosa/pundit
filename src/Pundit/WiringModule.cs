using System;
using EBerzosa.Pundit.CommandLine.Builders;
using EBerzosa.Pundit.CommandLine.Controllers;
using EBerzosa.Pundit.CommandLine.Utils;
using EBerzosa.Pundit.Core;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Services;
using LightInject;


namespace EBerzosa.Pundit.CommandLine
{
   internal class WiringModule : ICompositionRoot
   {
      public void Compose(IServiceRegistry serviceRegistry)
      {
         // Builders
         serviceRegistry.RegisterAssembly(typeof(WiringModule).Assembly, (serviceType, implementingType) => serviceType == typeof(IBuilder));

         // Controllers
         serviceRegistry.RegisterAssembly(typeof(WiringModule).Assembly, (serviceType, implementingType) =>
         {
            if (serviceType == typeof(IController) || serviceType == typeof(ISubBuilder))
               serviceRegistry.Register(implementingType);

            return false;
         });

         // Services
         serviceRegistry.Register<ServiceFactory>(new PerContainerLifetime());

         // Others
         serviceRegistry.Register(factory => new ManifestResolver(Environment.CurrentDirectory));

         serviceRegistry.Register<IWriter, ConsoleWriter>(new PerContainerLifetime());
      }
   }
}
