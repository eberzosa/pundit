using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pundit.Core.Model;

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
