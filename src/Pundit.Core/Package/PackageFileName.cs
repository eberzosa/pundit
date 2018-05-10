using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NuGet.Versioning;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Package
{
   internal class PackageFileName
   {
      public const string PackageFileNamePattern = "{0}-{1}-{2}{3}"; // Id - Major.Minor.Patch-Revision[-Release] - Platform .pundit
      public const string NoArchName = "noarch";

      private static readonly Regex PackageNameRgx = new Regex(@"^(.*)-(\d+)\.(\d+)\.(\d+)-(\d+)-(.*)" + PackageManifest.PackedExtension.Replace(".", "\\.") + "$");

      private readonly string _fileName;
      private readonly string _searchFileName;
      private readonly string _relatedSearchFileName;

      public string FileName => NotNullOrThrow(_fileName, nameof(FileName));

      public string SearchFileName => NotNullOrThrow(_searchFileName, nameof(SearchFileName));

      public string RelatedSearchFileName => NotNullOrThrow(_relatedSearchFileName, nameof(RelatedSearchFileName));


      public PackageFileName(PackageManifest pkg)
      {
         pkg.Validate();

         _fileName = string.Format(PackageFileNamePattern,
            pkg.PackageId,
            ToPunditFileVersion(pkg.Version),
            TrimPlatformName(pkg.Platform),
            PackageManifest.PackedExtension);

         _relatedSearchFileName = string.Format(PackageFileNamePattern,
            pkg.PackageId,
            pkg.Version.Major + "." + pkg.Version.Minor + "." + pkg.Version.Patch + "-*",
            TrimPlatformName(pkg.Platform),
            PackageManifest.PackedExtension);
      }

      public PackageFileName(PackageKey key)
      {
         _fileName = string.Format(PackageFileNamePattern,
            key.PackageId,
            ToPunditFileVersion(key.Version),
            TrimPlatformName(key.Platform),
            PackageManifest.PackedExtension);
      }

      public PackageFileName(UnresolvedPackage pkg)
      {
         _searchFileName = string.Format(PackageFileNamePattern,
            pkg.PackageId,
            ToPunditFileSearchVersion(pkg.VersionPattern),
            TrimPlatformName(pkg.Platform),
            PackageManifest.PackedExtension);
      }

      public static string TrimPlatformName(string platform) 
         => string.IsNullOrEmpty(platform) ? NoArchName : platform;

      public static PackageKey GetPackageKeyFromFileName(string fileName)
      {
         Match mtch = PackageNameRgx.Match(fileName);

         if (!mtch.Success)
            throw new ArgumentException($"Invalid package name ({fileName})", nameof(fileName));

         string packageId = mtch.Groups[1].Value;

         var version = new NuGetVersion(
            int.Parse(mtch.Groups[2].Value),
            int.Parse(mtch.Groups[3].Value),
            int.Parse(mtch.Groups[4].Value),
            int.Parse(mtch.Groups[5].Value));

         string platform = mtch.Groups[6].Value;

         return new PackageKey(packageId, version, platform);
      }

      private string ToPunditFileVersion(NuGetVersion version) 
         => version.Major + "." + version.Minor + "." + version.Patch + "-" + (version.Release ?? "") + version.Revision;

      private string ToPunditFileSearchVersion(VersionRange range) 
         => range.IsFloating ? FromFloating(range.Float) : FromRegular(range);
      
      private string NotNullOrThrow(string value, string name) 
         => value ?? throw new InvalidOperationException($"The current status does not support '{name}'");

      private string FromFloating(FloatRange range)
      {
         if (range.FloatBehavior != NuGetVersionFloatBehavior.Revision)
            return range.ToString();

         var version = range.ToString();
         var index = version.LastIndexOf('.');

         return version.Substring(0, index) + '-' + version.Substring(index + 1);
      }

      private string FromRegular(VersionRange range)
      {
         // If there is no max version we only search for 'that' version in Pundit repos
         if (range.MaxVersion == null)
            return range.MinVersion.OriginalVersion;

         if (range.MinVersion == range.MaxVersion)
            return range.MinVersion.OriginalVersion;

         if (range.MinVersion.Major != range.MaxVersion.Major)
            return range.MinVersion.Major + ".*";

         if (range.MinVersion.Minor != range.MaxVersion.Minor)
            return range.MinVersion.Major + "." + range.MinVersion.Minor + ".*";

         if (range.MinVersion.Patch != range.MaxVersion.Patch)
            return range.MinVersion.Major + "." + range.MinVersion.Minor + "." + range.MinVersion.Patch + "-*";

         throw new NotSupportedException($"VersionRange  '{range}' is not supported");
      }
   }
}
