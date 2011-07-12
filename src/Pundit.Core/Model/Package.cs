using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   [XmlRoot("package")]
   [XmlInclude(typeof(DevPackage))]
   public class Package : ICloneable
   {
      public const string DefaultManifestFileName = "pundit.xml"; //package definition
      public const string PackedExtension = ".pundit";

      private static Regex _packageStringRgx = new Regex("^[0-9a-zA-Z\\._]+$");
      private static Regex _packageVersionRgx = new Regex("^[0-9\\*]+(\\.[0-9\\*]+){1,3}$");
      private const string PackageStringDescr = "allowed characters: letters (A-Z, a-z), numbers, underscore (_) and dot sign (.)";

      private List<PackageDependency> _dependencies = new List<PackageDependency>();

      [XmlAttribute("coreVersion")]
      public string CoreVersion { get; set; }

      //WARNING!!! remember to reflect copy constructor if adding a new property to this class

      [XmlElement("packageId")]
      public string PackageId { get; set; }

      [XmlElement("platform")]
      public string Platform { get; set; }

      [XmlElement("project-url")]
      public string ProjectUrl { get; set; }

      [XmlIgnore]
      public Version Version { get; set; }

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

      [XmlElement("release-notes")]
      public string ReleaseNotes { get; set; }

      [XmlElement("license")]
      public string License { get; set; }

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
         CoreVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
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

      public Package(string packageId, Version version)
      {
         if (packageId == null) throw new ArgumentNullException("packageId");

         PackageId = packageId;
         VersionString = version.ToString();
      }

      public static Package FromStream(Stream inputStream)
      {
         XmlSerializer xmls = new XmlSerializer(typeof(Package));

         Package dp = (Package)xmls.Deserialize(inputStream);

         dp.Validate();

         return dp;
      }

      public virtual void WriteTo(Stream s)
      {
         Validate();

         XmlWriterSettings settings = new XmlWriterSettings();
         //settings.OmitXmlDeclaration = true;
         settings.Encoding = Encoding.UTF8;
         settings.Indent = true;

         using (XmlWriter writer = XmlWriter.Create(s, settings))
         {
            XmlSerializer x = new XmlSerializer(typeof (Package));
            x.Serialize(writer, this);
         }
      }

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

      private bool IsValidPackageNameString(string s)
      {
         return _packageStringRgx.IsMatch(s);
      }

      private bool IsValidPackageVersion(string s)
      {
         return _packageVersionRgx.IsMatch(s);
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

         if(Version != null && !IsValidPackageVersion(Version.ToString()))
            ex.AddError("Version", "version format is invalid, expected Major.Minor.Build.Revision");

         if (ex.HasErrors)
            throw ex;
      }

      public override string ToString()
      {
         using (var ms = new MemoryStream())
         {
            WriteTo(ms);
            ms.Position = 0;

            return Encoding.UTF8.GetString(ms.GetBuffer());
         }
      }

      public object Clone()
      {
         return new Package(this);
      }

      public PackageDependency GetPackageDependency(PackageKey key)
      {
         if(Dependencies != null)
         {
            return Dependencies.Where(d => d.PackageId == key.PackageId && d.Platform == key.Platform).FirstOrDefault();
         }

         return null;
      }
   }
}
