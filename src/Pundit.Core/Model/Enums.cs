using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   /// <summary>
   /// Build configuration (debug or release)
   /// </summary>
   public enum BuildConfiguration
   {
      [XmlEnum("any")]
      Any = 0,

      /// <summary>
      /// Release build files. This is a defalt configuration.
      /// </summary>
      [XmlEnum("release")]
      Release,

      /// <summary>
      /// Debug build files (optional)
      /// </summary>
      [XmlEnum("debug")]
      Debug,

   }

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
      Other,

      [XmlEnum("unknown")]
      Unknown
   }

   public enum DependencyScope
   {
      [XmlEnum("normal")]
      Normal,

      [XmlEnum("test")]
      Test,

      [XmlEnum("build")]
      Build
   }

   [DataContract]
   public enum DiffType
   {
      NoChange,

      Add,

      Mod,

      Del
   }
}
