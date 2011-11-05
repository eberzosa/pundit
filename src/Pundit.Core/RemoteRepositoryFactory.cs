using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pundit.Core.Application.Repository;
using Pundit.Core.Model;

namespace Pundit.Core
{
   static class RemoteRepositoryFactory
   {
      private const string HttpRepoPrefix = "http://";
      private const string DirRepoPrefix = "dir://";

      public static IRemoteRepository Create(Repo r)
      {
         if (r == null) throw new ArgumentNullException("r");

         if(r.Uri.StartsWith(DirRepoPrefix))
            return new RemoteFolderRepository(r.Uri.Substring(DirRepoPrefix.Length));

         if(r.Uri.StartsWith(HttpRepoPrefix))
            return new HttpRestRemoteRepository(r.Uri.Substring(HttpRepoPrefix.Length));

         throw new NotSupportedException("repository " + r.Uri + " not supported");
      }
   }
}
