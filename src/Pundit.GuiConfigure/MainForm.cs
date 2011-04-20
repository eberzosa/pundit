using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pundit.Core.Model;

namespace Pundit.GuiConfigure
{
   public partial class MainForm : Form
   {
      private DevPackage _manifest;

      public MainForm()
      {
         InitializeComponent();

         _manifest = TryReadManifest();

         if(_manifest == null) _manifest = new DevPackage();

         DisplayManifest();
      }

      private DevPackage TryReadManifest()
      {
         string path = Path.Combine(Environment.CurrentDirectory, Package.DefaultPackageFileName);

         if(File.Exists(path))
         {
            using(Stream s = File.OpenRead(path))
            {
               return DevPackage.FromStream(s);
            }
         }

         return null;
      }

      private void DisplayManifest()
      {
         txtPackageId.Text = _manifest.PackageId;
         if(_manifest.Version != null) txtVersion.Text = _manifest.Version.ToString();
         txtAuthor.Text = _manifest.Author;
         txtProjectUrl.Text = _manifest.ProjectUrl;
         cbPlatform.Text = _manifest.Platform;
      }

      private void cmdAddDependency_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         SearchForm searchForm = new SearchForm();

         if(DialogResult.OK == searchForm.ShowDialog(this))
         {
            //...
         }
      }

      private void cmdCancel_Click(object sender, EventArgs e)
      {
         this.Close();
      }
   }
}
