using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Pundit.Vsix.Properties;

namespace Pundit.Vsix.Forms
{
   public partial class VsixOptions : UserControl
   {
      public VsixOptions()
      {
         InitializeComponent();

         lblVersion.Text = string.Format(lblVersion.Text, Assembly.GetExecutingAssembly().GetName().Version);

         RestoreSettings();
      }

      private void RestoreSettings()
      {
         //chkDoPing.Checked = PunditPackage.Settings.GetBoolean("Pundit", "DoPing");
      }

      public void Save()
      {
         //PunditPackage.Settings.SetBoolean("Pundit", "DoPing", chkDoPing.Checked);
      }

      private void chkDoPing_CheckedChanged(object sender, EventArgs e)
      {
         pnlPing.Enabled = chkDoPing.Checked;
      }

      private void lnkDocs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         Process.Start("http://pundit.codeplex.com/documentation");
      }

      private void lnkLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         Process.Start("http://pundit.codeplex.com/license");
      }
   }
}
