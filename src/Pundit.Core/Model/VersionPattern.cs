using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Pundit.Core.Model
{
   public class VersionPattern : IComparable<VersionPattern>
   {
      private static readonly Regex ValidationRgx = new Regex("^[0-9\\*]+(\\.[0-9\\*]+){0,3}$");
      private readonly Version _v;

      public VersionPattern(string pattern)
      {
         if(!ValidationRgx.IsMatch(pattern))
            throw new ArgumentException("Version pattern is not valid", "pattern");

         string[] sp = pattern.Split('.');

         int major = (sp.Length == 0 || sp[0] == "*") ? int.MaxValue : int.Parse(sp[0]);
         int minor = (sp.Length < 2 || sp[1] == "*") ? int.MaxValue : int.Parse(sp[1]);
         int build = (sp.Length < 3 || sp[2] == "*") ? int.MaxValue : int.Parse(sp[2]);
         int revision = (sp.Length < 4 || sp[3] == "*") ? int.MaxValue : int.Parse(sp[3]);

         _v = new Version(major, minor, build, revision);
      }

      public int CompareTo(VersionPattern other)
      {
         return _v.CompareTo(other._v);
      }

      public static bool operator ==(VersionPattern v1, VersionPattern v2)
      {
         return v1._v == v2._v;
      }

      public static bool operator !=(VersionPattern v1, VersionPattern v2)
      {
         return v1._v != v2._v;
      }

      public static bool operator <(VersionPattern v1, VersionPattern v2)
      {
         return v1._v < v2._v;
      }

      public static bool operator <=(VersionPattern v1, VersionPattern v2)
      {
         return v1._v <= v2._v;
      }

      public static bool operator >(VersionPattern v1, VersionPattern v2)
      {
         return v1._v > v2._v;
      }

      public static bool operator >=(VersionPattern v1, VersionPattern v2)
      {
         return v1._v >= v2._v;
      }

      public override bool Equals(object obj)
      {
         if(obj is VersionPattern)
         {
            VersionPattern that = (VersionPattern) obj;

            return that._v.Equals(_v);
         }

         return false;
      }

      public override int GetHashCode()
      {
         return _v.GetHashCode();
      }

      public static bool AreCompatible(VersionPattern p1, VersionPattern p2)
      {
         return p1._v.Major == p2._v.Major &&
                p1._v.Minor == p2._v.Minor;
      }
   }
}
