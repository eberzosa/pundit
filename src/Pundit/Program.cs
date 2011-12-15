using System;
using System.Threading;
using Pundit.Core;
using Pundit.Core.Application.Console;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Console
{
   static class Program
   {
      private static readonly Version CoreVersion = PackageUtils.GetProductVersion(typeof (LocalConfiguration).Assembly);
      private static readonly IConsoleOutput Con = new GlamTerm();

      static int Main(string[] args)
      {
         PrintBanner();

         Con.StartProgress(400);
         for (int i = 0; i <= 400; i++)
         {
            Con.UpdateProgress(i);
            Thread.Sleep(3);
         }

            try
            {
               IConsoleCommand cmd = CommandFactory.CreateCommand(Con, Environment.CurrentDirectory, args);

               cmd.Execute();
            }
            catch (Exception ex)
            {
               Con.WriteLine(ConsoleColor.Red, ex.Message);

               if (ex is InvalidPackageException)
                  Con.WriteLine(ConsoleColor.Red, ex.ToString());
               else
               {

#if DEBUG
                  Con.WriteLine(ConsoleColor.Red, ex.ToString());
#endif
               }

               return 1;
            }

         return 0;
      }

      private static void PrintBanner()
      {
         Con.WriteLine(ConsoleColor.Green, Strings.Banner, CoreVersion);
      }
   }
}
