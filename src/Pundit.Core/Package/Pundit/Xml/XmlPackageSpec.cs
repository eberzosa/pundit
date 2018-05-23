using System.Collections.Generic;
using System.Xml.Serialization;

namespace EBerzosa.Pundit.Core.Model.Xml
{
   [XmlRoot("package")]
   public class XmlPackageSpec : XmlPackageManifestRoot
   {
      [XmlArray("files")]
      [XmlArrayItem("file")]
      public List<XmlSourceFiles> Files { get; set; }
   }
}
