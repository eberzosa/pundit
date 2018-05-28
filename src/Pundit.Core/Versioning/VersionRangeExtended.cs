using System;
using System.Collections.Generic;
using System.Linq;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.Core.Versioning
{
   public class VersionRangeExtended
   {
      public NuGet.Versioning.VersionRange NuGetVersionRange { get; }

      public string ReleaseLabel { get; set; }

      public bool HasReleaseLabel => ReleaseLabel != null;

      public IComparer<NuGet.Versioning.NuGetVersion> Comparer => new VersionComparerExtended(HasReleaseLabel ? VersionComparer.Pundit : VersionComparer.VersionRelease);

      public VersionRangeExtended(NuGet.Versioning.VersionRange range)
      {
         NuGetVersionRange = range;
      }

      public VersionRangeExtended(NuGet.Versioning.NuGetVersion minVersion)
      {
         NuGetVersionRange = new NuGet.Versioning.VersionRange(minVersion);
      }

      public VersionRangeExtended(NuGet.Versioning.NuGetVersion minVersion, NuGet.Versioning.NuGetVersion maxVersion, string originalString)
      {
         NuGetVersionRange = new NuGet.Versioning.VersionRange(minVersion, true, maxVersion, false, null, originalString);
      }

      /// <summary>
      /// True if the given version falls into the floating range.
      /// </summary>
      public bool Satisfies(NuGet.Versioning.NuGetVersion version)
      {
         Guard.NotNull(version, nameof(version));

         if (HasReleaseLabel)
         {
            if (NuGetVersionRange.MinVersion.IsPrerelease)
               throw new NotSupportedException($"Versions with release labels not supported when using on-demand release labels, '{NuGetVersionRange.MinVersion}'");

            if (NuGetVersionRange.MinVersion == NuGetVersionRange.MaxVersion)
               throw new NotSupportedException($"Versions with all numbers declares are not supported '{NuGetVersionRange.MinVersion}'");

            if (ReleaseLabel != null && version.IsPrerelease && !ReleaseLabel.Equals(version.ReleaseLabels.First(), StringComparison.OrdinalIgnoreCase))
               return false;
            
            return GetVersionRangeWithLabel(version.IsPrerelease).Satisfies(version, VersionComparer.Pundit);
         }

         NuGet.Versioning.VersionRange range;
         if (NuGetVersionRange.MaxVersion?.IsPrerelease == false && version.IsPrerelease)
            range = NuGetVersionRange.ReplaceMaxVersion(NuGetVersionRange.MaxVersion.Append("-"));
         else
            range = NuGetVersionRange;

         return range.Satisfies(version, VersionComparer.VersionRelease);
      }

      /// <summary>Return the version that best matches the range.</summary>
      public NuGet.Versioning.NuGetVersion FindBestMatch(IEnumerable<NuGet.Versioning.NuGetVersion> versions)
      {
         NuGet.Versioning.NuGetVersion current = null;
         if (versions == null)
            return null;

         foreach (var version in versions)
            if (IsBetter(current, version))
               current = version;

         return current;
      }

      /// <summary>
      /// Determines if a given version is better suited to the range than a current version.
      /// </summary>
      public bool IsBetter(NuGet.Versioning.NuGetVersion current, NuGet.Versioning.NuGetVersion considering)
      {
         if (ReferenceEquals(current, considering))
            return false;

         if (ReferenceEquals(considering, null))
            return false;

         if (!Satisfies(considering))
            return false;

         if (ReferenceEquals(current, null))
            return true;

         if (!HasReleaseLabel)
            return VersionComparer.Compare(current, considering, VersionComparison.VersionRelease) == -1;

         // We don't want to compare the revision number as ReleaseLabel versions do not use it as main version
         if (!considering.IsPrerelease && current.IsPrerelease)
            considering = considering.Append("-").RevisionToLabel();

         return VersionComparer.Compare(current.Prepend(ReleaseLabel), considering.Prepend(ReleaseLabel), VersionComparison.PunditVersion) == -1;
      }

      public override string ToString()
      {
         return NuGetVersionRange + (HasReleaseLabel ? " Release: " + ReleaseLabel : "");
      }

      private NuGet.Versioning.VersionRange GetVersionRangeWithLabel(bool appendMinimumPreReleaseToMaxVersion)
      {
         var maxVersion = appendMinimumPreReleaseToMaxVersion ? NuGetVersionRange.MaxVersion.Append("-") : NuGetVersionRange.MaxVersion;

         return new NuGet.Versioning.VersionRange(NuGetVersionRange.MinVersion.Prepend(ReleaseLabel).RevisionToLabel(), NuGetVersionRange.IsMinInclusive, 
            maxVersion, NuGetVersionRange.IsMaxInclusive);
      }
   }
}
