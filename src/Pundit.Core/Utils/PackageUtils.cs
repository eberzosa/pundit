using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using EBerzosa.Pundit.Core;
using NuGet.Versioning;
using Pundit.Core.Model;

namespace Pundit.Core.Utils
{
   public static class PackageUtils
   {
      public const string PackageFileNamePattern = "{0}-{1}-{2}{3}";
      public const string NoArchName = "noarch";

      private const string DevMarker = "dev";

      private static readonly Regex PackageNameRgx = new Regex("^(.*)-(\\d+)\\.(\\d+)\\.(\\d+)-(" + DevMarker + "){0,1}(\\d+)-(.*)" + PackageManifest.PackedExtension.Replace(".", "\\.") + "$");

      public static string GetFileName(PackageManifest pkg)
      {
         pkg.Validate();

         return string.Format(PackageFileNamePattern,
            pkg.PackageId,
            VersionUtils.ToPunditSearchVersion(pkg.Version),
            TrimPlatformName(pkg.Platform),
            PackageManifest.PackedExtension);
      }

      public static string GetFileName(PackageKey key)
      {
         return string.Format(PackageFileNamePattern,
            key.PackageId,
            VersionUtils.ToPunditSearchVersion(key.Version),
            TrimPlatformName(key.Platform),
            PackageManifest.PackedExtension);
      }

      public static string GetSearchPattern(UnresolvedPackage pkg)
      {
         return string.Format(PackageFileNamePattern,
            pkg.PackageId,
            pkg.VersionPattern,
            TrimPlatformName(pkg.Platform),
            PackageManifest.PackedExtension);
      }

      public static string GetBuildsSearchFilePattern(PackageManifest pkg)
      {
         pkg.Validate();

         return string.Format(PackageFileNamePattern,
            pkg.PackageId,
            pkg.Version.Major, pkg.Version.Minor, pkg.Version.Patch,
            "*",
            "",
            string.IsNullOrEmpty(pkg.Platform) ? "noarch" : pkg.Platform,
            PackageManifest.PackedExtension);         
      }

      public static PackageKey GetPackageKeyFromFileName(string fileName)
      {
         Match mtch = PackageNameRgx.Match(fileName);

         if(!mtch.Success)
            throw new ArgumentException("Invalid package name (" + fileName + ")", "fileName");

         string packageId = mtch.Groups[1].Value;

         var v = mtch.Groups[5].Success
            ? new NuGetVersion(
               int.Parse(mtch.Groups[2].Value),
               int.Parse(mtch.Groups[3].Value),
               int.Parse(mtch.Groups[4].Value),
               int.Parse(mtch.Groups[6].Value),
               new[] {mtch.Groups[5].Value}, "")
            : new NuGetVersion(
               int.Parse(mtch.Groups[2].Value),
               int.Parse(mtch.Groups[3].Value),
               int.Parse(mtch.Groups[4].Value),
               int.Parse(mtch.Groups[6].Value));

         string platform = mtch.Groups[7].Value;

         return new PackageKey(packageId, v, platform);
      }

      public static string[] SearchAllRelatedBuilds(string sourceDirectory, PackageManifest pkg)
      {
         return Directory.GetFiles(sourceDirectory, GetBuildsSearchFilePattern(pkg));
      }

      public static string TrimPlatformName(string platform)
      {
         return string.IsNullOrEmpty(platform) ? NoArchName : platform;
      }

      public static bool ArePlatformsEqual(string platform1, string platform2)
      {
         return TrimPlatformName(platform1)
            .Equals(
               TrimPlatformName(platform2),
               StringComparison.CurrentCultureIgnoreCase);
      }

      public static Version GetProductVersion(Assembly asm)
      {
         FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
         string version = fvi.ProductVersion;
         return new Version(version);
      }
   }
}
