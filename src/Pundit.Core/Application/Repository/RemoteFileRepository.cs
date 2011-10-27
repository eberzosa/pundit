using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pundit.Core.Model;

namespace Pundit.Core.Application.Repository
{
   class RemoteFileRepository : IRemoteRepository
   {
      private readonly string _rootPath;

      public RemoteFileRepository(string rootPath)
      {
         if (rootPath == null) throw new ArgumentNullException("rootPath");
         if (!Directory.Exists(rootPath))
            throw new ArgumentException("root directory [" + rootPath + "] does not exist", "rootPath");

         _rootPath = rootPath;
      }

      public void Publish(Stream packageStream)
      {
         throw new NotImplementedException();
      }

      public Stream Download(PackageKey key)
      {
         throw new NotImplementedException();
      }

      public PackageSnapshotKey[] GetSnapshot(string changeId, out long nextChangeId)
      {
         nextChangeId = 0;

         throw new NotImplementedException();
      }
   }
}
