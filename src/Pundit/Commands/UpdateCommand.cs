using System;
using System.Collections.Generic;
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
            latest = AutoUpdate.CheckUpdates().First();
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
               GlamTerm.Write(ConsoleColor.Red, "You have an update!");
            }
            else
            {
               GlamTerm.Write(ConsoleColor.Green, "You are up to date");
            }
         }
      }
   }
}
