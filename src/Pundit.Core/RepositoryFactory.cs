﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pundit.Core.Application.Repository;
using Pundit.Core.Model;

namespace Pundit.Core
{
   public static class RepositoryFactory
   {
      private const string SqliteRepoPrefix = "sqlite://";
      private const string DirRepoPrefix = "dir://";

      public static IRepository CreateFromUri(string uri)
      {
         if (uri == null) throw new ArgumentNullException("uri");

         if(uri.StartsWith(SqliteRepoPrefix))
            return new SqliteRepository(uri.Substring(SqliteRepoPrefix.Length));

         if(uri.StartsWith(DirRepoPrefix))
            return new FileRepository(uri);

         throw new ArgumentException("unknown repository type: " + uri);
      }
   }
}
