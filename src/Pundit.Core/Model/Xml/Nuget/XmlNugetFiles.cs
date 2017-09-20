using System.Xml.Serialization;

namespace EBerzosa.Pundit.Core.Model.Xml.Nuget
{
   public class XmlNugetFiles
   {
      [XmlAttribute("src")]
      public string Include { get; set; }

      [XmlAttribute("exclude")]
      public string Exclude { get; set; }
      
      // Must begin with lib, content, build or tools
      [XmlAttribute("target")]
      public string TargetDirectory { get; set; }
   }
}