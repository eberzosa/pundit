using System.Xml.Serialization;

namespace EBerzosa.Pundit.Core.Model.Xml
{
   public class XmlSourceFiles
   {
      [XmlAttribute("kind")]
      public XmlPackageFileKind FileKind { get; set; }

      [XmlAttribute("include")]
      public string Include { get; set; }

      [XmlAttribute("exclude")]
      public string Exclude { get; set; }

      [XmlAttribute("flatten")]
      public bool Flatten { get; set; }

      [XmlAttribute("basedir")]
      public string BaseDirectory { get; set; }

      [XmlAttribute("targetdir")]
      public string TargetDirectory { get; set; }

      [XmlAttribute("includeemptydirs")]
      public bool IncludeEmptyDirs { get; set; }

      [XmlAttribute("configuration")]
      public XmlBuildConfiguration Configuration { get; set; }
   }
}