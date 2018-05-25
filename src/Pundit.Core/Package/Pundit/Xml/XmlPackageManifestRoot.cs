using System.Diagnostics;
using System.Xml.Serialization;

namespace EBerzosa.Pundit.Core.Model.Xml
{
   [DebuggerDisplay("{PackageId} [{Version}] [{Platform}] [Deps={Dependencies?.Count}]")]
   [XmlRoot("package")]
   public class XmlPackageManifestRoot : XmlPackageManifest
   {
      //[XmlElement("platform")]
      //public string Platform { get; set; }
   }
}