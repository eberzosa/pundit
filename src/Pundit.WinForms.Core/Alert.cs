using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pundit.Core;

namespace Pundit.WinForms.Core
{
   public static class Alert
   {
      private static readonly string DefaultAlertTitle;

      static Alert()
      {
         DefaultAlertTitle = "Pundit v" + typeof (LocalRepository).Assembly.GetName().Version;
      }

      public static void Message(string msg)
      {
         MessageBox.Show(msg, DefaultAlertTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
      }

      public static void Error(string msg)
      {
         MessageBox.Show(msg, DefaultAlertTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      public static void MessageManifestSaved(string manifestLocation)
      {
         Message(string.Format(AlertStrings.MessageManifestSaved, manifestLocation));
      }
   }
}
