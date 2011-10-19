using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using Pundit.Core.Model;
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
      private void ManageDependenciesCommandCallback(object caller, EventArgs args)
      {
         if(IsInValidState)
         {
            DevPackage pkg = InstantManifest;

            var form = new EditDependenciesForm(pkg.Dependencies);

            if(DialogResult.OK == form.ShowDialog())
            {
               pkg.Dependencies = new List<PackageDependency>(form.Dependencies);

               pkg.WriteTo(ManifestPath);

               Alert.MessageManifestSaved(ManifestPath);
            }
         }
      }
   }
}
