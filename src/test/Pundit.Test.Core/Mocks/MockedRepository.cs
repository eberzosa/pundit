using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pundit.Core.Model;

namespace Pundit.Test.Mocks
{
   public class MockedRepository : ILocalRepository
   {
      private Dictionary<string, Version[]> _versions = new Dictionary<string, Version[]>();
      private Dictionary<PackageKey, Package> _manifests = new Dictionary<PackageKey, Package>();

      public void SetVersions(string packageId, params Version[] versions)
      {
         _versions[packageId] = versions;
      }

      public void SetManifest(PackageKey key, Package pkg)
      {
         _manifests[key] = pkg;
      }

      #region [Interface]

      public ICollection<Version> GetVersions(UnresolvedPackage package, VersionPattern pattern)
      {
         Version[] all = _versions[package.PackageId];
         List<Version> matching = new List<Version>();

         foreach(Version v in all)
         {
            if(pattern.Matches(v))
               matching.Add(v);
         }

         return matching.ToArray();
      }

      public Package GetManifest(PackageKey key)
      {
         return _manifests[key];
      }

      public ICollection<bool> BinariesExists(IEnumerable<PackageKey> packages)
      {
         throw new NotImplementedException();
      }

      public Repo FindOnlineRepository(PackageKey key)
      {
         throw new NotImplementedException();
      }

      public ICollection<PackageKey> Search(string substring)
      {
         throw new NotImplementedException();
      }

      public void Put(Stream packageStream, Action<long> readCallback)
      {
         throw new NotImplementedException();
      }

      public Stream Get(PackageKey binaryKey)
      {
         throw new NotImplementedException();
      }

      #endregion

      #region Implementation of IDisposable

      public void Dispose()
      {
      }

      #endregion
   }
}
