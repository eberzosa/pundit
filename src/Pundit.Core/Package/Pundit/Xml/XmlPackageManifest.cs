﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace EBerzosa.Pundit.Core.Model.Xml
{
   [DebuggerDisplay("{PackageId} [{Version}] [Deps={Dependencies?.Count}]")]
   [XmlRoot("package")]
   public class XmlPackageManifest
   {
      [XmlAttribute("coreVersion")]
      public string CoreVersion { get; set; }
      
      [XmlElement("packageId")]
      public string PackageId { get; set; }

      [XmlElement("project-url")]
      public string ProjectUrl { get; set; }
      
      [XmlElement("version")]
      public string Version { get; set; }

      // This needs to be moved to the root. Is here to be able to resolve Pundit packages dependencies for NuGet as NuGet does not have a FW declared
      [XmlElement("platform")]
      public string Platform { get; set; }

      [XmlElement("author")]
      public string Author { get; set; }

      [XmlElement("description")]
      public string Description { get; set; }

      [XmlElement("release-notes")]
      public string ReleaseNotes { get; set; }

      [XmlElement("license")]
      public string License { get; set; }

      [XmlArray("dependencies")]
      [XmlArrayItem("package")]
      public List<XmlPackageDependency> Dependencies { get; set; }
   }
}
