using EBerzosa.Utils;
using NuGet.Versioning;

namespace EBerzosa.Pundit.Core.Versioning
{
   public class VersionRangeExtended
   {
      private readonly VersionRange _versionRange;
      private readonly FloatRangeExtended _floatRange;

      public string OriginalString => _versionRange.OriginalString;

      /// <summary>
      /// True if the range has a floating version above the min version.
      /// </summary>
      public bool IsFloating => _floatRange != null && _floatRange.FloatBehaviour != FloatBehaviour.None;

      public VersionRangeExtended(VersionRange versionRange)
      {
         Guard.NotNull(versionRange, nameof(versionRange));

         _versionRange = versionRange;
      }

      public VersionRangeExtended(FloatRangeExtended floatRange)
      {
         Guard.NotNull(floatRange, nameof(floatRange));

         _floatRange = floatRange;
      }

      public static implicit operator VersionRangeExtended(VersionRange versionRange)
      {
         return versionRange != null ? new VersionRangeExtended(versionRange) : null;
      }
   }
}
