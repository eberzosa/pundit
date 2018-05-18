using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Versioning;

namespace Pundit.Core.Model
{
   public class PackageManifest : ICloneable
   {
      public const string DefaultManifestFileName = "pundit.xml"; //package definition
      public const string PackedExtension = ".pundit";

      private const string PackageStringDescr = "allowed characters: letters (A-Z, a-z), numbers, underscore (_) and dot sign (.)";

      private static readonly Regex PackageStringRgx = new Regex("^[0-9a-zA-Z\\._]+$");
      private static readonly Regex PackageVersionRgx = new Regex("^[0-9\\*]+(\\.[0-9\\*]+){3}(-dev){0,1}$");
      

      private List<PackageDependency> _dependencies = new List<PackageDependency>();

      public string CoreVersion { get; internal set; }

      //WARNING!!! remember to reflect copy constructor if adding a new property to this class

      public string PackageId { get; set; }

      public string Platform { get; set; }

      public string ProjectUrl { get; set; }

      public PunditVersion Version { get; set; }

      public string Author { get; set; }

      public string Description { get; set; }

      public string ReleaseNotes { get; set; }

      public string License { get; set; }

      public List<PackageDependency> Dependencies
      {
         get => _dependencies;
         set => _dependencies = new List<PackageDependency>(value);
      }

      public PackageManifest()
      {
         CoreVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
      }

      /// <summary>
      /// Crates an instance of a package copying it from the source package
      /// </summary>
      /// <param name="copy">The package to copy from. Must be valid,
      /// otherwise <see cref="InvalidPackageException"/> is thrown</param>
      public PackageManifest(PackageManifest copy)
         : this()
      {
         copy.Validate();
         
         _dependencies = new List<PackageDependency>(copy._dependencies.Where(pd => pd.Scope == DependencyScope.Normal));

         PackageId = copy.PackageId;
         Platform = copy.Platform;
         ProjectUrl = copy.ProjectUrl;
         Version = copy.Version;
         Author = copy.Author;
         Description = copy.Description;
         ReleaseNotes = copy.ReleaseNotes;
         License = copy.License;
      }
      
      private bool IsValidPackageNameString(string s)
      {
         return PackageStringRgx.IsMatch(s);
      }

      /// <summary>
      /// Validates the package. In case of invalid package throws <see cref="InvalidPackageException"/>
      /// </summary>
      public virtual void Validate()
      {
         var ex = new InvalidPackageException();

         if(string.IsNullOrEmpty(PackageId))
            ex.AddError("PackageId", "package id is required");

         if(!IsValidPackageNameString(PackageId))
            ex.AddError("PackageId", "package id is invalid, " + PackageStringDescr);

         if(!string.IsNullOrEmpty(Platform) && !IsValidPackageNameString(Platform))
            ex.AddError("Platform", "platform name is invalid, " + PackageStringDescr);

         if(Version == null)
            ex.AddError("Version", "version is required");

         if (ex.HasErrors)
            throw ex;
      }

      public object Clone()
      {
         return new PackageManifest(this);
      }

      public PackageDependency GetPackageDependency(PackageKey key)
      {
         return Dependencies?.Where(d => d.PackageId == key.PackageId && d.Platform == key.Platform).FirstOrDefault();
      }

      public override string ToString() => PackageId + " [" + Version + "] [" + Platform + "]";
   }
}
