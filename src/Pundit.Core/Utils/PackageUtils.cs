using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pundit.Core.Model;

namespace Pundit.Core.Utils
{
   public static class PackageUtils
   {
      public const string PackageFileNamePattern = "{0}-{1}.{2}.{3}-{4}-{5}{6}";

      public static string GetFileName(Package pkg)
      {
         pkg.Validate();

         return string.Format(PackageFileNamePattern,
            pkg.PackageId,
            pkg.Version.Major, pkg.Version.Minor, pkg.Version.Build,
            pkg.Version.Revision,
            string.IsNullOrEmpty(pkg.Platform) ? "noarch" : pkg.Platform,
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

      public static string[] SearchAllRelatedBuilds(string sourceDirectory, Package pkg)
      {
         return Directory.GetFiles(sourceDirectory, GetBuildsSearchFilePattern(pkg));
      }
   }
}
