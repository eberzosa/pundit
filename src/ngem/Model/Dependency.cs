using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NGem.Model
{
   [XmlRoot("package")]
   public class Dependency
   {
      [XmlAttribute("id")]
      public string PackageId { get; set; }

      [XmlAttribute("version")]
      public string VersionPattern { get; set; }
   }
}
