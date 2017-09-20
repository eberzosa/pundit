using EBerzosa.CommandLineProcess;
using LightInject;

namespace EBerzosa.Pundit.CommandLine
{
   internal static class Startup
   {
      public static IServiceContainer CreateContainer()
      {
         var container = new ServiceContainer();

         container.RegisterFrom<Core.WiringModule>();
         container.RegisterFrom<WiringModule>();
         return container;
      }

      public static ICommandAppMultiple CreateMultiCommandApp() => CommandApp.MultiCommandApp();
   }
}
