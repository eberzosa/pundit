using System;
using System.Linq;
using System.Text.RegularExpressions;
using EBerzosa.Pundit.Core.Framework;
using EBerzosa.Pundit.Core.Mappings;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Versioning;

namespace EBerzosa.Pundit.Core.Package
{
   internal static class PackageExtensions
   {
      private const string PackageFileNamePattern = "{0}-{1}-{2}{3}"; // Id - Major.Minor.Patch-[ReleaseLabel]Revision - Platform .pundit
      private static readonly Regex PackageNameRgx = new Regex(@"^(.*)-(\d+)\.(\d+)\.(\d+)-([A-z0-9.]+)-(.*)" + PackageManifest.PackedExtension.Replace(".", "\\.") + "$");


      public static string GetFileName(this PackageManifest manifest, bool useLegacy)
      {
         return string.Format(PackageFileNamePattern,
            manifest.PackageId,
            ToPunditFileVersion(manifest.Version),
            manifest.Framework.GetShortFolderName(),
            PackageManifest.PackedExtension);
      }

      public static string GetRelatedSearchFileName(this PackageManifest manifest, bool useLegacy)
      {
         if (!useLegacy)
            throw new NotSupportedException();

         return string.Format(PackageFileNamePattern,
            manifest.PackageId,
            manifest.Version.Major + "." + manifest.Version.Minor + "." + manifest.Version.Patch + "-*",
            manifest.Framework.GetShortFolderName(),
            PackageManifest.PackedExtension);
      }

      public static string GetFileName(this PackageKey key, bool useLegacy)
      {
         if (!useLegacy)
            throw new NotSupportedException();

         return string.Format(PackageFileNamePattern,
            key.PackageId,
            ToPunditFileVersion(key.Version),
            key.Framework.GetShortFolderName(),
            PackageManifest.PackedExtension);
      }

      public static string GetSearchFileName(this UnresolvedPackage package, bool useLegacy)
      {
         if (!useLegacy)
            throw new NotSupportedException();

         return string.Format(PackageFileNamePattern,
            package.PackageId,
            ToPunditFileSearchVersion(package.AllowedVersions),
            package.Framework.GetShortFolderName(),
            PackageManifest.PackedExtension);
      }

      private static string ToPunditFileVersion(this PunditVersion version)
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

         PunditVersion version;
         if (int.TryParse(mtch.Groups[5].Value, out var rev))
         {
            version = new PunditVersion(
               int.Parse(mtch.Groups[2].Value),
               int.Parse(mtch.Groups[3].Value),
               int.Parse(mtch.Groups[4].Value),
               rev);
         }
         else
         {
            version = new PunditVersion(
               int.Parse(mtch.Groups[2].Value),
               int.Parse(mtch.Groups[3].Value),
               int.Parse(mtch.Groups[4].Value),
               mtch.Groups[5].Value);
         }

         return new PackageKey(packageId, version, PunditFramework.Parse(mtch.Groups[6].Value));
      }

      private static string ToPunditFileSearchVersion(FloatRange range)
      {

         var versionParts = range.OriginalVersion.Split('-');
         var numberParts = versionParts[0].Split('.');

         if (numberParts.Length <= 3)
            return range.OriginalVersion;

         return string.Join(".", numberParts, 0, 3) + '-' + numberParts[3] + (versionParts.Length > 1 ? "-" + string.Join("-", versionParts) : "");
      }
   }
}
