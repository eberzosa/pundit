using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   public enum PackageFileKind
   {
      /// <summary>
      /// Placed in project-root/lib
      /// </summary>
      [XmlEnum("bin")]
      Binary,

      /// <summary>
      /// Placed in project-root/include/package-id
      /// </summary>
      [XmlEnum("include")]
      Include,

      /// <summary>
      /// Placed in project-root/tools/package-id
      /// </summary>
      [XmlEnum("tools")]
      Tools,

      /// <summary>
      /// placed in project-root/other/package-id
      /// </summary>
      [XmlEnum("other")]
      Other
   }
}