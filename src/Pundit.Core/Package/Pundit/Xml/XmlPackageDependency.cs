using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace EBerzosa.Pundit.Core.Model.Xml
{
   [DebuggerDisplay("{PackageId} [{VersionPattern}] [{Scope}] [DevOnly={DevTimeOnly}]")]
   [XmlRoot("package")]
   [Serializable]
   public class XmlPackageDependency
   {
      [XmlAttribute("id")]
      public string PackageId { get; set; }

      [XmlAttribute("version")]
      public string VersionPattern { get; set; }
      
      [XmlAttribute("platform")]
      public string Platform { get; set; }

      // Deprecated
      [Obsolete("This is only use for compatibility with old XML packages", false)]
      [XmlAttribute("devtime")]
      [DefaultValue(false)]
      public bool DevTimeOnly { get; set; }

      [XmlAttribute("scope")]
      [DefaultValue(XmlDependencyScope.Normal)]
      public XmlDependencyScope Scope { get; set; }
      
      [XmlAttribute("createPlatformFolder")]
      [DefaultValue(false)]
      public bool CreatePlatformFolder { get; set; }
   }
}
