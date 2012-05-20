using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pundit.Core.Application.Console;
using Pundit.Core.Model;

namespace Pundit.Vsix
{
   class ExtensionApplication
   {
      private IConsoleOutput _console;
      private string _manifestDirectoryName;

      private ExtensionApplication()
      {
         
      }

      private static ExtensionApplication _instance;
      public static ExtensionApplication Instance
      {
         get { return _instance ?? (_instance = new ExtensionApplication()); }
      }

      #region [ External Setters ]

      public void AssignConsole(IConsoleOutput console)
      {
         _console = console;
      }

      public void AssignManifestDirectory(string fullName)
      {
         _manifestDirectoryName = fullName;
      }

      #endregion

      #region [ Command Wrappers ]

      public void SearchTextCommand(string text)

      #endregion

      public void ExecuteCommand(string rawText)
      {
         if (string.IsNullOrEmpty(rawText)) return;
         string[] args = rawText.Trim().Split(' ');
         if (args.Length == 0) return;

         try
         {
            IConsoleCommand cmd = CommandFactory.CreateCommand(_console,
               _manifestDirectoryName, args);

            cmd.Execute();
         }
         catch (NoCurrentDirectoryException)
         {
            _console.WriteLine(ConsoleColor.Red, "this command requires a solution to be opened");
         }
         catch (Exception ex)
         {
            _console.WriteLine(ConsoleColor.Red, ex.Message);
         }

      }

   }
}
