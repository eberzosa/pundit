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
}
