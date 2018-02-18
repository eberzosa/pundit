using System.Xml.Serialization;

namespace EBerzosa.Pundit.Core.Repository.Xml
{
   [XmlRoot("repositories")]
   public class XmlRegisteredRepositories
   {
      [XmlAttribute("auto-update")]
      public bool AllowAutoUpdate { get; set; }

      [XmlAttribute("last-updated")]
      public long LastUpdated { get; set; }

      [XmlArray("list")]
      [XmlArrayItem("repository")]
      public XmlRegisteredRepository[] RepositoriesArray { get; set; }
   }
}