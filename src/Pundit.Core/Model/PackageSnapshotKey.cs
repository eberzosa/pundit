using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   /// <summary>
   /// Difference type
   /// </summary>
   public enum SnapshotPackageDiff
   {
      /// <summary>
      /// Package was added
      /// </summary>
      [XmlEnum("add")]
      Add = 0,

      /// <summary>
      /// Package was deleted
      /// </summary>
      [XmlEnum("del")]
      Del = 1
   }

   /// <summary>
   /// Snapshot atom
   /// </summary>
   [XmlRoot("key")]
   public class PackageSnapshotKey
   {
      /// <summary>
      /// Modification type
      /// </summary>
      [XmlAttribute("diff")]
      public SnapshotPackageDiff Diff { get; set; }

      [XmlElement("manifest")]
      public Package Manifest { get; set; }

      public PackageSnapshotKey()
      {
         
      }

      public PackageSnapshotKey(Package manifest, SnapshotPackageDiff diff)
      {
         if (manifest == null) throw new ArgumentNullException("manifest");

         Diff = diff;
         Manifest = manifest;
      }

      public override string ToString()
      {
         return string.Format("{0}: {1}", Diff, Manifest.Key);
      }
   }
}
