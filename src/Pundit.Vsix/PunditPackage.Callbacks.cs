using System;
using System.IO;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Pundit.Vsix.Forms;
using Pundit.Vsix.Forms.Console;
using Pundit.WinForms.Core;
using Package = Microsoft.VisualStudio.Shell.Package;

namespace Pundit.Vsix
{
   public partial class PunditPackage : Package
   {
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Microsoft.Samples.VisualStudio.MenuCommands.PunditPackage.OutputCommandString(System.String)")]
      private void AddReferenceCommandCallback(object caller, EventArgs args)
      {
         //Alert.Message("Good news, 'add referene' menu extended! WOOOOOOOOW IT'S A DOUBLE RAINBOW!!!!!!! DOUBLE RAINBOW ALL THE WAY!!!!");
         new AddReferenceForm(ManifestDirectory).ShowDialog();
      }

      private void FindPackageCommandCallback(object caller, EventArgs args)
      {
         if(args != null && args != EventArgs.Empty)
         {
            var oleArgs = args as OleMenuCmdEventArgs;
            if(oleArgs != null)
            {
               string text = oleArgs.InValue as string;
               if(!string.IsNullOrEmpty(text))
               {
                  ConsoleVsToolWindow.Search(text, true);
                  ShowToolWindow();
               }
            }
         }
      }

      private void OpenXmlManifest(object caller, EventArgs args)
      {
         
      }

      private void ShowToolWindow()
      {
         //this method will show the window if it's not active or bring it to front if it's collapsed
         ToolWindowPane window = this.FindToolWindow(typeof(ConsoleVsToolWindow), 0, true);
         if ((null == window) || (null == window.Frame))
         {
            throw new NotSupportedException(Strings.CantCreateToolWindow);
         }
         IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
         Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());         
      }

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Microsoft.Samples.VisualStudio.MenuCommands.PunditPackage.OutputCommandString(System.String)")]
      private void ResolveDependenciesCommandCallback(object caller, EventArgs args)
      {
         SaveSetting("LastResolved", DateTime.UtcNow.Ticks.ToString());

         ShowToolWindow();

         ConsoleVsToolWindow.ResolveDependencies();
      }

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Microsoft.Samples.VisualStudio.MenuCommands.PunditPackage.OutputCommandString(System.String)")]
      private void ShowPunditConsoleCallback(object caller, EventArgs args)
      {
         ShowToolWindow();
      }

      private void OnSolutionOpened()
      {
         LastSolutionDirectory = SolutionDirectory;
         _cmdResolve.Visible = true;

         //StartBackgroundActivity();
      }

      private void OnSolutionClosed()
      {
         LastSolutionDirectory = null;
         _cmdResolve.Visible = false;

         //StopBackgroundActivity();
      }
   }
}
