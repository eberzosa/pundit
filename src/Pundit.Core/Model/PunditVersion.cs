using System;
using System.Globalization;
using System.Net.NetworkInformation;

namespace EBerzosa.Pundit.Core.Model
{
   public class PunditVersion : IComparable, IComparable<PunditVersion>, IEquatable<PunditVersion>
   {
      public int Major { get; set; }

      public int Minor { get; set; }

      public int Revision { get; set; }

      public int Build { get; set; }

      public bool IsDeveloper { get; set; }

      public PunditVersion()
      {
      }

      public PunditVersion(int major, int minor, int build, int revision, bool isDeveloper)
      {
         Major = major;
         Minor = minor;
         Build = build;
         Revision = revision;
         IsDeveloper = isDeveloper;
      }

      public PunditVersion(string version)
      {
         if (!TryParse(version, out PunditVersion punditVersion))
            throw new ArgumentException("Invalid version", nameof(version));
         
         Major = punditVersion.Major;
         Minor = punditVersion.Minor;
         Build = punditVersion.Build;
         Revision = punditVersion.Revision;
         IsDeveloper = punditVersion.IsDeveloper;
      }

      public PunditVersion(PunditVersion version)
      {
         Major = version.Major;
         Minor = version.Minor;
         Build = version.Build;
         Revision = version.Revision;
         IsDeveloper = version.IsDeveloper;
      }

      public PunditVersion(Version version)
      {
         Major = version.Major;
         Minor = version.Minor;
         Build = version.Build;
         Revision = version.Revision;
         IsDeveloper = false;
      }
      public int CompareTo(object version)
      {
         var version1 = version as PunditVersion;

         if (version1 == null)
            throw new ArgumentException(nameof(version));

         return CompareTo(version1);
      }

      public int CompareTo(PunditVersion value)
      {
         if (value == null)
            return 1;

         if (Major != value.Major)
            return Major > value.Major ? 1 : -1;

         if (Minor != value.Minor)
            return Minor > value.Minor ? 1 : -1;

         if (Build != value.Build)
            return Build > value.Build ? 1 : -1;

         if (Revision != value.Revision)
            return Revision > value.Revision ? 1 : -1;

         if (IsDeveloper == value.IsDeveloper)
            return 0;

         return IsDeveloper ? -1 : 1;
      }

      public override string ToString() => $"{Major}.{Minor}.{Build}.{Revision}{(IsDeveloper ? "-dev" : "")}";

      public override int GetHashCode()
      {
         return 0 | (Major & 15) << 28 | (Minor & byte.MaxValue) << 20 | (Build & byte.MaxValue) << 12 | Revision & 4095;
      }

      public override bool Equals(object obj)
      {
         var version = obj as PunditVersion;
         return !(version == null) && Equals(version);
      }
      
      public bool Equals(PunditVersion obj)
      {
         return !(obj == null) && Major == obj.Major && (Minor == obj.Minor && Build == obj.Build) && Revision == obj.Revision;
      }

      public static PunditVersion Parse(string version)
      {
         if (!TryParse(version, out PunditVersion punditVersion))
            throw new ArgumentException("Is not a PunditVersion", nameof(version));

         return punditVersion;
      }

      public static bool TryParse(string version, out PunditVersion punditVersion)
      {
         punditVersion = null;

         if (version == null)
            return false;

         string[] versionDeveloper = version.Split('-');

         if (versionDeveloper.Length < 1 || versionDeveloper.Length > 2)
            return false;

         var isDeveloper = versionDeveloper.Length == 2 && "dev".Equals(versionDeveloper[1], StringComparison.OrdinalIgnoreCase);

         var versionArray = versionDeveloper[0].Split('.');
         
         if (versionArray.Length < 2 || versionArray.Length > 4)
            return false;

         if (!TryParseComponent(versionArray[0], out var major) || !TryParseComponent(versionArray[1], out var minor))
            return false;

         if (versionArray.Length < 3)
         {
            punditVersion = new PunditVersion(major, minor, 0, 0, isDeveloper);
            return true;
         }

         if (!TryParseComponent(versionArray[2], out var build))
            return false;

         if (versionArray.Length < 4)
         {
            punditVersion = new PunditVersion(major, minor, build, 0, isDeveloper);
            return true;
         }

         if (!TryParseComponent(versionArray[3], out var revision))
            return false;

         punditVersion = new PunditVersion(major, minor, build, revision, isDeveloper);
         return true;
      }

      private static bool TryParseComponent(string component, out int parsedComponent)
      {
         if (!int.TryParse(component, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedComponent))
            return false;

         if (parsedComponent >= 0)
            return true;

         return false;
      }

      public static bool operator ==(PunditVersion v1, PunditVersion v2)
      {
         if ((object)v1 == null)
            return (object)v2 == null;
         return v1.Equals(v2);
      }
      
      public static bool operator !=(PunditVersion v1, PunditVersion v2)
      {
         return !(v1 == v2);
      }
      
      public static bool operator <(PunditVersion v1, PunditVersion v2)
      {
         if ((object)v1 == null)
            throw new ArgumentNullException(nameof(v1));
         return v1.CompareTo(v2) < 0;
      }
      public static bool operator <=(PunditVersion v1, PunditVersion v2)
      {
         if ((object)v1 == null)
            throw new ArgumentNullException(nameof(v1));
         return v1.CompareTo(v2) <= 0;
      }

      public static bool operator >(PunditVersion v1, PunditVersion v2)
      {
         return v2 < v1;
      }
      
      public static bool operator >=(PunditVersion v1, PunditVersion v2)
      {
         return v2 <= v1;
      }
   }
}