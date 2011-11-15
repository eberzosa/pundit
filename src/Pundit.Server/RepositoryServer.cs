using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pundit.Core.Model;

namespace Pundit.Server
{
   public class RepositoryServer : IRemoteRepository
   {
      public void Publish(Stream packageStream)
      {
      }

      public Stream Download(string platform, string packageId, string version)
      {
         return Download(new PackageKey(packageId, new Version(version), platform));
      }

      private Stream Download(PackageKey key)
      {
         return null;
      }

      public RemoteSnapshot GetSnapshot(string changeId)
      {
         PackageSnapshotKey[] keys = new[] {new PackageSnapshotKey(new Package("test1", new Version("1.2.10")))};

         return new RemoteSnapshot {Changes = keys, NextChangeId = "testnext"};
      }
   }
}
