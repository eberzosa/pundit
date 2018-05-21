using System;
using EBerzosa.Utils;
using NuGet.Versioning;

namespace EBerzosa.Pundit.Core.Versioning
{
   public class VersionRangeExtended
   {
      public VersionRange NuGetVersionRange { get; }

      public string ReleaseLabel { get; set; }

      public VersionRangeExtended(NuGetVersion minVersion)
      {
         NuGetVersionRange = new VersionRange(minVersion);
      }

      public VersionRangeExtended(NuGetVersion minVersion, NuGetVersion maxVersion, string originalString)
      {
         NuGetVersionRange = new VersionRange(minVersion, true, maxVersion, false, null, originalString);
      }

      /// <summary>
      /// True if the given version falls into the floating range.
      /// </summary>
      public bool Satisfies(NuGetVersion version)
      {
         Guard.NotNull(version, nameof(version));

         return ReleaseLabel == null
            ? NuGetVersionRange.Satisfies(version, NuGet.Versioning.VersionComparison.VersionRelease)
            : NuGetVersionRange.Satisfies(version, VersionComparer.Pundit);
      }
   }
}
