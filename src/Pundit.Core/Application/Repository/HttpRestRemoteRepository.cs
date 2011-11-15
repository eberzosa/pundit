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

      public Stream Download(string platform, string packageId, string version)
      {
         throw new NotImplementedException();
      }

      public RemoteSnapshot GetSnapshot(string changeId)
      {
         throw new NotImplementedException();
      }
   }
}
