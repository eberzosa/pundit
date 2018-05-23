using System.Xml.Serialization;

namespace EBerzosa.Pundit.Core.Model.Xml
{
   public enum XmlPackageFileKind
   {
      [XmlEnum("bin")]
      Binary,
      
      [XmlEnum("include")]
      Include,
      
      [XmlEnum("tools")]
      Tools,
      
      [XmlEnum("other")]
      Other,

      [XmlEnum("unknown")]
      Unknown
   }
}