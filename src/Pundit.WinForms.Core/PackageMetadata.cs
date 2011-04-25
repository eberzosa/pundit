using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pundit.Core.Model;

namespace Pundit.WinForms.Core
{
   public partial class PackageMetadata : UserControl
   {
      private Package _pkg;

      public PackageMetadata()
      {
         InitializeComponent();

         if(_pkg != null)
         {
            ShowPackage();
         }
      }

      public void ShowPackage()
      {
         if (_pkg != null)
         {
            txtPackageId.Text = _pkg.PackageId;
            if (_pkg.Version != null) txtVersion.Text = _pkg.Version.ToString();
            cbPlatform.Text = _pkg.Platform;
            txtProjectUri.Text = _pkg.ProjectUrl;
            txtAuthor.Text = _pkg.Author;
         }
      }

      private void CollectPackage()
      {
         _pkg.PackageId = txtPackageId.Text;
         _pkg.Version = new Version(txtVersion.Text);
         _pkg.Platform = cbPlatform.Text;
         _pkg.ProjectUrl = txtProjectUri.Text;
         _pkg.Author = txtAuthor.Text;
      }

      public Package Package
      {
         set { _pkg = value; }
         get
         {
            CollectPackage();
            return _pkg;
         }
      }

      private void cmdViewDescription_Click(object sender, EventArgs e)
      {
         if (_pkg != null)
         {
            var form = new PlainTextModalEditForm("Description", _pkg.Description);

            if(form.ShowDialog() == DialogResult.OK)
            {
               _pkg.Description = form.EditedText;
            }
         }
      }

      private void cmdViewReleaseNotes_Click(object sender, EventArgs e)
      {
         if (_pkg != null)
         {
            var form = new PlainTextModalEditForm("Release Notes", _pkg.ReleaseNotes);

            if (form.ShowDialog() == DialogResult.OK)
            {
               _pkg.ReleaseNotes = form.EditedText;
            }
         }
      }

      private void cmdViewLicense_Click(object sender, EventArgs e)
      {
         if (_pkg != null)
         {
            var form = new PlainTextModalEditForm("License", _pkg.License);

            if (form.ShowDialog() == DialogResult.OK)
            {
               _pkg.License = form.EditedText;
            }
         }
      }
   }
}
