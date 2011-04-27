using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Pundit.Core.Model;
using Pundit.WinForms.Core;

namespace Pundit.WinForms.App
{
   public partial class MainForm : Form
   {
      private string _manifestPath;
      private DevPackage _manifest;

      public MainForm()
      {
         InitializeComponent();

         _manifest = TryReadManifest();

         if(_manifest == null) _manifest = new DevPackage();

         Text = string.Format(Text, _manifest.PackageId);
         ucPackageDependencies.Dependencies = _manifest.Dependencies;
      }

      private DevPackage TryReadManifest()
      {
         string path = Path.Combine(Environment.CurrentDirectory, Package.DefaultPackageFileName);

         if(File.Exists(path))
         {
            _manifestPath = path;

            using(Stream s = File.OpenRead(path))
            {
               return DevPackage.FromStream(s);
            }
         }

         return null;
      }

      private void cmdCancel_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      private void cmdMetadata_Click(object sender, EventArgs e)
      {
         var form = new MetadataForm(_manifest);

         form.ShowDialog();
      }

      private void cmdSave_Click(object sender, EventArgs e)
      {
         _manifest.Dependencies = new List<PackageDependency>(ucPackageDependencies.Dependencies);

         _manifest.WriteTo(_manifestPath, true);

         MessageBox.Show("manifest saved to " + _manifestPath, "Success");
      }

      private void cmdPublishing_Click(object sender, EventArgs e)
      {
         PackageFilesForm form = new PackageFilesForm(_manifest.Files);

         if(form.ShowDialog() == DialogResult.OK)
         {
            _manifest.Files = new List<SourceFiles>(form.Files);
         }
      }
   }
}
