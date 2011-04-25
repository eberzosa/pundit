using System;
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
         using (Stream s = File.Open(_manifestPath, FileMode.OpenOrCreate, FileAccess.Write))
         {
            _manifest.WriteTo(s);
         }

         MessageBox.Show("manifest saved to " + _manifestPath, "Success");
      }
   }
}
