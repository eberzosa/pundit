﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pundit.Core.Model;

namespace Pundit.Test.Mocks
{
   public class MockedRepository : IRepository
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

      public bool[] PackagesExist(PackageKey[] packages)
      {
         throw new NotImplementedException();
      }

      public PackageKey[] Search(string substring)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}
