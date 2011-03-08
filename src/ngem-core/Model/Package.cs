using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NGem.Core.Model
{
   public class Package
   {
      private List<PackageDependency> _dependencies = new List<PackageDependency>();

      [XmlElement("packageId")]
      public string PackageId { get; set; }

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

      [XmlArray("dependencies")]
      [XmlArrayItem("package")]
      public List<PackageDependency> Dependencies
      {
         get { return _dependencies; }
         set { _dependencies = new List<PackageDependency>(value); }
      }

      public static void WriteTo(Stream s, Package g)
      {
         XmlSerializer x = new XmlSerializer(typeof(Package));
         x.Serialize(s, g);
      }

      public override string ToString()
      {
         MemoryStream ms = new MemoryStream();
         WriteTo(ms, this);
         ms.Position = 0;

         return Encoding.UTF8.GetString(ms.GetBuffer());
      }

   }
}
