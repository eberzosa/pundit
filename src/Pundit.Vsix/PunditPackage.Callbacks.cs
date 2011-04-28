using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using Pundit.WinForms.Core;

namespace Pundit.Vsix
{
   public partial class PunditPackage : Package
   {
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Microsoft.Samples.VisualStudio.MenuCommands.PunditPackage.OutputCommandString(System.String)")]
      private void AddReferenceCommandCallback(object caller, EventArgs args)
      {
         MessageBox.Show("Good news, 'add referene' menu extended!");
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
            MessageBox.Show(sd.FullName);
      }
   }
}
