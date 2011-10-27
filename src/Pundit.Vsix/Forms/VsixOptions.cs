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
         string sPing = PunditPackage.ReadSetting("NotifPing");
         string sNum = PunditPackage.ReadSetting("NotifTimeSpanTicks");

         bool ping;
         long ticks;

         if (!bool.TryParse(sPing, out ping)) ping = false;
         if (!long.TryParse(sNum, out ticks)) ticks = TimeSpan.FromHours(1).Ticks;

         chkDoPing.Checked = ping;

         //min, hr, days
         TimeSpan span = TimeSpan.FromTicks(ticks);
         if(span.TotalDays > 0)
         {
            numPingInterval.Value = (decimal)span.TotalDays;
            cmbPingMeasure.SelectedIndex = 2;
         }
         else if(span.TotalHours > 0)
         {
            numPingInterval.Value = (decimal) span.TotalHours;
            cmbPingMeasure.SelectedIndex = 1;
         }
         else
         {
            numPingInterval.Value = (decimal) span.TotalMinutes;
            cmbPingMeasure.SelectedIndex = 0;
         }
      }

      public void Save()
      {
         PunditPackage.SaveSetting("NotifPing", chkDoPing.Checked.ToString());

         TimeSpan span;
         switch(cmbPingMeasure.SelectedIndex)
         {
            case 0:
               span = TimeSpan.FromMinutes((double) numPingInterval.Value);
               break;
            case 1:
               span = TimeSpan.FromHours((double) numPingInterval.Value);
               break;
            default:
               span = TimeSpan.FromDays((double) numPingInterval.Value);
               break;
         }
         PunditPackage.SaveSetting("NotifTimeSpanTicks", span.Ticks.ToString());
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
