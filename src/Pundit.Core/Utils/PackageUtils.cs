using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Pundit.Core.Model;

namespace Pundit.Core.Utils
{
   public static class PackageUtils
   {
      public const string PackageFileNamePattern = "{0}-{1}.{2}.{3}-{4}-{5}{6}";
      public const string NoArchName = "noarch";
      private static Regex _packageNameRgx = new Regex("^(.*)-(\\d+)\\.(\\d+)\\.(\\d+)-(\\d+)-(.*)" +
         Package.PackedExtension.Replace(".", "\\.") + "$");

      public static string GetFileName(Package pkg)
      {
         pkg.Validate();

         return string.Format(PackageFileNamePattern,
            pkg.PackageId,
            pkg.Version.Major, pkg.Version.Minor, pkg.Version.Build,
            pkg.Version.Revision,
            TrimPlatformName(pkg.Platform),
            Package.PackedExtension);
      }

      public static string GetFileName(PackageKey key)
      {
         return string.Format(PackageFileNamePattern,
                              key.PackageId,
                              key.Version.Major, key.Version.Minor,
                              key.Version.Build, key.Version.Revision,
                              TrimPlatformName(key.Platform),
                              Package.PackedExtension);
      }

      public static string GetSearchPattern(UnresolvedPackage pkg, VersionPattern pattern)
      {
         Version v = pattern.ToVersion();

         return string.Format(PackageFileNamePattern,
                       pkg.PackageId,
                       v.Major, v.Minor,
                       v.Build == -1 ? "*" : v.Build.ToString(),
                       v.Revision == -1 ? "*" : v.Revision.ToString(),
                       TrimPlatformName(pkg.Platform),
                       Package.PackedExtension);
      }

      public static string GetBuildsSearchFilePattern(Package pkg)
      {
         pkg.Validate();

         return string.Format(PackageFileNamePattern,
            pkg.PackageId,
            pkg.Version.Major, pkg.Version.Minor, pkg.Version.Build,
            "*",
            string.IsNullOrEmpty(pkg.Platform) ? "noarch" : pkg.Platform,
            Package.PackedExtension);         
      }

      public static PackageKey GetPackageKeyFromFileName(string fileName)
      {
         Match mtch = _packageNameRgx.Match(fileName);

         if(!mtch.Success)
            throw new ArgumentException("Invalid package name (" + fileName + ")", "fileName");

         string packageId = mtch.Groups[1].Value;
         var v = new Version(
            int.Parse(mtch.Groups[2].Value),
            int.Parse(mtch.Groups[3].Value),
            int.Parse(mtch.Groups[4].Value),
            int.Parse(mtch.Groups[5].Value));
         string platform = mtch.Groups[6].Value;

         return new PackageKey(packageId, v, platform);
      }

      public static string[] SearchAllRelatedBuilds(string sourceDirectory, Package pkg)
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
