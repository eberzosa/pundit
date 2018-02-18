using System;
using EBerzosa.CommandLineProcess;
using EBerzosa.CommandLineProcess.Utils;
using Pundit.Core;
using System.Linq;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.CommandLine.Controllers
{
   internal class SourcesController : Controller
   {
      private readonly LocalRepository _localRepository;

      public SourcesController(LocalRepository localRepository, IOutput output, IInput input) 
         : base(output, input)
      {
         Guard.NotNull(localRepository, nameof(localRepository));

         _localRepository = localRepository;
      }

      public ExitCode Info()
      {
         SafeExecute(() =>
         {
            Output.Title($"Repositories ({_localRepository.Registered.TotalCount} found):");

            var first = true;

            Output.BeginColumns(new int?[] {10, null});

            foreach (var rr in _localRepository.Registered.RepositoriesArray)
            {
               if (!first)
                  Output.Empty();
               else
                  first = false;

               Output.Text("Name:").Text(rr.Name);

               //GlamTerm.Write("enabled:".PadRight(15));
               //GlamTerm.WriteLine(rr.IsEnabled ? ConsoleColor.Green : ConsoleColor.Red, rr.IsEnabled ? "yes" : "no");

               Output.Text("Publish:");

               if (rr.UseForPublishing)
                  Output.Success("yes");
               else
                  Output.Error("no");

               Output.Text("Url:").Text(rr.Uri);
            }

            Output.EndColumns();
         });

         return ExitCode.Success;
      }
   }
}
