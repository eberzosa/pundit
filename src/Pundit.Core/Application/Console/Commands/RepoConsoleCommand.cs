using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Application.Console.Commands
{
   class RepoConsoleCommand : BaseConsoleCommand
   {
      public RepoConsoleCommand(IConsoleOutput console, string currentDirectory, string[] args) : base(console, currentDirectory, args)
      {
      }

      public override void Execute()
      {
         if(NamelessParameters.Length == 0)
            throw new ArgumentException("no action");

         string action = NamelessParameters[0];

         if(action == "list")
            ListRepositories();
         else if(action == "delete")
            DeleteRepository();
         else if(action == "add")
            AddRepository();
         else if(action == "enable")
            EnableRepository(true);
         else if(action == "disable")
            EnableRepository(false);
         else throw new ArgumentException("unknown action " + action);
      }

      private void ListRepositories()
      {
         console.WriteLine("local repository");
         console.Write("env:".PadRight(20));
         console.Write(Environment.GetEnvironmentVariable(LocalConfiguration.LocalRepositoryRootVar) ?? "<not set>");
         console.Write(" (");
         console.Write(LocalConfiguration.LocalRepositoryRootVar);
         console.WriteLine(")");
         console.Write("location:".PadRight(20));
         console.WriteLine(LocalConfiguration.DbLocation);
         console.Write("occupied space:".PadRight(20));
         console.WriteLine(PathUtils.FileSizeToString(LocalConfiguration.RepositoryManager.OccupiedSpace));
         console.Write("binary cache:".PadRight(20));
         console.WriteLine(PathUtils.FileSizeToString(LocalConfiguration.RepositoryManager.OccupiedBinarySpace));

         console.WriteLine("");
         console.Write("configured repositories ({0}):", LocalConfiguration.RepositoryManager.AllRepositories.Count());
         foreach (Repo rr in LocalConfiguration.RepositoryManager.AllRepositories)
         {
            console.WriteLine("");

            console.Write("tag:".PadRight(20));
            console.WriteLine(rr.Tag);

            console.Write("enabled:".PadRight(20));
            console.WriteLine(rr.IsEnabled ? ConsoleColor.Green : ConsoleColor.Red, rr.IsEnabled ? "yes" : "no");

            console.Write("publish:".PadRight(20));
            console.WriteLine(rr.UseForPublishing ? ConsoleColor.Green : ConsoleColor.Yellow,
               rr.UseForPublishing ? "yes" : "no");

            console.Write("url:".PadRight(20));
            console.WriteLine(rr.Uri);

            console.Write("refresh:".PadRight(20));
            console.Write("every ");
            console.Write(rr.RefreshIntervalInHours.ToString());
            console.WriteLine(" hour(s)");

            console.Write("last refreshed:".PadRight(20));
            console.WriteLine(rr.LastRefreshed == DateTime.MinValue ? "never" : rr.LastRefreshed.ToString());

            console.Write("delta:".PadRight(20));
            console.WriteLine(rr.LastChangeId.ToString());
         }

      }

      private void DeleteRepository()
      {
         
      }

      private void AddRepository()
      {
         
      }

      private void EnableRepository(bool enable)
      {
         
      }
   }
}
