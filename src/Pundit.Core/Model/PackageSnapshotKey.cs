using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   public enum SnapshotPackageDiff
   {
      Add = 0,
      Del = 1
   }

   [XmlRoot("key")]
   [DataContract]
   public class PackageSnapshotKey
   {
      [XmlAttribute("diff")]
      [DataMember(Name = "diff")]
      public SnapshotPackageDiff Diff { get; set; }

      [XmlElement("manifest")]
      [DataMember(Name = "manifest")]
      public Package Manifest { get; set; }

      public PackageSnapshotKey()
      {
         
      }

      public PackageSnapshotKey(Package manifest, SnapshotPackageDiff diff = SnapshotPackageDiff.Add)
      {
         Diff = SnapshotPackageDiff.Add;
         Manifest = manifest;
      }
   }
}
