using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Pundit.Core.Model
{
   public class VersionPattern
   {
      //at least two numbers
      private string _originalPattern;
      private static readonly Regex ValidationRgx = new Regex("^[0-9\\*]+(\\.[0-9\\*]+){1,3}$");
      private Version _v;

      public VersionPattern(string pattern)
      {
         if(!ValidationRgx.IsMatch(pattern))
            throw new ArgumentException("Version pattern is not valid", "pattern");

         _originalPattern = pattern;

         string[] sp = pattern.Split('.');

         int major = int.Parse(sp[0]);
         int minor = int.Parse(sp[1]);
         int build = (sp.Length < 3 || sp[2] == "*") ? -1 : int.Parse(sp[2]);
         int revision = (sp.Length < 4 || sp[3] == "*") ? -1 : int.Parse(sp[3]);

         string s = major + "." + minor;
         if (build != -1) s += ("." + build);
         if (revision != -1) s += ("." + revision);

         _v = new Version(s);
      }

      public bool Matches(Version v)
      {
         if (_v.Major != v.Major || _v.Minor != v.Minor)
            return false;

         if (_v.Build != -1 && (_v.Build != v.Build))
            return false;

         if (_v.Revision != -1 && (_v.Revision != v.Revision))
            return false;

         return true;
      }

      public override string ToString()
      {
         return _originalPattern;
      }
   }
}
