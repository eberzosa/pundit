using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Pundit.Core;
using Pundit.Core.Utils;

namespace Pundit.Console.Commands
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

               GlamTerm.Write("downloading update... ");

               string exePath = null;

               try
               {
                  exePath = AutoUpdate.Download(latest, Environment.CurrentDirectory);

                  GlamTerm.WriteOk();
               }
               catch(Exception ex)
               {
                  GlamTerm.WriteFail();

                  GlamTerm.WriteLine("update failed: " + ex.Message);
               }

               if(exePath != null)
               {
                  GlamTerm.Write("updating myself...");

                  Process.Start("pundit-update.exe");
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
