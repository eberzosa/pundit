using System;
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

      static int Main(string[] args)
      {
         log4net.Config.XmlConfigurator.Configure();

         PrintBanner();

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
         Log.Info(string.Format(Strings.Banner, Assembly.GetExecutingAssembly().GetName().Version));
      }

      private static void PrintHelp()
      {
         System.Console.Write(Strings.Help, AppDomain.CurrentDomain.FriendlyName);
      }
   }
}
