using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pundit.Core;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Console.Commands
{
   class InfoCommand : BaseCommand
   {
      public InfoCommand(string[] args) : base(args)
      {

      }

      public override void Execute()
      {
         GlamTerm.WriteLine("local repository");
         GlamTerm.Write("location:       ");
         GlamTerm.WriteLine(LocalRepository.GlobalRootPath);
         GlamTerm.Write("occupied space: ");
         GlamTerm.WriteLine(PathUtils.FileSizeToString(LocalRepository.OccupiedSpace));

         GlamTerm.WriteLine();
         GlamTerm.Write("configured repositories ({0}):", LocalRepository.Registered.All.Count());
         foreach(RegisteredRepository rr in LocalRepository.Registered.All)
         {
            GlamTerm.WriteLine();

            GlamTerm.Write("name:".PadRight(15));
            GlamTerm.WriteLine(rr.Name);

            GlamTerm.Write("enabled:".PadRight(15));
            GlamTerm.WriteLine(rr.IsEnabled ? ConsoleColor.Green : ConsoleColor.Red, rr.IsEnabled ? "yes" : "no");

            GlamTerm.Write("publish:".PadRight(15));
            GlamTerm.WriteLine(rr.UseForPublishing ? ConsoleColor.Green : ConsoleColor.Yellow,
               rr.UseForPublishing ? "yes" : "no");

            GlamTerm.Write("url:".PadRight(15));
            GlamTerm.WriteLine(rr.Uri);
         }
      }
   }
}
