using System;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Pundit.Vsix.Forms.Console;
using Pundit.WinForms.Core;
using Package = Microsoft.VisualStudio.Shell.Package;

namespace Pundit.Vsix
{
   public partial class PunditPackage : Package
   {
      private void AddReferenceCommandCallback(object caller, EventArgs args)
      {
         //new AddReferenceForm(ManifestDirectory).ShowDialog();
      }

      private void FindPackageCommandCallback(object caller, EventArgs args)
      {
         if(args != null && args != EventArgs.Empty)
         {
            var oleArgs = args as OleMenuCmdEventArgs;
            if(oleArgs != null)
            {
               var text = oleArgs.InValue as string;
               if(!string.IsNullOrEmpty(text))
               {
                  ExtensionApplication.Instance.SearchTextCommand(text, true);
               }
            }
         }
      }

      private void OpenXmlManifestCallback(object caller, EventArgs args)
      {
         ExtensionApplication.Instance.OpenXmlManifestCommand();
      }

      private void ShowHelpCallback(object caller, EventArgs args)
      {
         ExtensionApplication.Instance.ShowHelpCommand();
      }

      private void ShowToolWindow()
      {
         //this method will show the window if it's not active or bring it to front if it's collapsed
         ToolWindowPane window = this.FindToolWindow(typeof(ConsoleVsToolWindow), 0, true);
         if ((null == window) || (null == window.Frame))
         {
            throw new NotSupportedException(VSPackage.CantCreateToolWindow);
         }
         IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
         Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
      }

      private void ResolveDependenciesCommandCallback(object caller, EventArgs args)
      {
         //SaveSetting("LastResolved", DateTime.UtcNow.Ticks.ToString());

         ExtensionApplication.Instance.ResolveDependenciesCommand();
      }

      private void ShowPunditConsoleCallback(object caller, EventArgs args)
      {
         ExtensionApplication.Instance.ShowConsoleCommand();
      }

      private void EnableSolutionButtons(bool enable)
      {
         _cmdResolve.Enabled = enable;
         _cmdAddReference.Enabled = enable;
         _cmdEditManifest.Enabled = enable;
      }

      private void OnSolutionOpened()
      {
         ExtensionApplication.Instance.AssignSolutionDirectory(SolutionDirectory);
         EnableSolutionButtons(true);
      }

      private void OnSolutionClosed()
      {
         ExtensionApplication.Instance.AssignSolutionDirectory(null);
         EnableSolutionButtons(false);
      }
   }
}
