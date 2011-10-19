using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Pundit.Core.Model;
using Pundit.Vsix.Forms;
using Pundit.WinForms.Core;
using Package = Microsoft.VisualStudio.Shell.Package;

namespace Pundit.Vsix
{
   public partial class PunditPackage : Package
   {
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Microsoft.Samples.VisualStudio.MenuCommands.PunditPackage.OutputCommandString(System.String)")]
      private void AddReferenceCommandCallback(object caller, EventArgs args)
      {
         Alert.Message("Good news, 'add referene' menu extended! WOOOOOOOOW IT'S A DOUBLE RAINBOW!!!!!!! DOUBLE RAINBOW ALL THE WAY!!!!");
      }

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Microsoft.Samples.VisualStudio.MenuCommands.PunditPackage.OutputCommandString(System.String)")]
      private void GlobalSettingsCommandCallback(object caller, EventArgs args)
      {
         new GlobalSettingsForm().ShowDialog();
      }

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Microsoft.Samples.VisualStudio.MenuCommands.PunditPackage.OutputCommandString(System.String)")]
      private void ResolveDependenciesCommandCallback(object caller, EventArgs args)
      {
         DirectoryInfo sd = SolutionDirectory;

         if (sd != null)
         {
            var form = new PackageResolveProcessForm(ManifestPath);

            form.ShowDialog();
         }
      }

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Microsoft.Samples.VisualStudio.MenuCommands.PunditPackage.OutputCommandString(System.String)")]
      private void ShowPunditConsoleCallback(object caller, EventArgs args)
      {
         //just show the tool window
         ToolWindowPane window = this.FindToolWindow(typeof(ConsoleVsToolWindow), 0, true);
         if ((null == window) || (null == window.Frame))
         {
            throw new NotSupportedException(Strings.CantCreateToolWindow);
         }
         IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
         Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
      }
   }
}
