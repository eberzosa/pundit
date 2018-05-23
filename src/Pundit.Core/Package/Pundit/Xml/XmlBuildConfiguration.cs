using System.Xml.Serialization;

namespace EBerzosa.Pundit.Core.Model.Xml
{
   public enum XmlBuildConfiguration
   {
      [XmlEnum("any")]
      Any = 0,
      
      [XmlEnum("release")]
      Release,
      
      [XmlEnum("debug")]
      Debug
   }
}