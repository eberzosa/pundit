using System;
using NuGet.Versioning;

namespace EBerzosa.Pundit.Core.Versioning
{
   public class PunditVersion : IFormattable, IComparable, IComparable<PunditVersion>, IEquatable<PunditVersion>
   {
      public NuGetVersion NuGetVersion { get; }

      public int Major => NuGetVersion.Major;

      public int Minor => NuGetVersion.Minor;

      public int Patch => NuGetVersion.Patch;

      public int Revision => NuGetVersion.Revision;

      public string Release => NuGetVersion.Release;

      public string OriginalVersion => NuGetVersion.OriginalVersion;

      public Version Version => NuGetVersion.Version;


      public PunditVersion(NuGetVersion version) 
         => NuGetVersion = version;

      public PunditVersion(Version version)
         => NuGetVersion = new NuGetVersion(version);

      public PunditVersion(int major, int minor, int patch, int revision)
         => NuGetVersion = new NuGetVersion(major, minor, patch, revision);

      public PunditVersion(int major, int minor, int patch, int revision, string release, string metadata) 
         => NuGetVersion = new NuGetVersion(major, minor, patch, revision, release, metadata);

      public static PunditVersion Parse(string value)
      {
         var version = NuGetVersion.Parse(value);
         return version == null ? null : new PunditVersion(version);
      }


      /// <summary>Equals</summary>
      public static bool operator ==(PunditVersion version1, PunditVersion version2) => Compare(version1, version2) == 0;

      /// <summary>Not equal</summary>
      public static bool operator !=(PunditVersion version1, PunditVersion version2) => (uint)Compare(version1, version2) > 0U;

      /// <summary>Less than</summary>
      public static bool operator <(PunditVersion version1, PunditVersion version2) => Compare(version1, version2) < 0;

      /// <summary>Less than or equal</summary>
      public static bool operator <=(PunditVersion version1, PunditVersion version2) => Compare(version1, version2) <= 0;

      /// <summary>Greater than</summary>
      public static bool operator >(PunditVersion version1, PunditVersion version2) => Compare(version1, version2) > 0;

      /// <summary>Greater than or equal</summary>
      public static bool operator >=(PunditVersion version1, PunditVersion version2) => Compare(version1, version2) >= 0;

      private static int Compare(PunditVersion version1, PunditVersion version2)
      {
         if ((object)version1 == null && (object)version2 == null)
            return 0;

         if ((object)version1 != null && (object)version2 == null)
            return 1;

         if ((object)version1 == null)
            return -1;

         return VersionComparer.Default.Compare(version1.NuGetVersion, version2.NuGetVersion);
      }


      public override string ToString() => NuGetVersion.ToString();

      public override int GetHashCode() => NuGetVersion.GetHashCode();

      public string ToString(string format, IFormatProvider formatProvider) => NuGetVersion.ToString(format, formatProvider);

      public int CompareTo(object obj) => NuGetVersion.CompareTo(obj);

      public int CompareTo(PunditVersion other) => NuGetVersion.CompareTo(other?.NuGetVersion);

      public bool Equals(PunditVersion other) => NuGetVersion.Equals(other?.NuGetVersion);
   }
}
