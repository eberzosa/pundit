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

      NuGetVersion[] GetVersions(UnresolvedPackage package);

      PackageManifest GetManifest(PackageKey key);

      bool[] PackagesExist(PackageKey[] packages);

      PackageKey[] Search(string substring);
   }
}
