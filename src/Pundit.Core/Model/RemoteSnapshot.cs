using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pundit.Core.Model
{
   /// <summary>
   /// 
   /// </summary>
   [DataContract]
   public class RemoteSnapshot
   {
      /// <summary>
      /// False indicates that this is a full snapshot as opposet to just a changeset
      /// </summary>
      [DataMember(Name = "isDelta")]
      public bool IsDelta { get; set; }

      /// <summary>
      /// Changes of the full snapshot or a delta
      /// </summary>
      [DataMember(Name = "changes")]
      public PackageSnapshotKey[] Changes { get; set; }

      /// <summary>
      /// Next delta id. Call <see cref="IRemoteRepository"/> with this delta value to obtain only a changeset.
      /// </summary>
      [DataMember(Name = "delta")]
      public string NextChangeId { get; set; }

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
      /// <param name="nextChangeId"></param>
      /// <param name="changes"></param>
      public RemoteSnapshot(bool isDelta, string nextChangeId, PackageSnapshotKey[] changes)
      {
         IsDelta = isDelta;
         NextChangeId = nextChangeId;
         Changes = changes;
      }
   }
}
