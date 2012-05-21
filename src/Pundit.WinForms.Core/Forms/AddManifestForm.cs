using System;
using System.Windows.Forms;
using Pundit.Core.Model;

namespace Pundit.WinForms.Core.Forms
{
   public partial class AddManifestForm : Form
   {
      public AddManifestForm()
      {
         InitializeComponent();
      }

      private void PackageIdTextChanged(object sender, EventArgs e)
      {
         cmdOk.Enabled = Package.IsValidPackageNameString(txtPackageId.Text);
      }

      public string PackageId { get { return txtPackageId.Text; } }

      public string Platform { get { return cbPlatform.Text; } }

      private void txtPackageId_KeyUp(object sender, KeyEventArgs e)
      {
         if(e.KeyCode == Keys.Return && cmdOk.Enabled)
         {
            DialogResult = DialogResult.OK;
         }
      }

      private void cbPlatform_KeyUp(object sender, KeyEventArgs e)
      {
         if(e.KeyCode == Keys.Return && cmdOk.Enabled)
         {
            DialogResult = DialogResult.OK;
         }
      }
   }
}
