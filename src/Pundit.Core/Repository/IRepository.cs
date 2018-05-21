﻿using System.Collections.Generic;
using System.IO;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Versioning;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Repository
{
   public interface IRepository
   {
      string Name { get; }

      string RootPath { get; }

      bool CanPublish { get; set; }

      RepositoryType Type { get; }

      void Publish(Stream packageStream);
      
      Stream Download(PackageKey key);

      ICollection<NuGet.Versioning.NuGetVersion> GetVersions(UnresolvedPackage package);

      PackageManifest GetManifest(PackageKey key);

      bool PackageExist(PackageKey package);

      IEnumerable<PackageKey> Search(string substring);
   }
}
