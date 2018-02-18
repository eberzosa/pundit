using System.Xml.Serialization;

namespace EBerzosa.Pundit.Core.Repository.Xml
{
   public class XmlRegisteredRepository
   {
      [XmlAttribute("name")]
      public string Name { get; set; }

      [XmlAttribute("uri")]
      public string Uri { get; set; }

      [XmlAttribute("publish")]
      public bool UseForPublishing { get; set; }
   }
}