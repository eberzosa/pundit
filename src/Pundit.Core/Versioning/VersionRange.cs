using System;

namespace EBerzosa.Pundit.Core.Versioning
{
   public class VersionRange : IFormattable, IEquatable<VersionRange>
   {
      public NuGet.Versioning.VersionRange NuGetVersionRange { get; }
      
      public PunditVersion MinVersion { get; }

      public bool IsMinInclusive => NuGetVersionRange.IsMinInclusive;
     
      public PunditVersion MaxVersion { get; }

      public bool IsMaxInclusive => NuGetVersionRange.IsMaxInclusive;

      public string OriginalString => NuGetVersionRange.OriginalString;


      public VersionRange(NuGet.Versioning.VersionRange range)
      {
         NuGetVersionRange = range;
         MinVersion = new PunditVersion(NuGetVersionRange.MinVersion);
         MaxVersion = new PunditVersion(NuGetVersionRange.MaxVersion);
      }

      public VersionRange(PunditVersion minVersion = null, bool includeMinVersion = true, PunditVersion maxVersion = null,
         bool includeMaxVersion = false, NuGet.Versioning.FloatRange floatRange = null, string originalString = null)
      {
         NuGetVersionRange = new NuGet.Versioning.VersionRange(minVersion.NuGetVersion, includeMinVersion, maxVersion.NuGetVersion, includeMaxVersion,
            floatRange, originalString);

         MinVersion = minVersion;
         MaxVersion = maxVersion;
      }

      public static VersionRange Parse(string value)
      {
         var range = NuGet.Versioning.VersionRange.Parse(value);
         return range == null ? null : new VersionRange(range);
      }


      public override string ToString() => NuGetVersionRange.ToString();

      public string ToString(string format, IFormatProvider formatProvider) => NuGetVersionRange.ToString(format, formatProvider);

      public bool Equals(VersionRange other) => NuGetVersionRange.Equals(other?.NuGetVersionRange);
   }
}
