using System;
using System.Linq;
using NuGet.Versioning;

namespace EBerzosa.Pundit.Core
{
   internal static class VersionUtils
   {
      public const string DevMarker = "dev";

      public static string MakeFloatVersionString(string version)
      {
         return version.Split('.').Length < 4 ? version + ".*" : version;
      }

      public static bool IsDeveloper(this NuGetVersion version)
      {
         return version.Release == DevMarker;
      }

      public static void MakeDeveloper(ref NuGetVersion version)
      {
         version = new NuGetVersion(version.Version, DevMarker);
      }

      public static string ToPunditSearchVersion(NuGetVersion version)
      {
         return version.Major + "." + version.Minor + "." + version.Patch + "-" + (version.Release ?? "") + version.Revision;
      }
      
      public static string ToPunditSearchVersion(VersionRange version, bool isDeveloper)
      {
         if (!version.IsFloating && version.OriginalString.Split('.').Length == 4)
            return version.MinVersion.Major + "." + version.MinVersion.Minor + "." 
               + version.MinVersion.Patch + AppendRevision(isDeveloper, version.MinVersion.Revision.ToString());

         if (!version.IsFloating)
            throw new NotSupportedException("Non floating versions are not supported yet.");

         if (new[] {NuGetVersionFloatBehavior.None, NuGetVersionFloatBehavior.AbsoluteLatest, NuGetVersionFloatBehavior.Prerelease }.Contains(version.Float.FloatBehavior))
            throw new NotSupportedException($"Float behavior '{version.Float.FloatBehavior}' is not supported");

         if (version.Float.FloatBehavior == NuGetVersionFloatBehavior.Major)
            return "*" + AppendRevision(isDeveloper, "*");

         if (version.Float.FloatBehavior == NuGetVersionFloatBehavior.Minor)
            return version.Float.MinVersion.Major + ".*" + AppendRevision(isDeveloper, "*");

         if (version.Float.FloatBehavior == NuGetVersionFloatBehavior.Patch)
            return version.Float.MinVersion.Major + "." + version.Float.MinVersion.Minor + ".*" + AppendRevision(isDeveloper, "*");

         if (version.Float.FloatBehavior == NuGetVersionFloatBehavior.Revision)
            return version.Float.MinVersion.Major + "." + version.Float.MinVersion.Minor + "." + version.Float.MinVersion.Patch + AppendRevision(isDeveloper, "*");
         
         throw new NotSupportedException();
      }

      private static string AppendRevision(bool isDeveloper, string defaultValue)
      {
         return "-" + (isDeveloper ? DevMarker : "") + defaultValue;
      }
   }
}
