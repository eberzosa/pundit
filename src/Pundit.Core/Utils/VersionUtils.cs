﻿using System;
using EBerzosa.Pundit.Core.Versioning;

namespace EBerzosa.Pundit.Core.Utils
{
   internal static class VersionUtils
   {
      public static VersionRange GetVersionRangeFromPuntitDependencyVersion(string version)
      {
         if (version.Contains("*"))
            throw new NotSupportedException("Pundit versions cannot contain *");

         var parts = version.Split('.');

         if (parts.Length > 4 || parts.Length < 1)
            throw new NotSupportedException($"Version '{version}' is not supported");

         if (parts.Length == 4)
         {
            var floatVersion = FloatRange.Parse(version);
            return new VersionRange(floatVersion.MinVersion, floatVersion);
         }

         var indexOfRelease = version.IndexOf('-');

         if (indexOfRelease > -1)
            version = version.Substring(0, indexOfRelease - 1) + ".*" + version.Substring(indexOfRelease);
         else
            version += ".*";

         var floatVersion2 = FloatRange.Parse(string.Join(".", parts));
         return new VersionRange(floatVersion2.MinVersion, floatVersion2);
      }
   }
}
