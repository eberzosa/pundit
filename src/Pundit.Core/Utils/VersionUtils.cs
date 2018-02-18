using System;
using NuGet.Versioning;

namespace EBerzosa.Pundit.Core
{
   internal static class VersionUtils
   {
      public const string DevMarker = "dev";

      public static VersionRange GetRangeFromPuntitDependencyVersion(string version)
      {
         var parts = version.Split('.');
         
         if (parts.Length > 4 || parts.Length < 1)
            throw new NotSupportedException($"Version '{version}' is not supported");

         var minVersion = NuGetVersion.Parse(version);

         if (parts.Length == 4)
            return new VersionRange(minVersion, true, minVersion, true, null, version);

         var release = parts[parts.Length - 1].Split('-');

         if (release.Length == 1)
            parts[parts.Length - 1] = (int.Parse(parts[parts.Length - 1]) + 1).ToString();
         else
            parts[parts.Length - 1] = (int.Parse(release[0]) + 1).ToString() + '-' + release[1];

         return new VersionRange(minVersion, true, NuGetVersion.Parse(string.Join(".", parts)), false, null, version);
      }
   }
}
