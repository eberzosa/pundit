using System;
using EBerzosa.Pundit.Core.Versioning;

namespace EBerzosa.Pundit.Core.Converters
{
   internal static class VersionConverterExtensions
   {
      public static VersionRangeExtended ConvertPunditDependencyVersionToVersionRangeExtended(string punditDependencyVersion)
      {
         if (punditDependencyVersion.IndexOf('-') != -1)
            throw new NotSupportedException("Versions cannot contain Release part");
         
         var minVersion = NuGet.Versioning.NuGetVersion.Parse(punditDependencyVersion);

         var parts = punditDependencyVersion.Split('.');

         if (parts.Length == 4)
            return new VersionRangeExtended(minVersion);

         if (parts.Length == 3)
            return new VersionRangeExtended(minVersion, new NuGet.Versioning.NuGetVersion(minVersion.Major, minVersion.Minor, minVersion.Patch + 1), punditDependencyVersion);

         if (parts.Length == 2)
            return new VersionRangeExtended(minVersion, new NuGet.Versioning.NuGetVersion(minVersion.Major, minVersion.Minor + 1, 0), punditDependencyVersion);

         if (parts.Length == 1)
            return new VersionRangeExtended(minVersion, new NuGet.Versioning.NuGetVersion(minVersion.Major + 1, 0, 0), punditDependencyVersion);

         throw new NotSupportedException($"Version '{punditDependencyVersion}' is not supported");
      }
   }
}