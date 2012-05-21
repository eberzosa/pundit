using System.IO;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.Settings;
using Pundit.Core.Model;
using Pundit.WinForms.Core;
using Package = Microsoft.VisualStudio.Shell.Package;

namespace Pundit.Vsix
{
	public partial class PunditPackage : Package
	{
	   private static WritableSettingsStore _settingsStore;

      private DirectoryInfo GetPropAsDir(__VSPROPID prop)
      {
         IVsSolution solution = GetService(typeof(SVsSolution)) as IVsSolution;

         if (solution != null)
         {
            object objSolutionDir;

            solution.GetProperty((int)prop, out objSolutionDir);

            if (objSolutionDir != null)
            {
               string solutionDir = objSolutionDir.ToString();

               return new DirectoryInfo(solutionDir);
            }
         }

         return null;
      }

      private DirectoryInfo SolutionDirectory
      {
         get { return GetPropAsDir(__VSPROPID.VSPROPID_SolutionDirectory); }
      }


      private void Test()
      {
         IVsProject proj = GetService(typeof (IVsProject)) as IVsProject;

         //proj.AddItem()
      }

	   private const string SettingsRoot = "Pundit\\Common";

      private WritableSettingsStore GetWritableSettingsStore(string settingsRoot)
      {
         SettingsManager settingsManager = new ShellSettingsManager(this);
         WritableSettingsStore userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
         if (!userSettingsStore.CollectionExists(settingsRoot))
         {
            userSettingsStore.CreateCollection(settingsRoot);
         }
         return userSettingsStore;
      }

      public static void SaveSetting(string key, string value)
      {
         _settingsStore.SetString(SettingsRoot, key, value);         
      }

      public static string ReadSetting(string key)
      {
         if(_settingsStore.PropertyExists(SettingsRoot, key))
            return _settingsStore.GetString(SettingsRoot, key);

         return null;
      }
	}
}
