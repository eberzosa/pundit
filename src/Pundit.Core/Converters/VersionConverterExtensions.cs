using System;

namespace EBerzosa.Pundit.Core.Converters
{
   internal static class VersionConverterExtensions
   {
      public static NuGet.Versioning.VersionRange ConvertPunditDependencyVersionToVersionRange(string punditDependencyVersion)
      {
         if (punditDependencyVersion.IndexOf('-') != -1)
            throw new NotSupportedException("Versions cannot contain Release part");
         
         var minVersion = NuGet.Versioning.NuGetVersion.Parse(punditDependencyVersion);

         var parts = punditDependencyVersion.Split('.');

         if (parts.Length == 4)
            return new NuGet.Versioning.VersionRange(minVersion);

         if (parts.Length == 3)
            return new NuGet.Versioning.VersionRange(
               minVersion, true, 
               new NuGet.Versioning.NuGetVersion(minVersion.Major, minVersion.Minor, minVersion.Patch + 1), false, 
               null, punditDependencyVersion);

         if (parts.Length == 2)
            return new NuGet.Versioning.VersionRange(
               minVersion, true, 
               new NuGet.Versioning.NuGetVersion(minVersion.Major, minVersion.Minor + 1, 0), false,
               null, punditDependencyVersion);

         if (parts.Length == 1)
            return new NuGet.Versioning.VersionRange(
               minVersion, true, 
               new NuGet.Versioning.NuGetVersion(minVersion.Major + 1, 0, 0), false,
               null, punditDependencyVersion);

         throw new NotSupportedException($"Version '{punditDependencyVersion}' is not supported");
      }
   }
}