using System;
using System.Windows.Forms;

namespace Pundit.Vsix.Forms.Configuration.General
{
   public partial class VsixOptions : UserControl
   {
      public VsixOptions()
      {
         InitializeComponent();

         RestoreSettings();
      }

      private void RestoreSettings()
      {
         chkDoPing.Checked = ExtensionApplication.Instance.Settings.AutoResolveEnabled;

         //min, hr, days
         TimeSpan span = TimeSpan.FromSeconds(ExtensionApplication.Instance.Settings.AutoResolveFrequencySec);
         if(span.TotalDays > 0)
         {
            numPingInterval.Value = ((decimal)span.TotalDays).FitRange(numPingInterval.Minimum, numPingInterval.Maximum);
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
         ExtensionApplication.Instance.Settings.AutoResolveEnabled = chkDoPing.Checked;

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
         ExtensionApplication.Instance.Settings.AutoResolveFrequencySec = (long)span.TotalSeconds;
         ExtensionApplication.Instance.SaveSettings();
      }

      private void chkDoPing_CheckedChanged(object sender, EventArgs e)
      {
         pnlPing.Enabled = chkDoPing.Checked;
      }
   }
}
