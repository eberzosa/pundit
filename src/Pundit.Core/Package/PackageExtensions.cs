using System;
using System.Linq;
using System.Text.RegularExpressions;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Versioning;

namespace EBerzosa.Pundit.Core.Package
{
   internal static class PackageExtensions
   {
      private const string NoArchFrameworkShortName = "noarch";

      private const string PackageFileNamePattern = "{0}-{1}-{2}{3}"; // Id - Major.Minor.Patch-[ReleaseLabel]Revision - Platform .pundit
      private static readonly Regex PackageNameRgx = new Regex(@"^(.*)-(\d+)\.(\d+)\.(\d+)-([A-z0-9.]+)-(.*)" + PackageManifest.PackedExtension.Replace(".", "\\.") + "$");


      public static string GetFileName(this PackageManifest manifest)
      {
         return string.Format(PackageFileNamePattern,
               manifest.PackageId,
               ToPunditFileVersion(manifest.Version),
               manifest.Framework.GetPunditFrameworkFolderName(),
               PackageManifest.PackedExtension);
      }

      public static string GetRelatedSearchFileName(this PackageManifest manifest)
      {
         return string.Format(PackageFileNamePattern,
            manifest.PackageId,
            manifest.Version.Major + "." + manifest.Version.Minor + "." + manifest.Version.Patch + "-*",
            manifest.Framework.GetPunditFrameworkFolderName(),
            PackageManifest.PackedExtension);
      }

      public static string GetFileName(this PackageKey key)
      {
         return string.Format(PackageFileNamePattern,
            key.PackageId,
            ToPunditFileVersion(key.Version),
            key.Framework.GetPunditFrameworkFolderName(),
            PackageManifest.PackedExtension);
      }

      public static string GetSearchFileName(this UnresolvedPackage package)
      {
         return string.Format(PackageFileNamePattern,
            package.PackageId,
            ToPunditFileSearchVersion(package.AllowedVersions),
            package.Framework.GetPunditFrameworkFolderName(),
            PackageManifest.PackedExtension);
      }

      private static string ToPunditFileVersion(this NuGet.Versioning.NuGetVersion version)
      {
         return version.ReleaseLabels.Any()
            ? version.ToString()
            : version.Major + "." + version.Minor + "." + version.Patch + "-" + (version.Release ?? "") + version.Revision;
      }

      // TODO: Move this to antoher place
      public static PackageKey GetPackageKeyFromFileName(string fileName)
      {
         Match mtch = PackageNameRgx.Match(fileName);

         if (!mtch.Success)
            throw new ArgumentException($"Invalid package name ({fileName})", nameof(fileName));

         string packageId = mtch.Groups[1].Value;

         NuGet.Versioning.NuGetVersion version;
         if (int.TryParse(mtch.Groups[5].Value, out var rev))
         {
            version = new NuGet.Versioning.NuGetVersion(
               int.Parse(mtch.Groups[2].Value),
               int.Parse(mtch.Groups[3].Value),
               int.Parse(mtch.Groups[4].Value),
               rev);
         }
         else
         {
            version = new NuGet.Versioning.NuGetVersion(
               int.Parse(mtch.Groups[2].Value),
               int.Parse(mtch.Groups[3].Value),
               int.Parse(mtch.Groups[4].Value),
               mtch.Groups[5].Value);
         }

         return new PackageKey(packageId, version, GetFramework(mtch.Groups[6].Value));
      }

      private static string ToPunditFileSearchVersion(VersionRangeExtended range)
      {
         var versionParts = range.NuGetVersionRange.OriginalString.Split('-');
         var numberParts = versionParts[0].Split('.');

         if (numberParts.Length <= 3)
            return range.NuGetVersionRange.OriginalString;

         return string.Join(".", numberParts, 0, 3) + '-' + numberParts[3] + (versionParts.Length > 1 ? "-" + string.Join("-", versionParts) : "");
      }

      public static NuGet.Frameworks.NuGetFramework GetFramework(string frameworkString)
      {
         if (string.IsNullOrEmpty(frameworkString))
            return NuGet.Frameworks.NuGetFramework.AnyFramework;

         if (NoArchFrameworkShortName.Equals(frameworkString, StringComparison.OrdinalIgnoreCase))
            return NuGet.Frameworks.NuGetFramework.AnyFramework;

         return NuGet.Frameworks.NuGetFramework.Parse(frameworkString);
      }

      private static string GetPunditFrameworkFolderName(this NuGet.Frameworks.NuGetFramework framework)
      {
         return framework == NuGet.Frameworks.NuGetFramework.AnyFramework
            ? NoArchFrameworkShortName
            : framework.GetShortFolderName();
      }
   }
}
