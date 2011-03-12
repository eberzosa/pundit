using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NGem.Core.Model
{
   [XmlRoot("package")]
   [XmlInclude(typeof(DevPackage))]
   public class Package
   {
      private List<PackageDependency> _dependencies = new List<PackageDependency>();

      [XmlElement("packageId")]
      public string PackageId { get; set; }

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

      /// <summary>
      /// Validates the package. In case of invalid package throws <see cref="InvalidPackageException"/>
      /// </summary>
      public virtual void Validate()
      {
         InvalidPackageException ex = new InvalidPackageException();

         if(string.IsNullOrEmpty(PackageId))
            ex.AddError("PackageId", "package id is required");

         if(Version == null)
            ex.AddError("Version", "version is required");

         if (ex.HasErrors)
            throw ex;
      }

      public override string ToString()
      {
         using (MemoryStream ms = new MemoryStream())
         {
            WriteTo(ms);
            ms.Position = 0;

            return Encoding.UTF8.GetString(ms.GetBuffer());
         }
      }

   }
}
