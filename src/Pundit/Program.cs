using System;
using System.Diagnostics;
using EBerzosa.CommandLineProcess;
using EBerzosa.Pundit.CommandLine.Builders;
using LightInject;

namespace EBerzosa.Pundit.CommandLine
{
   internal static class Program
   {
      static int Main(string[] args)
      {
         var debuggerAttached = Debugger.IsAttached;
         
         do
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

               var result = app.Run(args);

               if (!debuggerAttached)
                  return result;

               Console.WriteLine("".PadLeft(System.Console.WindowWidth - 1, '='));
               Console.Write("> ");
               var line = Console.ReadLine();

               args = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }
         } while (args.Length == 0 || !("exit".Equals(args[0], StringComparison.OrdinalIgnoreCase) || "quit".Equals(args[0], StringComparison.OrdinalIgnoreCase)));

         return ExitCode.Success.ToInteger();
      }
   }
}