using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   /// <summary>
   /// Pundit manifest
   /// </summary>
   [XmlRoot("package")]
   [XmlInclude(typeof(DevPackage))]
   public class Package : IEquatable<Package>, ICloneable
   {
      /// <summary>
      /// Default package manifest file name
      /// </summary>
      public const string DefaultManifestFileName = "pundit.xml"; //package definition

      /// <summary>
      /// MIME type of the package file (.pundit)
      /// </summary>
      public const string DefaultContentType = "application/octet-stream";

      /// <summary>
      /// Default extension for packages
      /// </summary>
      public const string PackedExtension = ".pundit";

      /// <summary>
      /// Platform name synonym for packages not targeting any platforms
      /// </summary>
      public const string NoArchPlatformName = "noarch";

      private static readonly Regex PackageStringRgx = new Regex("^[0-9a-zA-Z\\._]+$");
      private static readonly Regex PackageVersionRgx = new Regex("^[0-9\\*]+(\\.[0-9\\*]+){3}$");
      private const string PackageStringDescr = "allowed characters: letters (A-Z, a-z), numbers, underscore (_) and dot sign (.)";

      private string _platform;

      private List<PackageDependency> _dependencies = new List<PackageDependency>();

      //WARNING!!! remember to reflect copy constructor if adding a new property to this class

      /// <summary>
      /// Specifies if this package is stable
      /// </summary>
      //[XmlAttribute("stable")]
      //public bool IsStable { get; set; }

      /// <summary>
      /// Globally unique package ID
      /// </summary>
      [XmlElement("packageId")]
      public string PackageId { get; set; }

      /// <summary>
      /// Package platform
      /// </summary>
      [XmlElement("platform")]
      public string Platform
      {
         get { return _platform; }
         set { _platform = string.IsNullOrEmpty(value) ? NoArchPlatformName : value; }
      }

      /// <summary>
      /// Project URL (optional)
      /// </summary>
      [XmlElement("projectUrl")]
      public string ProjectUrl { get; set; }

      /// <summary>
      /// Package version (required)
      /// </summary>
      [XmlIgnore]
      public Version Version { get; set; }

      /// <summary>
      /// Package version formatted as string
      /// </summary>
      [XmlElement("version")]
      public string VersionString
      {
         get { return this.Version.ToString(); }
         set { this.Version = new Version(value); }
      }

      [XmlElement("author")]
      public string Author { get; set; }

      [XmlElement("description")]
      public string Description { get; set; }

      [XmlElement("releaseNotes")]
      public string ReleaseNotes { get; set; }

      /// <summary>
      /// License agreement (optional)
      /// </summary>
      [XmlElement("license")]
      public string License { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [XmlArray("dependencies")]
      [XmlArrayItem("package")]
      public List<PackageDependency> Dependencies
      {
         get { return _dependencies; }
         set { _dependencies = new List<PackageDependency>(value); }
      }

      /// <summary>
      /// Creates an instance of empty package (the state is invalid)
      /// </summary>
      public Package()
      {
         Platform = Platform;
      }

      /// <summary>
      /// Creates an instance of empty package (state is valid)
      /// </summary>
      /// <param name="packageId"></param>
      /// <param name="version"></param>
      /// <exception cref="ArgumentNullException"></exception>
      public Package(string packageId, Version version)
         : this()
      {
         if (packageId == null) throw new ArgumentNullException("packageId");
         if (version == null) throw new ArgumentNullException("version");

         PackageId = packageId;
         VersionString = version.ToString();
      }

      /// <summary>
      /// Crates an instance of a package copying it from the source package
      /// </summary>
      /// <param name="copy">The package to copy from. Must be valid,
      /// otherwise <see cref="InvalidPackageException"/> is thrown</param>
      /// <param name="includeDevTime"></param>
      public Package(Package copy, bool includeDevTime = false) : this()
      {
         copy.Validate();

         if(includeDevTime)
            _dependencies = new List<PackageDependency>(copy._dependencies);
         else
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

      /// <summary>
      /// Package unique key
      /// </summary>
      public PackageKey Key
      {
         get { return new PackageKey(PackageId, Version, Platform); }
      }

      /// <summary>
      /// Deserializes package from stream
      /// </summary>
      /// <param name="inputStream">Stream of xml document</param>
      /// <returns></returns>
      public static Package FromStreamXml(Stream inputStream)
      {
         using(var ms = new MemoryStream())
         {
            inputStream.CopyTo(ms);
            ms.Position = 0;

            try
            {
               return ms.FromXmlStream<Package>();
            }
            catch(InvalidOperationException)
            {
               ms.Position = 0;
               return ms.FromXmlStream<Package>(false);
            }
         }
      }

      /// <summary>
      /// Serializes package to a stream in XML format
      /// </summary>
      /// <param name="s"></param>
      public virtual void WriteTo(Stream s)
      {
         Validate();
         this.WriteXmlTo(s);
      }

      /// <summary>
      /// Serializes package metadata to a file in XML format
      /// </summary>
      /// <param name="destFileName"></param>
      /// <param name="createBackup"></param>
      public virtual void WriteTo(string destFileName, bool createBackup = true)
      {
         if (createBackup)
         {
            string backupName = destFileName + ".bak";

            if(File.Exists(backupName)) File.Delete(backupName);

            if(File.Exists(destFileName)) File.Move(destFileName, backupName);
         }

         if(File.Exists(destFileName)) File.Delete(destFileName);

         using(Stream s = File.Create(destFileName))
         {
            WriteTo(s);
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="s"></param>
      /// <returns></returns>
      public static bool IsValidPackageNameString(string s)
      {
         if (string.IsNullOrEmpty(s)) return false;

         return PackageStringRgx.IsMatch(s);
      }

      private bool IsValidPackageVersion(string s)
      {
         return PackageVersionRgx.IsMatch(s);
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

         if(Version != null && !IsValidPackageVersion(Version.ToString(4)))
            ex.AddError("Version", "version format is invalid, expected Major.Minor.Build.Revision");

         if (ex.HasErrors)
            throw ex;
      }

      public bool Equals(Package other)
      {
         if (ReferenceEquals(other, null)) return false;
         return Key.Equals(other.Key);
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(obj, null)) return false;
         if (ReferenceEquals(obj, this)) return true;
         if (GetType() != obj.GetType()) return false;
         return Equals((Package) obj);
      }

      public override int GetHashCode()
      {
         return Key.GetHashCode();
      }

      public override string ToString()
      {
         return this.ToXmlString();
      }

      public virtual object Clone()
      {
         return new Package(this);
      }

      /// <summary>
      /// Looks up a dependency with the specific key
      /// </summary>
      /// <param name="key"></param>
      /// <returns></returns>
      public PackageDependency GetPackageDependency(PackageKey key)
      {
         if(Dependencies != null)
         {
            return Dependencies.FirstOrDefault(d => d.PackageId == key.PackageId && d.Platform == key.Platform);
         }

         return null;
      }
   }
}
