using System;
using System.Reflection;
using Pundit;
using Pundit.Console.Commands;
using Pundit.Core.Model;

namespace Pundit.Console
{
   static class Program
   {
      private const ConsoleColor ErrorColor = ConsoleColor.Red;

      static int Main(string[] args)
      {
         PrintBanner();

         try
         {
            ICommand cmd = CommandFactory.CreateCommand(args);

            cmd.Execute();
         }
         catch(Exception ex)
         {
            using(new ColorChange(ErrorColor))
            {
               System.Console.WriteLine(ex.Message);

               if(ex is InvalidPackageException)
                  System.Console.Write(ex);
            }

            return 1;
         }

         return 0;
      }

      private static void PrintBanner()
      {
         System.Console.WriteLine(Strings.Banner, Assembly.GetExecutingAssembly().GetName().Version);
      }

      private static void PrintHelp()
      {
         System.Console.Write(Strings.Help, AppDomain.CurrentDomain.FriendlyName);
      }
   }
}
