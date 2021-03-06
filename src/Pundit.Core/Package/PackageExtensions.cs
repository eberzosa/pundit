﻿using System;
using System.Text.RegularExpressions;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Versioning;

namespace EBerzosa.Pundit.Core.Package
{
   internal static class PackageExtensions
   {
      private const string PackageFileNamePattern = "{0}-{1}-{2}.pundit"; // Id - Major.Minor.Patch-Revision - Platform .pundit
      private static readonly Regex PackageNameRgx = new Regex(@"^(?<PackageId>.*?)-(?<Version>(?:\d+\.){2}(?:\d+-\d+))-(?<Framework>.*?)\.pundit$");


      public static string GetNewManifestFileName(this PackageManifest manifest)
      {
         return string.Format(PackageFileNamePattern,
               manifest.PackageId,
               ToPunditVersion(manifest.Version, false),
               "any");
      }

      public static string GetRelatedSearchFileName(this PackageKey key)
      {
         return string.Format(PackageFileNamePattern,
            key.PackageId,
            ToPunditVersion(key.Version, true),
            key.Framework);
      }

      public static string GetFileName(this PackageKey key)
      {
         return string.Format(PackageFileNamePattern,
            key.PackageId,
            ToPunditVersion(key.Version, false),
            key.Framework);
      }

      public static string GetNoFrameworkFileName(this PackageKey key)
      {
         return string.Format(PackageFileNamePattern,
            key.PackageId,
            ToPunditVersion(key.Version, false),
            "*");
      }

      public static string GetSearchFileName(this UnresolvedPackage package)
      {
         return string.Format(PackageFileNamePattern,
            package.PackageId,
            ToPunditFileSearchVersion(package.AllowedVersions),
            package.Framework ?? "*");
      }

      // TODO: Move this to antoher place
      public static PackageKey GetPackageKeyFromFileName(string fileName)
      {
         Match mtch = PackageNameRgx.Match(fileName);

         if (!mtch.Success)
            throw new ArgumentException($"Invalid package name ({fileName})", nameof(fileName));

         var packageId = mtch.Groups["PackageId"].Value;
         var version = NuGet.Versioning.NuGetVersion.Parse(mtch.Groups["Version"].Value.Replace('-', '.'));
         var framework = mtch.Groups["Framework"].Value;

         return new PackageKey(packageId, version, framework);
      }

      private static string ToPunditFileSearchVersion(VersionRangeExtended range)
      {
         return ToPunditFileSearchVersion(range.NuGetVersionRange);
      }

      private static string ToPunditFileSearchVersion(NuGet.Versioning.VersionRange range)
      {
         if (range.MaxVersion != null)
         {
            if (range.MinVersion.Major < range.MaxVersion.Major)
               return range.MinVersion.Major + ".*";

            if (range.MinVersion.Minor < range.MaxVersion.Minor)
               return range.MinVersion.Major + "." + range.MinVersion.Minor + ".*";

            if (range.MinVersion.Patch < range.MaxVersion.Patch)
               return range.MinVersion.Major + "." + range.MinVersion.Minor + "." + range.MinVersion.Patch + "-*";
         }

         return range.MinVersion.Major + "." + range.MinVersion.Minor + "." + range.MinVersion.Patch + "-" +
                range.MinVersion.Revision;
      }

      private static string ToPunditVersion(NuGet.Versioning.NuGetVersion version, bool searchableByRevision)
      {
         return version.Major + "." + version.Minor + "." + version.Patch + "-" + 
            (searchableByRevision ? "*" : version.Revision.ToString());
      }

      public static NuGet.Frameworks.NuGetFramework GetFramework(string frameworkString)
      {
         if (string.IsNullOrEmpty(frameworkString))
            return NuGet.Frameworks.NuGetFramework.AgnosticFramework;

         if ("noarch".Equals(frameworkString, StringComparison.OrdinalIgnoreCase))
            return NuGet.Frameworks.NuGetFramework.AgnosticFramework;

         var parsedFramework = NuGet.Frameworks.NuGetFramework.Parse(frameworkString);

         if (parsedFramework == NuGet.Frameworks.NuGetFramework.UnsupportedFramework)
            return new NuGet.Frameworks.NuGetFramework(frameworkString);

         return parsedFramework;
      }
   }
}
