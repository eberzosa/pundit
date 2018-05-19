using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace EBerzosa.Pundit.Core.Model.Xml.Nuget
{
   [XmlRoot("package", Namespace = "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd")]
   public class XmlNugetSpec : XmlPackageManifest
   {
      [XmlArray("files")]
      [XmlArrayItem("file")]
      public List<XmlSourceFiles> Files { get; set; }
   }
}
