using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pundit.Core.Application.Server;
using Pundit.Core.Model;

namespace Pundit.Core
{
   /// <summary>
   /// 
   /// </summary>
   public static class RemoteRepositoryServerFactory
   {
      /// <summary>
      /// Creates <see cref="IRemoteRepository"/> implementtion using sqlite for metadata storage and disk for package storage
      /// </summary>
      /// <param name="rootDir">Root directory for both sql db and data which must exist. If it's empty, a new db will be created.</param>
      /// <returns></returns>
      public static IRemoteRepository CreateSqlDiskServer(string rootDir)
      {
         return new RepositoryServer(rootDir);
      }
   }
}
