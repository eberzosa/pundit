using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NGem.Model
{
   [XmlRoot("project")]
   public class Gem
   {
      private List<Dependency> _dependencies = new List<Dependency>();

      [XmlElement("packageId")]
      public string PackageId { get; set; }

      [XmlIgnore]
      public Version Version { get; set; }

      [XmlElement("version")]
      public string VersionString
      {
         get { return this.Version.ToString(); }
         set { this.Version = new Version(value);}
      }

      [XmlArray("dependencies")]
      [XmlArrayItem("package")]
      public List<Dependency> Dependencies
      {
         get { return _dependencies; }
         set { _dependencies = new List<Dependency>(value);}
      }

      public static void WriteTo(Stream s, Gem g)
      {
         XmlSerializer x = new XmlSerializer(typeof(Gem));
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
