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

      [XmlElement("key")]
      public PackageKey Key { get; set; }

      public PackageSnapshotKey()
      {
         
      }

      public PackageSnapshotKey(PackageKey key)
      {
         Diff = DiffType.Add;
         Key = key;
      }
   }
}
