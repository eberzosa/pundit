using System;
using NuGet.Versioning;

namespace EBerzosa.Pundit.Core
{
   internal static class VersionUtils
   {
      public const string DevMarker = "dev";

      public static VersionRange GetRangeFromPuntitDependencyVersion(string version)
      {
         if (version.Contains("*"))
            throw new NotSupportedException("Pundit versions cannot contain *");

         var parts = version.Split('.');

         if (parts.Length > 4 || parts.Length < 1)
            throw new NotSupportedException($"Version '{version}' is not supported");

         if (parts.Length == 4)
            return new VersionRange(NuGetVersion.Parse(version), true, NuGetVersion.Parse(version), true, null, version);

         var indexOfRelease = version.IndexOf('-');

         if (indexOfRelease > -1)
            version = version.Substring(0, indexOfRelease - 1) + ".*" + version.Substring(indexOfRelease);
         else
            version += ".*";

         return VersionRange.Parse(version);
      }

      private static VersionRange GetRangeFromPuntitDependencyVersionUsingRanges(string version)
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
