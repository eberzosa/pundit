using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pundit.Core.Application.Console;
using Pundit.Core.Model;
using Pundit.WinForms.Core;
using Pundit.WinForms.Core.Forms;

namespace Pundit.Vsix
{
   interface IPunditVsCommands
   {
      void ShowConsoleToolWindow();

      void OpenFileInEditor(string fullPath);
   }

   class ExtensionApplication
   {
      private IConsoleOutput _console;
      private DirectoryInfo _solutionDirectory;
      private IPunditVsCommands _vsCommands;

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

      public void AssignVsCommands(IPunditVsCommands vsCommands)
      {
         _vsCommands = vsCommands;
      }

      public void AssignSolutionDirectory(DirectoryInfo directory)
      {
         _solutionDirectory = directory;
      }

      #endregion

      #region [ Command Wrappers ]

      public void SearchTextCommand(string text, bool formatXml)
      {
         ExecuteCommand("search " + text + (formatXml ? " -x" : ""));
      }

      public void ResolveDependenciesCommand()
      {
         ExecuteCommand("resolve");
      }

      public void OpenXmlManifestCommand()
      {
         if(_vsCommands != null)
         {
            string path = ManifestPath;
            if (path != null)
            {
               if (File.Exists(path))
               {
                  _vsCommands.OpenFileInEditor(path);
               }
               else
               {
                  if(DialogResult.Yes == Alert.AskYesNo(string.Format(VSPackage.OpenXmlManifest_DoesntExist, path)))
                  {
                     var form = new AddManifestForm();
                     if(DialogResult.OK == form.ShowDialog())
                     {
                        var package = new DevPackage();
                        package.PackageId = form.PackageId;
                        package.Platform = form.Platform;
                        package.Version = new Version(1, 0, 0, 0);
                        
                        using(Stream s = File.Create(path))
                        {
                           package.WriteTo(s);
                        }

                        _vsCommands.OpenFileInEditor(path);
                     }
                  }
               }
            }
         }
      }

      public void ShowHelpCommand()
      {
         ExecuteCommand("help");
      }

      public void ShowConsoleCommand()
      {
         if(_vsCommands != null) _vsCommands.ShowConsoleToolWindow();
      }

      #endregion

      private DirectoryInfo ManifestDirectory
      {
         get
         {
            if (_solutionDirectory != null && _solutionDirectory.Parent != null)
            {
               return _solutionDirectory.Parent;
            }

            return null;
         }
      }

      private string ManifestPath
      {
         get
         {
            DirectoryInfo md = ManifestDirectory;

            if (md != null)
            {
               string manifestPath = Path.Combine(
                  md.FullName,
                  Package.DefaultManifestFileName);

               return manifestPath;
            }

            return null;
         }
      }

      private bool SolutionHasManifest
      {
         get
         {
            string path = ManifestPath;

            return path != null && File.Exists(path);
         }
      }

      public void ExecuteCommand(string rawText)
      {
         Task.Factory.StartNew(() => ExecuteCommandImpl(rawText), TaskCreationOptions.LongRunning);
      }

      private void ExecuteCommandImpl(string rawText)
      {
         if (string.IsNullOrEmpty(rawText)) return;
         string[] args = rawText.Trim().Split(' ');
         if (args.Length == 0) return;

         try
         {
            if(_vsCommands != null) _vsCommands.ShowConsoleToolWindow();

            IConsoleCommand cmd = CommandFactory.CreateCommand(_console,
               ManifestDirectory == null ? null : ManifestDirectory.FullName,
               args);

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
         finally
         {
            _console.FinishCommand();
            if (_vsCommands != null) _vsCommands.ShowConsoleToolWindow();
         }
      }
   }
}
