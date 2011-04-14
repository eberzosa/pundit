using System;
using System.IO;
using System.Reflection;
using log4net;
using Pundit;
using Pundit.Console.Commands;
using Pundit.Core.Model;

namespace Pundit.Console
{
   static class Program
   {
      private static readonly ILog Log = LogManager.GetLogger(typeof (Program));
      private static readonly string ExeName = Assembly.GetExecutingAssembly().GetName().Name;
      private static readonly Version CoreVersion = typeof (IRepository).Assembly.GetName().Version;

      static int Main(string[] args)
      {
         log4net.Config.XmlConfigurator.Configure();

         PrintBanner();

         if(args != null && args.Length == 1 &&
            (args[0] == "-h" || args[0] == "--help" || args[0] == "/?"))
         {
            PrintHelp();

            return 0;
         }
         
         if(args != null && args.Length == 2 && args[0] == "help")
         {
            string helpString = Strings.ResourceManager.GetString("Help_" + args[1]);

            if(helpString != null)
            {
               Log.InfoFormat(helpString, ExeName);
            }
            else
            {
               Log.ErrorFormat("Article {0} not found", args[1]);
            }

            return 0;
         }

         try
         {
            ICommand cmd = CommandFactory.CreateCommand(args);

            cmd.Execute();
         }
         catch(Exception ex)
         {
            Log.Fatal(ex.Message);

            if(ex is InvalidPackageException)
               Log.Fatal(ex);

            Log.Fatal(ex.StackTrace);

            return 1;
         }

         return 0;
      }

      private static void PrintBanner()
      {
         Log.InfoFormat(Strings.Banner, CoreVersion);
      }

      private static void PrintHelp()
      {
         Log.InfoFormat(Strings.Help, ExeName);
      }
   }
}
