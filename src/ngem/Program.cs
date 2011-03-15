using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NDesk.Options;
using NGem;
using NGem.Core.Application;

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
            int retcode = 0;
            string command = args[0];

            if(command == "pack")
            {
               return CreatePackage(args.Length > 1 ? args[1] : null);
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

      private static string ResolvePath(string argPath)
      {
         return null;
      }

      private static int CreatePackage(string packageFilePath)
      {
         //if(Path.IsPathRooted(packageFilePath))

         //using(PackageWriter writer = new PackageWriter())

         return 0;
      }
   }
}
