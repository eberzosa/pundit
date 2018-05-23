using System.Collections.Generic;
using System.IO;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;

namespace EBerzosa.Pundit.Core.Repository
{
   public interface IRepository
   {
      string Name { get; }

      string RootPath { get; }

      bool CanPublish { get; set; }

      RepositoryType Type { get; }

      void Publish(string packagePath);
      
      Stream Download(PackageKey key);

      ICollection<NuGet.Versioning.NuGetVersion> GetVersions(UnresolvedPackage package);

      PackageManifest GetManifest(PackageKey key, NuGet.Frameworks.NuGetFramework projectFramework);

      bool PackageExist(PackageKey package);

      IEnumerable<PackageKey> Search(string substring);
   }
}
