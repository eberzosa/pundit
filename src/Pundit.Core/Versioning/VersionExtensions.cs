using System;
using System.Linq;
using NuGet.Versioning;

namespace EBerzosa.Pundit.Core.Versioning
{
   internal static class VersionExtensions
   {
      public static VersionRange GetVersionRangeFromDependency(string punditDependencyVersion)
      {
         var minVersion = NuGetVersion.Parse(punditDependencyVersion);

         var parts = punditDependencyVersion.Split('.');

         if (parts.Length > 0 && parts[0].Contains('-') || parts.Length > 1 && parts[1].Contains('-'))
            throw new NotSupportedException("Versions with release labels can only contain 3 digits x.y.z-ReleaseLabel.[Build]");

         if (parts.Length >= 4)
         {
            var dashInPacth = parts[2].Contains('-');
            var dashInRevision = parts[3].Contains('-');

            if (!dashInPacth && dashInRevision)
               throw new NotSupportedException("Versions with release labels can only contain 3 digits x.y.z-ReleaseLabel.[Build]");

            if (parts.Length > 4 && !dashInPacth)
               throw new NotSupportedException($"Version '{punditDependencyVersion}' is not supported");

            if (!dashInPacth)
               return new VersionRange(minVersion, true, minVersion, true, null, punditDependencyVersion);
         }

         if (parts.Length >= 3) // 4+ will enter here if came back from previous
         {
            if (minVersion.ReleaseLabels.Any())
            {
               var version = minVersion.RevisionToLabel();
               return new VersionRange(version, true, version, true, null, punditDependencyVersion);
            }

            return new VersionRange(minVersion, true, minVersion.Add(1, VersionPart.Patch), false, null, punditDependencyVersion);
         }

         if (parts.Length == 2)
            return new VersionRange(minVersion, true, minVersion.Add(1, VersionPart.Minor), false, null, punditDependencyVersion);

         if (parts.Length == 1)
            return new VersionRange(minVersion, true, minVersion.Add(1, VersionPart.Major), false, null, punditDependencyVersion);

         throw new NotSupportedException($"Version '{punditDependencyVersion}' is not supported");
      }

      public static NuGetVersion Add(this NuGetVersion version, int value, VersionPart versionPart)
      {
         return new NuGetVersion(
            version.Major + (versionPart == VersionPart.Major ? value : 0),
            version.Minor + (versionPart == VersionPart.Minor ? value : 0),
            version.Patch + (versionPart == VersionPart.Patch ? value : 0),
            version.Revision + (versionPart == VersionPart.Revision ? value : 0),
            version.ReleaseLabels,
            version.Metadata);
      }

      public static NuGetVersion RemoveRevision(this NuGetVersion version)
      {
         return new NuGetVersion(version.Major, version.Minor, version.Patch, 0, version.ReleaseLabels, version.Metadata);
      }

      public static NuGetVersion Append(this NuGetVersion version, string releaseLabel)
      {
         return new NuGetVersion(version.Major, version.Minor, version.Patch, version.Revision, 
            version.ReleaseLabels.Concat(new[] {releaseLabel}), version.Metadata);
      }

      public static NuGetVersion Prepend(this NuGetVersion version, string releaseLabel)
      {
         return new NuGetVersion(version.Major, version.Minor, version.Patch, version.Revision,
            new[] {releaseLabel}.Concat(version.ReleaseLabels), version.Metadata);
      }

      public static NuGetVersion RevisionToLabel(this NuGetVersion version)
      {
         if (version.Revision == 0)
            return version;

         return new NuGetVersion(version.Major, version.Minor, version.Patch, 0, 
            version.ReleaseLabels.Concat(new[] {version.Revision.ToString()}), version.Metadata);
      }
      
      public static VersionRange ReplaceMaxVersion(this VersionRange range, NuGetVersion newMaxVersion)
      {
         return new VersionRange(range.MinVersion, range.IsMinInclusive, newMaxVersion, range.IsMaxInclusive, range.Float, range.OriginalString);
      }
   }
}