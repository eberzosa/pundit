using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EBerzosa.Pundit.Core.Model.Package
{
   [DebuggerDisplay("{PackageId} [{Version.ToString()}] [{Framework?.GetShortFolderName()}]")]
   public class PackageManifest
   {
      public const string DefaultManifestFileName = "pundit.xml"; //package definition
      public const string PackedExtension = ".pundit";

      private const string PackageStringDescr = "allowed characters: letters (A-Z, a-z), numbers, underscore (_) and dot sign (.)";

      private static readonly Lazy<string> CoreVersionValue = new Lazy<string>(() => Assembly.GetExecutingAssembly().GetName().Version.ToString());

      private static readonly Regex PackageStringRgx = new Regex("^[0-9a-zA-Z\\._]+$");


      private List<PackageDependency> _dependencies = new List<PackageDependency>();

      public string CoreVersion { get; internal set; }

      
      public string PackageId { get; set; }

      public NuGet.Versioning.NuGetVersion Version { get; set; }

      public NuGet.Frameworks.NuGetFramework Framework { get; set; }

      public string ProjectUrl { get; set; }

      public string Author { get; set; }

      public string Description { get; set; }

      public string ReleaseNotes { get; set; }

      public string License { get; set; }

      public List<PackageDependency> Dependencies
      {
         get => _dependencies;
         set => _dependencies = new List<PackageDependency>(value);
      }


      public PackageManifest() => CoreVersion = CoreVersionValue.Value;

      
      /// <summary>
      /// Validates the package. In case of invalid package throws <see cref="InvalidPackageException"/>
      /// </summary>
      public virtual void Validate()
      {
         var ex = new InvalidPackageException();

         if (string.IsNullOrEmpty(PackageId))
            ex.AddError("PackageId", "PackageId is missing");

         if (!IsValidPackageNameString(PackageId))
            ex.AddError("PackageId", "PackageId is invalid, " + PackageStringDescr);

         if (Version == null)
            ex.AddError("Version", "Version is missing");

         if (ex.HasErrors)
            throw ex;
      }
      

      public PackageDependency GetPackageDependency(PackageKey key) 
         => Dependencies?.Where(d => d.PackageId == key.PackageId && d.Framework == key.Framework).FirstOrDefault();

      public override string ToString() => PackageId + " [" + Version + "] [" + Framework + "]";

      private bool IsValidPackageNameString(string s) => PackageStringRgx.IsMatch(s);
   }
}