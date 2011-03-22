﻿using System;
using System.Collections.Generic;
using System.IO;
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

      public void Publish(Stream packageStream)
      {
         throw new NotImplementedException();
      }

      public IEnumerable<Package> Search(string nameSubstring, VersionPattern minVersion)
      {
         throw new NotImplementedException();
      }

      public Stream Download(string packageId, Version packageVersion)
      {
         throw new NotImplementedException();
      }
   }
}
