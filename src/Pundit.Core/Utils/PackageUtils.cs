using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using EBerzosa.Pundit.Core.Model;
using Pundit.Core.Model;

namespace Pundit.Core.Utils
{
   public static class PackageUtils
   {
      public const string PackageFileNamePattern = "{0}-{1}.{2}.{3}-{4}{5}-{6}{7}";
      public const string NoArchName = "noarch";

      private const string DevMarker = "dev";

      private static readonly Regex PackageNameRgx = new Regex("^(.*)-(\\d+)\\.(\\d+)\\.(\\d+)-(" + DevMarker + "){0,1}(\\d+)-(net.*)" + PackageManifest.PackedExtension.Replace(".", "\\.") + "$");

      public static string GetFileName(PackageManifest pkg)
      {
         pkg.Validate();

         return string.Format(PackageFileNamePattern,
            pkg.PackageId,
            pkg.Version.Major, 
            pkg.Version.Minor, 
            pkg.Version.Build, 
            pkg.Version.IsDeveloper ? DevMarker : "", 
            pkg.Version.Revision,
            TrimPlatformName(pkg.Platform),
            PackageManifest.PackedExtension);
      }

      public static string GetFileName(PackageKey key)
      {
         return string.Format(PackageFileNamePattern,
                              key.PackageId,
                              key.Version.Major, 
                              key.Version.Minor,
                              key.Version.Build, 
                              key.Version.IsDeveloper ? DevMarker : "",
                              key.Version.Revision,
                              TrimPlatformName(key.Platform),
                              PackageManifest.PackedExtension);
      }

      public static string GetSearchPattern(UnresolvedPackage pkg, VersionPattern pattern)
      {
         Version v = pattern.ToVersion();
         
         return string.Format(PackageFileNamePattern,
                       pkg.PackageId,
                       v.Major, v.Minor,
                       v.Build == -1 ? "*" : v.Build.ToString(),
                       "*",
                       v.Revision == -1 ? "" : v.Revision.ToString(),
                       TrimPlatformName(pkg.Platform),
                       PackageManifest.PackedExtension);
      }

      public static string GetBuildsSearchFilePattern(PackageManifest pkg)
      {
         pkg.Validate();

         return string.Format(PackageFileNamePattern,
            pkg.PackageId,
            pkg.Version.Major, pkg.Version.Minor, pkg.Version.Build,
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

         var v = new PunditVersion(
            int.Parse(mtch.Groups[2].Value),
            int.Parse(mtch.Groups[3].Value),
            int.Parse(mtch.Groups[4].Value),
            int.Parse(mtch.Groups[6].Value),
            mtch.Groups[5].Success);
         
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
