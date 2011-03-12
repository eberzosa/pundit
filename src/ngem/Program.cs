using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NDesk.Options;
using NGem;

namespace ngem
{
   class Program
   {
      static int Main(string[] args)
      {
         PrintBanner();

         if(args.Length == 0)
         {
            PrintHelp();
         }
         else
         {
            string command = args[0];

            if(command == "pack")
            {
               CreatePackage(args.Length > 1 ? args[1] : null);
            }
            else if(command == "template")
            {
               
            }
            else
            {
               using (new ColorChange(ConsoleColor.Red))
               {
                  Console.WriteLine("wrong command");
               }

               PrintHelp();

               return 1;
            }
         }

         return 0;
      }

      private static void PrintBanner()
      {
         Console.WriteLine(Strings.Banner, Assembly.GetExecutingAssembly().GetName().Version);
      }

      private static void PrintHelp()
      {
         Console.Write(Strings.Help, AppDomain.CurrentDomain.FriendlyName);
      }

      private static void CreatePackage(string packageFilePath)
      {
         
      }
   }
}
