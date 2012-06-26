using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pundit.Core.Model;
using Pundit.Vsix.Resources;
using Pundit.WinForms.Core;

namespace Pundit.Vsix.Application
{
   class ManifestUpgrader
   {
      private readonly string _manifestPath;

      public ManifestUpgrader(string manifestPath)
      {
         _manifestPath = manifestPath;
      }

      public bool UpgradeManifest()
      {
         if(NeedsUpgrade)
         {
            if(DialogResult.Yes == Alert.AskYesNo(VSPackage.Manifest_NeedsUpgrade))
            {
               DevPackage p;
               using(Stream s = File.OpenRead(_manifestPath))
               {
                  p = DevPackage.FromStreamXml(s);
               }

               try
               {
                  p.WriteTo(_manifestPath, true);
                  Alert.Message(VSPackage.Manifest_UpgradeOk);
                  return true;
               }
               catch(Exception ex)
               {
                  Alert.Error(string.Format(VSPackage.Manifest_UpgradeFail, ex.Message));
               }
            }
         }
         return false;
      }

      private bool NeedsUpgrade
      {
         get
         {
            /*if(_manifestPath != null && File.Exists(_manifestPath))
            {
               string content = File.ReadAllText(_manifestPath);
               return !content.Contains(Package.XmlNamespace);
            }*/

            return false;
         }
      }
   }
}
