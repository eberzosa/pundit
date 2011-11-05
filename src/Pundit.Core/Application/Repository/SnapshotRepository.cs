using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pundit.Core.Model;

namespace Pundit.Core.Application.Repository
{
   /// <summary>
   /// Snapshot-based repository
   /// </summary>
   class SnapshotRepository : IRepository
   {
      public SnapshotRepository(string snapshotPath)
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

      public Version[] GetVersions(UnresolvedPackage package, VersionPattern pattern)
      {
         throw new NotImplementedException();
      }

      public Package GetManifest(PackageKey key)
      {
         throw new NotImplementedException();
      }

      public bool[] PackagesExist(PackageKey[] packages)
      {
         throw new NotImplementedException();
      }

      public PackageKey[] Search(string substring)
      {
         throw new NotImplementedException();
      }
   }
}
