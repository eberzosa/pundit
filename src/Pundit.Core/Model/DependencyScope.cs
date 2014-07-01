using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   /// <summary>
   /// Package dependency scope
   /// </summary>
   [XmlRoot("scope")]
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
}