using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.Settings;

namespace Pundit.Vsix.AppPackage
{
   //IPunditVsCommands
   public partial class PunditPackage
   {
      public void ShowConsoleToolWindow(bool bringToFront)
      {
         ShowToolWindow(bringToFront);
      }

      public void OpenFileInEditor(string fullPath)
      {
         IVsCommandWindow service = (IVsCommandWindow)this.GetService(typeof(SVsCommandWindow));
         if (service != null)
         {
            string command = string.Format("File.OpenFile \"{0}\"", fullPath);
            service.ExecuteCommand(command);
         }
      }

      private const string SettingsRoot = "Pundit\\Common";
      private static WritableSettingsStore _settingsStore;

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

      public void SaveOption(string key, string value)
      {
         _settingsStore.SetString(SettingsRoot, key, value);
      }

      public string ReadOption(string key)
      {
         if (_settingsStore.PropertyExists(SettingsRoot, key))
            return _settingsStore.GetString(SettingsRoot, key);

         return null;
      }

      public void EnableSolutionButtons(bool enable)
      {
         _cmdResolve.Enabled = enable;
         _cmdAddReference.Enabled = enable;
         _cmdEditManifest.Enabled = enable;

         _cmdHelp.Enabled = true;
         _cmdSearch.Enabled = true;
      }
   }
}
