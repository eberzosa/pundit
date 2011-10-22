using System;
using System.Diagnostics;
using System.IO;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Application.Console.Commands
{
   class UpdateConsoleCommand : BaseConsoleCommand
   {
      public UpdateConsoleCommand(IConsoleOutput console, string currentDirectory, string[] args)
         : base(console, currentDirectory, args)
      {
      }

      public override void Execute()
      {
         Version current = PackageUtils.GetProductVersion(typeof (UpdateConsoleCommand).Assembly);
         AutoUpdate.Product latest = null;

         console.WriteLine("Checking for updates...");

         try
         {
            latest = AutoUpdate.CheckUpdates();
         }
         catch(Exception ex)
         {
            console.Write(false);
         }

         if(latest != null)
         {
            console.Write("current version: ");
            console.Write(ConsoleColor.Green, current.ToString());
            console.Write(", latest version: ");
            console.WriteLine(ConsoleColor.Yellow, latest.Version.ToString());

            if(latest.Version > current)
            {
               console.WriteLine(ConsoleColor.Red, "You have an update!");

               console.Write("downloading update to " + PathUtils.ExeFolder + "... ");

               string exePath = null;

               try
               {
                  exePath = AutoUpdate.Download(latest, PathUtils.ExeFolder);

                  console.Write(true);
               }
               catch(Exception ex)
               {
                  console.Write(false);

                  console.WriteLine("update failed: " + ex.Message + ex.StackTrace);
               }

               if(exePath != null)
               {
                  console.Write("updating myself...");

                  Process.Start(Path.Combine(PathUtils.ExeFolder, "pundit-updater.exe"));
               }
            }
            else
            {
               console.Write(ConsoleColor.Green, "You are up to date");
            }
         }
      }
   }
}
