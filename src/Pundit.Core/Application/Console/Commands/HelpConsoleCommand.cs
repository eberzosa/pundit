using System;
using Pundit.Core.Model;

namespace Pundit.Core.Application.Console.Commands
{
   class HelpConsoleCommand : BaseConsoleCommand
   {
      public HelpConsoleCommand(IConsoleOutput console, string currentDirectory,string[] args) : base(console, currentDirectory, args)
      {
      }

      public override void Execute()
      {
         string[] cmdLine = GetCommandLine();
         if(cmdLine == null || cmdLine.Length == 0)
         {
            console.WriteLine(Strings.Help, "pundit");
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
