using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   [XmlRoot("key")]
   [DataContract]
   public class PackageSnapshotKey
   {
      [XmlAttribute("diff")]
      [DataMember(Name = "diff")]
      public DiffType Diff { get; set; }

      [XmlElement("manifest")]
      [DataMember(Name = "manifest")]
      public Package Manifest { get; set; }

      public PackageSnapshotKey()
      {
         
      }

      public PackageSnapshotKey(Package manifest)
      {
         Diff = DiffType.Add;
         Manifest = manifest;
      }
   }
}
