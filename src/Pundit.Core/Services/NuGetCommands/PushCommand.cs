using System;
using System.IO;
using System.Linq;
using EBerzosa.Utils;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace EBerzosa.Pundit.Core.Services.NuGetCommands
{
   internal class NuGetPushService
   {
      private const int TimeOut = 10;

      private readonly string _packagePath;

      public string Source { private get; set; }

      public string SymbolSource { private get; set; }

      public string ApiKey { private get; set; }

      public string SymbolsApiKey { private get; set; }


      public NuGetPushService(string packagePath)
      {
         Guard.NotNull(packagePath, nameof(packagePath));

         _packagePath = packagePath;
      }

      public void Push()
      {
         Guard.NotEmpty(Source, nameof(Source));
         Guard.NotEmpty(ApiKey, nameof(ApiKey));

         var settings = Settings.LoadDefaultSettings(Directory.GetCurrentDirectory(), null, new CommandLineMachineWideSettings());
         var packageSourceProvider = new PackageSourceProvider(settings);
         
         Source = packageSourceProvider.ResolveAndValidateSource(Source);
         SymbolSource = packageSourceProvider.ResolveAndValidateSource(SymbolSource);

         var packageSource = packageSourceProvider.LoadPackageSources().FirstOrDefault(p => p.IsEnabled && p.Source == Source) 
                             ?? new PackageSource(Source, "runtimeRepo", true, false, false);

         var packageUpdate = new CachingSourceProvider(packageSourceProvider).CreateRepository(packageSource).GetResource<PackageUpdateResource>();

         packageUpdate.Push(
            _packagePath, SymbolSource, TimeOut, true,
            endpoint => ApiKey ?? GetApiKey(settings, endpoint, Source, false),
            symbolsEndpoint => SymbolsApiKey ?? GetApiKey(settings, symbolsEndpoint, SymbolSource, true), 
            true, NullLogger.Instance).Wait();
      }

      private static string GetApiKey(ISettings settings, string endpoint, string source, bool isSymbolApiKey)
      {
         var str = SettingsUtility.GetDecryptedValue(settings, ConfigurationConstants.ApiKeys, endpoint)
                   ?? SettingsUtility.GetDecryptedValue(settings, ConfigurationConstants.ApiKeys, source);

         if (str == null && source.IndexOf(NuGetConstants.NuGetHostName, StringComparison.OrdinalIgnoreCase) >= 0)
         {
            var key = isSymbolApiKey 
               ? NuGetConstants.DefaultSymbolServerUrl 
               : NuGetConstants.DefaultGalleryServerUrl;

            str = SettingsUtility.GetDecryptedValue(settings, ConfigurationConstants.ApiKeys, key);
         }

         return str;
      }
   }
}
