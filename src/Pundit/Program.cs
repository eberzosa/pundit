using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NDesk.Options;
using NGem;
using NGem.Commands;
using NGem.Core.Application;
using NGem.Core.Model;

namespace ngem
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
               Console.WriteLine(ex.Message);

               if(ex is InvalidPackageException)
                  Console.Write(ex);
            }

            return 1;
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
   }
}
