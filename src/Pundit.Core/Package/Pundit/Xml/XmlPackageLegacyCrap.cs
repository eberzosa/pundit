using System.Collections.Generic;
using System.Xml.Serialization;

namespace EBerzosa.Pundit.Core.Model.Xml
{
   [XmlRoot("package")]
   [XmlType("DevPackage")]
   public class XmlPackageLegacyCrap : XmlPackageManifest
   {
      // This needs to be moved to the root. Is here to be able to resolve Pundit packages dependencies for NuGet as NuGet does not have a FW declared
      //[XmlElement("platform")]
      //public string Platform { get; set; }

      [XmlArray("files")]
      [XmlArrayItem("file")]
      public List<XmlSourceFiles> Files { get; set; }
   }
}