using System;
using EBerzosa.Pundit.Core.Versioning;

namespace EBerzosa.Pundit.Core.Converters
{
   internal static class VersionConverterExtensions
   {
      //public static NuGet.Versioning.NuGetVersion ToNuGetVersion(this PunditVersion version)
      //{
      //   return new NuGet.Versioning.NuGetVersion(version.Version, version.ReleaseLabels, version.Metadata, version.OriginalVersion);
      //}

      //public static PunditVersion ToPunditVersion(this NuGet.Versioning.NuGetVersion version)
      //{
      //   return new PunditVersion(version.Version, version.ReleaseLabels, version.Metadata, version.OriginalVersion);
      //}

      //public static NuGet.Versioning.FloatRange ToNuGetFloatRange(this FloatRange floatRange)
      //{
      //   return new NuGet.Versioning.FloatRange(
      //      floatRange.FloatBehaviour.ToNuGetFloatBehaviour(),
      //      floatRange.MinVersion.ToNuGetVersion(),
      //      floatRange.OriginalReleasePrefix);
      //}

      //public static FloatRange ToPunditFloatRange(this NuGet.Versioning.FloatRange floatRange)
      //{
      //   return new FloatRange(
      //      floatRange.FloatBehavior.ToPunditFloatBehaviour(),
      //      floatRange.MinVersion.ToPunditVersion(),
      //      floatRange.OriginalReleasePrefix);
      //}

      public static NuGet.Versioning.NuGetVersionFloatBehavior ToNuGetFloatBehaviour(this FloatBehaviour behaviour)
      {
         if (behaviour == FloatBehaviour.RevisionPrerelease ||
             behaviour == FloatBehaviour.PatchPrerelease ||
             behaviour == FloatBehaviour.MinorPrerelease ||
             behaviour == FloatBehaviour.MajorPrerelease)
         {
            throw new NotSupportedException();
         }

         return (NuGet.Versioning.NuGetVersionFloatBehavior)behaviour;
      }

      public static FloatBehaviour ToPunditFloatBehaviour(this NuGet.Versioning.NuGetVersionFloatBehavior behaviour)
      {
         return (FloatBehaviour)behaviour;
      }
      //public static FloatRange ToFloatRange(this PunditVersion version)
      //{
      //   return new FloatRange(FloatBehaviour.None, version);
      //}

      //public static NuGet.Versioning.VersionRange ToNuGetVersionRange(this FloatRange floatRange)
      //{
      //   NuGet.Versioning.NuGetVersion maxVersion = null;
      //   if (floatRange.FloatBehaviour == FloatBehaviour.Revision)
      //      maxVersion = new NuGet.Versioning.NuGetVersion(floatRange.MinVersion.Major, floatRange.MinVersion.Minor, floatRange.MinVersion.Patch + 1);

      //   else if (floatRange.FloatBehaviour == FloatBehaviour.Patch)
      //      maxVersion = new NuGet.Versioning.NuGetVersion(floatRange.MinVersion.Major, floatRange.MinVersion.Minor + 1, 0);

      //   else if (floatRange.FloatBehaviour == FloatBehaviour.Minor)
      //      maxVersion = new NuGet.Versioning.NuGetVersion(floatRange.MinVersion.Major + 1, 0, 0);

      //   else
      //      throw new NotSupportedException();

      //   return (maxVersion != null)
      //      ? new NuGet.Versioning.VersionRange(floatRange.MinVersion.ToNuGetVersion(), true, maxVersion, false)
      //      : new NuGet.Versioning.VersionRange(floatRange.MinVersion.ToNuGetVersion(), floatRange.ToNuGetFloatRange());
      //}

      //public static FloatRange ConvertPunditDependencyVersionToFloatVersion(string punditDependencyVersion)
      //{
      //   if (punditDependencyVersion.Contains("*"))
      //      throw new NotSupportedException("Pundit versions cannot contain *");

      //   var parts = punditDependencyVersion.Split('.');

      //   if (parts.Length > 4 || parts.Length < 1)
      //      throw new NotSupportedException($"Version '{punditDependencyVersion}' is not supported");

      //   if (punditDependencyVersion.IndexOf('-') != -1)
      //      throw new NotSupportedException("Versions cannot contain Release part");

      //   return parts.Length == 4
      //      ? FloatRange.Parse(punditDependencyVersion)
      //      : FloatRange.Parse(punditDependencyVersion + ".*");
      //}

      public static VersionRangeExtended ConvertPunditDependencyVersionToVersionRangeExtended(string punditDependencyVersion)
      {
         if (punditDependencyVersion.IndexOf('-') != -1)
            throw new NotSupportedException("Versions cannot contain Release part");
         
         var minVersion = NuGet.Versioning.NuGetVersion.Parse(punditDependencyVersion);

         var parts = punditDependencyVersion.Split('.');

         if (parts.Length == 4)
            return new VersionRangeExtended(minVersion);

         if (parts.Length == 3)
            return new VersionRangeExtended(minVersion, new NuGet.Versioning.NuGetVersion(minVersion.Major, minVersion.Minor, minVersion.Patch + 1), punditDependencyVersion);

         if (parts.Length == 2)
            return new VersionRangeExtended(minVersion, new NuGet.Versioning.NuGetVersion(minVersion.Major, minVersion.Minor + 1, 0), punditDependencyVersion);

         if (parts.Length == 1)
            return new VersionRangeExtended(minVersion, new NuGet.Versioning.NuGetVersion(minVersion.Major + 1, 0, 0), punditDependencyVersion);

         throw new NotSupportedException($"Version '{punditDependencyVersion}' is not supported");
      }
   }
}