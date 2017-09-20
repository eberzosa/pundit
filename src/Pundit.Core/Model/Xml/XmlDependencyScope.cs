using System.Xml.Serialization;

namespace EBerzosa.Pundit.Core.Model.Xml
{
   public enum XmlDependencyScope
   {
      [XmlEnum("normal")]
      Normal,

      [XmlEnum("test")]
      Test,

      [XmlEnum("build")]
      Build
   }
}