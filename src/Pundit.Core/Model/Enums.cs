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

   /// <summary>
   /// Package dependency scope
   /// </summary>
   public enum DependencyScope
   {
      /// <summary>
      /// Dependency gets published and included in the production manifest as a reference.
      /// </summary>
      [XmlEnum("normal")]
      Normal,

      /// <summary>
      /// Dependency is required only at test time. Will not be included into production manifest.
      /// </summary>
      [XmlEnum("test")]
      Test,

      /// <summary>
      /// Dependency is required only at build time. Will not be included into production manifest.
      /// </summary>
      [XmlEnum("build")]
      Build
   }

   public enum DiffType
   {
      NoChange,

      Add,

      Mod,

      Del
   }
}
