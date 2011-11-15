using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pundit.Core.Model
{
   [DataContract]
   public class RemoteSnapshot
   {
      [DataMember(Name = "changes")]
      public PackageSnapshotKey[] Changes { get; set; }

      [DataMember(Name = "delta")]
      public string NextChangeId { get; set; }
   }
}
