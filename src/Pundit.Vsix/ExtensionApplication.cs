﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pundit.Core.Application.Console;
using Pundit.Core.Model;
using Pundit.Vsix.Application;
using Pundit.Vsix.Model;
using Pundit.Vsix.Resources;
using Pundit.WinForms.Core;
using Pundit.WinForms.Core.Forms;

namespace Pundit.Vsix
{
   interface IPunditVsCommands
   {
      void ShowConsoleToolWindow(bool bringToFront);

      void OpenFileInEditor(string fullPath);

      void SaveOption(string key, string value);

      string ReadOption(string key);

      void EnableSolutionButtons(bool enable);
   }

   enum SolutionStatus
   {
      Loaded,
      Unloaded
   }

   class ExtensionApplication
   {
      private IConsoleOutput _console;
      private DirectoryInfo _solutionDirectory;
      private IPunditVsCommands _vsCommands;
      private ExtensionSettings _settings;
      private StatusBarManager _statusBar;

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
         //IntellisenseInstaller.InteractiveInstallShema(console);
      }

      public void AssignVsCommands(IPunditVsCommands vsCommands)
      {
         _vsCommands = vsCommands;
         if(_vsCommands != null) _vsCommands.EnableSolutionButtons(false);
      }

      public void AssignSolutionDirectory(DirectoryInfo directory)
      {
         _solutionDirectory = directory;
      }

      public void SolutionStatusUpdated(SolutionStatus status)
      {
         switch(status)
         {
            case SolutionStatus.Loaded:
               if(new ManifestUpgrader(ManifestPath).UpgradeManifest())
               {
                  if(_vsCommands != null) _vsCommands.OpenFileInEditor(ManifestPath);
               }
               if(_vsCommands != null) _vsCommands.EnableSolutionButtons(true);
               if(_statusBar == null)
               {
                  _statusBar = new StatusBarManager();
                  _statusBar.StatusIcon = StatusIcon.Yellow;
                  _statusBar.StatusText = null;
               }
               break;
            case SolutionStatus.Unloaded:
               if(_vsCommands != null) _vsCommands.EnableSolutionButtons(false);
               if(_statusBar != null)
               {
                  _statusBar.Dispose();
                  _statusBar = null;
               }
               _solutionDirectory = null;
               break;
         }
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
                           package.WriteXmlTo(s);
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
         if(_vsCommands != null) _vsCommands.ShowConsoleToolWindow(true);
      }

      #endregion

      public ExtensionSettings Settings
      {
         get
         {
            if(_settings == null && _vsCommands != null)
            {
               string xml = _vsCommands.ReadOption("settings");
               if (!string.IsNullOrEmpty(xml)) _settings = ExtensionSettings.Deserialize(xml);

               if(_settings == null) _settings = new ExtensionSettings();
            }

            return _settings;
         }
      }

      public void SaveSettings()
      {
         if(_vsCommands != null && _settings != null)
         {
            _vsCommands.SaveOption("settings", _settings.Serialize());
         }
      }

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
         if (_console == null) return;
         if (string.IsNullOrEmpty(rawText)) return;
         string[] args = rawText.Trim().Split(' ');
         if (args.Length == 0) return;

         try
         {
            if(_vsCommands != null) _vsCommands.ShowConsoleToolWindow(false);

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
            if (_vsCommands != null) _vsCommands.ShowConsoleToolWindow(true);
         }
      }
   }
}