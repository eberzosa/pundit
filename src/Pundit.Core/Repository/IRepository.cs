using System.Collections.Generic;
using System.IO;
using NuGet.Versioning;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Repository
{
   public interface IRepository
   {
      string Name { get; }

      string RootPath { get; }

      bool CanPublish { get; set; }

      void Publish(Stream packageStream);
      
      Stream Download(PackageKey key);

      ICollection<NuGetVersion> GetVersions(UnresolvedPackage package);

      PackageManifest GetManifest(PackageKey key);

      bool PackageExist(PackageKey package);

      IEnumerable<PackageKey> Search(string substring);
   }
}
