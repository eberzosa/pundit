using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGem.Core.Model;

namespace NGem.Repository
{
   class LocalRepository : IRepository
   {
      private readonly string _rootPath;

      public LocalRepository(string rootPath)
      {
         _rootPath = rootPath;
      }

      public IEnumerable<Package> SearchPackage(string nameSubstring, VersionPattern minVersion)
      {
         throw new NotImplementedException();
      }
   }
}
