using System.IO;
using EBerzosa.Pundit.Core.Serializers;
using Pundit.Core.Model;

namespace Pundit.Core.Application
{
   public class InstalledPackagesManager
   {
      private const string CacheFileName = ".pundit-install-index";

      private readonly InstalledPackagesIndexSerializer _installedPackagesIndexSerializer;

      public InstalledPackagesManager(InstalledPackagesIndexSerializer installedPackagesIndexSerializer)
      {
         _installedPackagesIndexSerializer = installedPackagesIndexSerializer;
      }

      public InstalledPackagesIndex GetNew(string rootFolder, BuildConfiguration configuration)
      {
         var fullPath = Path.Combine(rootFolder, CacheFileName);

         return new InstalledPackagesIndex(fullPath) {Configuration = configuration};
      }

      public InstalledPackagesIndex ReadFromFolder(string rootFolder)
      {
         var fullPath = Path.Combine(rootFolder, CacheFileName);

         var fileInfo = new FileInfo(fullPath);
         if (!fileInfo.Exists || fileInfo.Length == 0)
            return new InstalledPackagesIndex(fullPath);

         using (Stream stream = File.OpenRead(fullPath))
            return _installedPackagesIndexSerializer.DeserializeInstalledPackagesIndex(stream, fullPath);
      }

      public void Save(InstalledPackagesIndex installedPackagesIndex)
      {
         if (File.Exists(installedPackagesIndex.Location))
            File.Delete(installedPackagesIndex.Location);

         using (Stream stream = File.Open(installedPackagesIndex.Location, FileMode.CreateNew, FileAccess.Write))
            _installedPackagesIndexSerializer.SerializePackageManifest(installedPackagesIndex, stream);
         
         new FileInfo(installedPackagesIndex.Location).Attributes |= FileAttributes.Hidden;
      }
   }
}