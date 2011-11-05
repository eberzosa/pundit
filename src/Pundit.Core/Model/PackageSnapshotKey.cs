using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   [XmlRoot("key")]
   public class PackageSnapshotKey
   {
      [XmlAttribute("diff")]
      public DiffType Diff { get; set; }

      [XmlElement("manifest")]
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
