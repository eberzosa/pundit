using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pundit.Core.Model;

namespace Pundit.Core.Application.Console.Commands
{
   class HelpConsoleCommand : BaseConsoleCommand
   {
      public HelpConsoleCommand(IConsoleOutput console, string[] args) : base(console, args)
      {
      }

      public override void Execute()
      {
         if(GetCommandLine().Length == 0)
         {
            console.WriteLine(Strings.Help, "");
         }
         else
         {
            string command = GetCommandLine()[0];
            string helpString = Strings.ResourceManager.GetString("Help_" + command);

            if (helpString != null)
            {
               console.WriteLine(helpString, "");
            }
            else
            {
               console.WriteLine(ConsoleColor.Red, "Article {0} not found", command);
            }

         }
      }
   }
}
