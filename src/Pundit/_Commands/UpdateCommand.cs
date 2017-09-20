using System;
using System.Diagnostics;
using System.IO;
using Pundit.Core;
using Pundit.Core.Utils;

namespace Legacy
{
   class UpdateCommand : BaseCommand
   {
      public UpdateCommand(string[] args) : base(args)
      {
      }

      public override void Execute()
      {
         Version current = PackageUtils.GetProductVersion(typeof (UpdateCommand).Assembly);
         AutoUpdate.Product latest = null;

         GlamTerm.WriteLine("Checking for updates...");

         try
         {
            latest = AutoUpdate.CheckUpdates();
         }
         catch(Exception ex)
         {
            GlamTerm.WriteFail();
         }

         if(latest != null)
         {
            GlamTerm.Write("current version: ");
            GlamTerm.Write(ConsoleColor.Green, current.ToString());
            GlamTerm.Write(", latest version: ");
            GlamTerm.WriteLine(ConsoleColor.Yellow, latest.Version.ToString());

            if(latest.Version > current)
            {
               GlamTerm.WriteLine(ConsoleColor.Red, "You have an update!");

               GlamTerm.Write("downloading update to " + PathUtils.ExeFolder + "... ");

               string exePath = null;

               try
               {
                  exePath = AutoUpdate.Download(latest, PathUtils.ExeFolder);

                  GlamTerm.WriteOk();
               }
               catch(Exception ex)
               {
                  GlamTerm.WriteFail();

                  GlamTerm.WriteLine("update failed: " + ex.Message + ex.StackTrace);
               }

               if(exePath != null)
               {
                  GlamTerm.Write("updating myself...");

                  Process.Start(Path.Combine(PathUtils.ExeFolder, "pundit-updater.exe"));
               }
            }
            else
            {
               GlamTerm.Write(ConsoleColor.Green, "You are up to date");
            }
         }
      }
   }
}
