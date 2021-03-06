﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;

namespace EBerzosa.Pundit.Core.Versioning
{
   /// <summary>
   /// An IVersionComparer for NuGetVersion and NuGetVersion types.
   /// </summary>
   public class VersionComparer : NuGet.Versioning.IVersionComparer
   {
      private readonly VersionComparison _mode;

      /// <summary>
      /// Creates a VersionComparer using the default mode.
      /// </summary>
      public VersionComparer() => _mode = VersionComparison.Default;

      /// <summary>
      /// Creates a VersionComparer that respects the given comparison mode.
      /// </summary>
      /// <param name="versionComparison">comparison mode</param>
      public VersionComparer(VersionComparison versionComparison) => _mode = versionComparison;


      /// <summary>
      /// A default comparer that compares metadata as strings.
      /// </summary>
      public static readonly NuGet.Versioning.IVersionComparer Default = new VersionComparer(VersionComparison.Default);

      /// <summary>
      /// A comparer that uses only the version numbers.
      /// </summary>
      public static readonly NuGet.Versioning.IVersionComparer Version = new VersionComparer(VersionComparison.Version);

      /// <summary>
      /// Compares versions without comparing the metadata.
      /// </summary>
      public static readonly NuGet.Versioning.IVersionComparer VersionRelease = new VersionComparer(VersionComparison.VersionRelease);

      /// <summary>
      /// Compares versions as Version + Revision with special handling for Releases.
      /// </summary>
      public static readonly NuGet.Versioning.IVersionComparer Pundit = new VersionComparer(VersionComparison.PunditVersion);

      /// <summary>
      /// A version comparer that follows SemVer 2.0.0 rules.
      /// </summary>
      public static NuGet.Versioning.IVersionComparer VersionReleaseMetadata = new VersionComparer(VersionComparison.VersionReleaseMetadata);

      /// <summary>
      /// Compares the given versions using the VersionComparison mode.
      /// </summary>
      public static int Compare(NuGet.Versioning.SemanticVersion version1, NuGet.Versioning.SemanticVersion version2, VersionComparison versionComparison) 
         => new VersionComparer(versionComparison).Compare(version1, version2);


      /// <summary>
      /// Determines if both versions are equal.
      /// </summary>
      public bool Equals(NuGet.Versioning.SemanticVersion x, NuGet.Versioning.SemanticVersion y)
      {
         if (ReferenceEquals(x, y))
            return true;

         if (ReferenceEquals(y, null) || ReferenceEquals(x, null))
            return false;

         if (_mode == VersionComparison.Default || _mode == VersionComparison.VersionRelease)
         {
            return x.Major == y.Major && x.Minor == y.Minor && x.Patch == y.Patch &&
                   GetRevisionOrZero(x) == GetRevisionOrZero(y) && AreReleaseLabelsEqual(x, y);
         }

         // Use the full comparer for non-default scenarios
         return Compare(x, y) == 0;
      }


      /// <summary>
      /// Gives a hash code based on the normalized version string.
      /// </summary>
      public int GetHashCode(NuGet.Versioning.SemanticVersion version)
      {
         if (ReferenceEquals(version, null))
            return 0;

         var combiner = new HashCodeCombiner();

         combiner.AddObject(version.Major);
         combiner.AddObject(version.Minor);
         combiner.AddObject(version.Patch);

         var nuGetVersion = version as NuGet.Versioning.NuGetVersion;

         if (nuGetVersion != null && nuGetVersion.Revision > 0)
            combiner.AddObject(nuGetVersion.Revision);

         if (_mode == VersionComparison.Default || _mode == VersionComparison.VersionRelease || _mode == VersionComparison.VersionReleaseMetadata)
         {
            var labels = GetReleaseLabelsOrNull(version);

            if (labels != null)
            {
               var comparer = StringComparer.OrdinalIgnoreCase;
               foreach (var label in labels)
                  combiner.AddObject(label, comparer);
            }
         }

         if (_mode == VersionComparison.VersionReleaseMetadata && version.HasMetadata)
            combiner.AddObject(version.Metadata, StringComparer.OrdinalIgnoreCase);

         return combiner.CombinedHash;
      }

      /// <summary>
      /// Compare versions.
      /// </summary>
      public int Compare(NuGet.Versioning.SemanticVersion x, NuGet.Versioning.SemanticVersion y)
      {
         var result = CompareSemVersion(x, y);
         if (result != 0)
            return result;

         var legacyX = x as NuGet.Versioning.NuGetVersion;
         var legacyY = y as NuGet.Versioning.NuGetVersion;

         result = CompareLegacyVersion(legacyX, legacyY);
         if (result != 0)
            return result;

         if (_mode == VersionComparison.Version)
            return 0;

         if (_mode == VersionComparison.PunditVersion)
         {
            legacyX = GetFinalVersion(legacyX);
            legacyY = GetFinalVersion(legacyY);
         }

         // compare release labels
         var xLabels = GetReleaseLabelsOrNull(legacyX ?? x);
         var yLabels = GetReleaseLabelsOrNull(legacyY ?? y);

         if (xLabels != null && yLabels == null)
            return -1;

         if (xLabels == null && yLabels != null)
            return 1;

         if (xLabels != null && yLabels != null)
         {
            result = CompareReleaseLabels(xLabels, yLabels, 
               legacyX.IsLegacyVersion ? legacyX.Revision : (int?)null, legacyY.IsLegacyVersion ? legacyY.Revision : (int?)null);

            if (result != 0)
               return result;
         }

         if (_mode == VersionComparison.PunditVersion)
         {
            result = CompareLegacyVersion(legacyX, legacyY);
            if (result != 0)
               return result;
         }

         // compare the metadata
         if (_mode == VersionComparison.VersionReleaseMetadata)
         {
            result = StringComparer.OrdinalIgnoreCase.Compare(x.Metadata ?? string.Empty, y.Metadata ?? string.Empty);
            if (result != 0)
               return result;
         }

         return 0;
      }

      public static int CompareSemVersion(NuGet.Versioning.SemanticVersion x, NuGet.Versioning.SemanticVersion y)
      {
         if (ReferenceEquals(x, y))
            return 0;

         if (ReferenceEquals(x, null))
            return -1;

         if (ReferenceEquals(y, null))
            return 1;

         // compare version
         var result = x.Major.CompareTo(y.Major);
         if (result != 0)
            return result;

         result = x.Minor.CompareTo(y.Minor);
         if (result != 0)
            return result;

         result = x.Patch.CompareTo(y.Patch);
         if (result != 0)
            return result;

         return 0;
      }

      /// <summary>
      /// Compares the 4th digit of the version number.
      /// </summary>
      private static int CompareLegacyVersion(NuGet.Versioning.NuGetVersion legacyX, NuGet.Versioning.NuGetVersion legacyY)
      {
         // true if one has a 4th version number
         if (legacyX != null && legacyY != null)
            return legacyX.Version.CompareTo(legacyY.Version);

         if (legacyX != null && legacyX.Version.Revision > 0)
            return 1;

         if (legacyY != null && legacyY.Version.Revision > 0)
            return -1;

         return 0;
      }

      private static NuGet.Versioning.NuGetVersion GetFinalVersion(NuGet.Versioning.NuGetVersion version)
      {
         if (version == null)
            return null;

         var last = version.ReleaseLabels != null && version.ReleaseLabels.Any()
            ? version.ReleaseLabels.Last()
            : null;

         if (last == null || !int.TryParse(last, out var revision))
            return new NuGet.Versioning.NuGetVersion(version.Major, version.Minor, version.Patch, 0, version.ReleaseLabels, version.Metadata);

         var labels = version.ReleaseLabels.Take(version.ReleaseLabels.Count() - 1);
         return new NuGet.Versioning.NuGetVersion(version.Major, version.Minor, version.Patch, revision, labels, version.Metadata);
      }

      /// <summary>
      /// Compares sets of release labels.
      /// </summary>
      private static int CompareReleaseLabels(string[] version1, string[] version2, int? version1Revision, int? version2Revision)
      {
         var result = 0;
         
         var count = Math.Max(version1.Length, version2.Length);
         
         for (var i = 0; i < count; i++)
         {
            var aExists = i < version1.Length;
            var bExists = i < version2.Length;

            if (!aExists && bExists)
               return -1;

            if (aExists && !bExists)
               return 1;

            // compare the labels
            result = CompareRelease(version1[i], version2[i]);

            if (result != 0)
               return result;
         }

         return result;
      }

      /// <summary>
      /// Release labels are compared as numbers if they are numeric, otherwise they will be compared
      /// as strings.
      /// </summary>
      private static int CompareRelease(string version1, string version2)
      {
         // check if the identifiers are numeric
         var v1IsNumeric = int.TryParse(version1, out var version1Num);
         var v2IsNumeric = int.TryParse(version2, out var version2Num);
         
         // if both are numeric compare them as numbers
         if (v1IsNumeric && v2IsNumeric)
            return version1Num.CompareTo(version2Num);

         // numeric labels come before alpha labels
         if (v1IsNumeric || v2IsNumeric)
            return v1IsNumeric ? -1 : 1;

         // Ignoring 2.0.0 case sensitive compare. Everything will be compared case insensitively as 2.0.1 specifies.
         return StringComparer.OrdinalIgnoreCase.Compare(version1, version2);
      }

      /// <summary>
      /// Returns an array of release labels from the version, or null.
      /// </summary>
      private static string[] GetReleaseLabelsOrNull(NuGet.Versioning.SemanticVersion version)
      {
         // Check if labels exist
         if (!version.IsPrerelease)
            return null;

         // Try to use string[] which is how labels are normally stored.
         var labels = version.ReleaseLabels as string[];

         if (labels == null && version.ReleaseLabels != null)
         {
            // This is not the expected type, enumerate and convert to an array.
            labels = version.ReleaseLabels.ToArray();
         }

         return labels;
      }

      /// <summary>
      /// Compare release labels
      /// </summary>
      private static bool AreReleaseLabelsEqual(NuGet.Versioning.SemanticVersion x, NuGet.Versioning.SemanticVersion y)
      {
         var xLabels = GetReleaseLabelsOrNull(x);
         var yLabels = GetReleaseLabelsOrNull(y);

         if (xLabels == null && yLabels != null)
            return false;

         if (xLabels != null && yLabels == null)
            return false;

         if (xLabels == null || yLabels == null)
            return true;

         // Both versions must have the same number of labels to be equal
         if (xLabels.Length != yLabels.Length)
            return false;

         // Check if the labels are the same
         for (var i = 0; i < xLabels.Length; i++)
            if (!StringComparer.OrdinalIgnoreCase.Equals(xLabels[i], yLabels[i]))
               return false;

         // labels are equal
         return true;
      }

      /// <summary>
      /// Returns the fourth version number or zero.
      /// </summary>
      private static int GetRevisionOrZero(NuGet.Versioning.SemanticVersion version)
      {
         var nugetVersion = version as NuGet.Versioning.NuGetVersion;
         return nugetVersion?.Revision ?? 0;
      }

   }
}
