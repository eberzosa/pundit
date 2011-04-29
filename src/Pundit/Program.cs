using System;
using System.Reflection;
using Pundit.Console.Commands;
using Pundit.Core;
using Pundit.Core.Model;

namespace Pundit.Console
{
   static class Program
   {
      private static readonly string ExeName = Assembly.GetExecutingAssembly().GetName().Name;
      private static readonly Version CoreVersion = typeof (IRepository).Assembly.GetName().Version;

      static int Main(string[] args)
      {
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
               GlamTerm.WriteLine(helpString, ExeName);
            }
            else
            {
               GlamTerm.WriteWarnLine("Article {0} not found", args[1]);
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
            GlamTerm.WriteErrorLine(ex.Message);

            if (ex is InvalidPackageException)
               GlamTerm.WriteErrorLine(ex.ToString());
            else
            {

#if DEBUG
               GlamTerm.WriteErrorLine(ex.ToString());
#endif
            }

            return 1;
         }

         return 0;
      }

      private static void PrintBanner()
      {
         GlamTerm.WriteLine(ConsoleColor.Green, Strings.Banner, CoreVersion);
      }

      private static void PrintHelp()
      {
         GlamTerm.WriteLine(Strings.Help, ExeName);
      }
   }
}
