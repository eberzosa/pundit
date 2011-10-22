using System;
using System.Linq;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Application.Console.Commands
{
   class InfoConsoleCommand : BaseConsoleCommand
   {
      public InfoConsoleCommand(IConsoleOutput console, string[] args) : base(console, args)
      {

      }

      public override void Execute()
      {
         console.WriteLine("local repository");
         console.Write("location:       ");
         console.WriteLine(LocalRepository.GlobalRootPath);
         console.Write("occupied space: ");
         console.WriteLine(PathUtils.FileSizeToString(LocalRepository.OccupiedSpace));

         console.WriteLine("");
         console.Write("configured repositories ({0}):", LocalRepository.Registered.All.Count());
         foreach(RegisteredRepository rr in LocalRepository.Registered.All)
         {
            console.WriteLine("");

            console.Write("name:".PadRight(15));
            console.WriteLine(rr.Name);

            console.Write("enabled:".PadRight(15));
            console.WriteLine(rr.IsEnabled ? ConsoleColor.Green : ConsoleColor.Red, rr.IsEnabled ? "yes" : "no");

            console.Write("publish:".PadRight(15));
            console.WriteLine(rr.UseForPublishing ? ConsoleColor.Green : ConsoleColor.Yellow,
               rr.UseForPublishing ? "yes" : "no");

            console.Write("url:".PadRight(15));
            console.WriteLine(rr.Uri);
         }
      }
   }
}
