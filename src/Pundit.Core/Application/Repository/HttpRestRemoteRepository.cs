using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pundit.Core.Model;

namespace Pundit.Core.Application.Repository
{
   class HttpRestRemoteRepository : IRemoteRepository
   {
      public HttpRestRemoteRepository(string absoluteUri)
      {
         
      }

      public void Publish(Stream packageStream)
      {
         throw new NotImplementedException();
      }

      public Stream Download(PackageKey key)
      {
         throw new NotImplementedException();
      }

      public PackageSnapshotKey[] GetSnapshot(string changeId, out string nextChangeId)
      {
         throw new NotImplementedException();
      }
   }
}
