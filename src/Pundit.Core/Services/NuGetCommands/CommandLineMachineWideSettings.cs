using System;
using System.Collections.Generic;
using NuGet.Common;
using NuGet.Configuration;

namespace EBerzosa.Pundit.Core.Services.NuGetCommands
{
   public class CommandLineMachineWideSettings : IMachineWideSettings
   {
      private readonly Lazy<IEnumerable<Settings>> _settings;

      public IEnumerable<Settings> Settings => _settings.Value;


      public CommandLineMachineWideSettings()
      {
         var baseDirectory = NuGetEnvironment.GetFolderPath(NuGetFolderPath.MachineWideConfigDirectory);
         _settings = new Lazy<IEnumerable<Settings>>(() => NuGet.Configuration.Settings.LoadMachineWideSettings(baseDirectory));
      }
   }
}