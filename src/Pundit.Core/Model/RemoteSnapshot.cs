using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   /// <summary>
   /// 
   /// </summary>
   [XmlRoot("snapshot")]
   public class RemoteSnapshot
   {
      /// <summary>
      /// False indicates that this is a full snapshot as opposed to just a changeset. It will be returned
      /// if you are passing a wrong delta which cannot be resolved or repote repository does not support
      /// incremental updates
      /// </summary>
      [XmlAttribute("isDelta")]
      public bool IsDelta { get; set; }

      /// <summary>
      /// Changes of the full snapshot or a delta
      /// </summary>
      [XmlArray("changes")]
      [XmlArrayItem("key")]
      public PackageSnapshotKey[] Changes { get; set; }

      /// <summary>
      /// Next delta id. Call <see cref="IRemoteRepository"/> with this delta value to obtain only a changeset.
      /// </summary>
      [XmlAttribute("nextDelta")]
      public string NextChangeId { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [XmlAttribute("count")]
      public long Count { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public RemoteSnapshot()
      {
         
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="isDelta"></param>
      /// <param name="changes"></param>
      /// <param name="nextChangeId"></param>
      public RemoteSnapshot(bool isDelta, IEnumerable<PackageSnapshotKey> changes, string nextChangeId)
      {
         IsDelta = isDelta;
         if (changes != null) Changes = changes.ToArray();
         NextChangeId = nextChangeId;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="s"></param>
      public void WriteTo(Stream s)
      {
         this.WriteXmlTo(s);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public string ToXml()
      {
         return this.ToXmlString();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="s"></param>
      /// <returns></returns>
      public static RemoteSnapshot FromXml(string s)
      {
         return s.FromXmlString<RemoteSnapshot>();
      }
   }
}
