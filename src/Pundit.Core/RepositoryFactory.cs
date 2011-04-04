using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pundit.Core.Application.Repository;
using Pundit.Core.Model;

namespace Pundit.Core
{
   public static class RepositoryFactory
   {
      public static IRepository CreateFromUri(string uri)
      {


         return new FileRepository(uri);
      }
   }
}
