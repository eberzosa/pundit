using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EBerzosa.Pundit.Core.PackageManager.Xml
{
   [DebuggerDisplay("{PackageId} {Version} [{Framework}]")]
   public class XmlPackageKey
   {
      [XmlAttribute("id")]
      public string PackageId { get; set; }
      
      [XmlAttribute("version")]
      [DataMember]
      public string Version { get; set; }
      
      [XmlAttribute("platform")]
      [DataMember]
      public string Framework { get; set; }
   }
}
