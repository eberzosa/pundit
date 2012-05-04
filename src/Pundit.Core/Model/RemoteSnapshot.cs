using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Pundit.Core.Model
{
   /// <summary>
   /// 
   /// </summary>
   [DataContract]
   public class RemoteSnapshot
   {
      /// <summary>
      /// False indicates that this is a full snapshot as opposed to just a changeset. It will be returned
      /// if you are passing a wrong delta which cannot be resolved or repote repository does not support
      /// incremental updates
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
      /// <param name="changes"></param>
      /// <param name="nextChangeId"></param>
      public RemoteSnapshot(bool isDelta, IEnumerable<PackageSnapshotKey> changes, string nextChangeId)
      {
         IsDelta = isDelta;
         if (changes != null) Changes = changes.ToArray();
         NextChangeId = nextChangeId;
      }
   }
}
