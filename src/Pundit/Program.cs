using EBerzosa.Pundit.CommandLine.Builders;
using LightInject;

namespace EBerzosa.Pundit.CommandLine
{
   internal static class Program
   {
      static int Main(string[] args)
      {
         using (var container = Startup.CreateContainer())
         using (container.BeginScope())
         {
            var app = Startup.CreateMultiCommandApp();
            app.RootCommand.AutoShowHelp = true;

            foreach (var builder in container.GetAllInstances<IBuilder>())
            {
               builder.Build(app.RootCommand);
               builder.ReplaceLegacy(ref args);
            }

            return app.Run(args);
         }
      }
   }
}