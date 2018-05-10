using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Package;
using EBerzosa.Utils;
using NuGet.Versioning;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Repository
{
   public class FileSystemRepository : IRepository
   {
      private readonly PackageReaderFactory _packageReaderFactory;

      public string Name { get; }

      public bool CanPublish { get; set; } = false;

      public string RootPath { get; }

      public RepositoryType Type { get; }

      public FileSystemRepository(PackageReaderFactory packageReaderFactory, string rootPath, string name, RepositoryType type)
      {
         Guard.NotNull(packageReaderFactory, nameof(packageReaderFactory));
         Guard.NotNull(rootPath, nameof(rootPath));

         if (!Directory.Exists(rootPath))
            throw new ArgumentException($"Root directory '{rootPath}' does not exist");

         _packageReaderFactory = packageReaderFactory;
         RootPath = rootPath;
         Name = name;
         Type = type;
      }

      public void Publish(Stream packageStream)
      {
         if (!CanPublish)
            throw new Exception("Publish is not allowed");

         var tempFile = Path.Combine(RootPath, "download-" + Guid.NewGuid());

         using (Stream ts = File.Create(tempFile))
            packageStream.CopyTo(ts);


         PackageManifest manifest;
         using (Stream ts = File.OpenRead(tempFile))
         using (var pr = _packageReaderFactory.Get(RepositoryType.Pundit, ts))
            manifest = pr.ReadManifest();

         foreach (var relatedBuild in Directory.GetFiles(RootPath, new PackageFileName(manifest).RelatedSearchFileName))
            File.Delete(relatedBuild);


         var targetPath = Path.Combine(RootPath, new PackageFileName(manifest).FileName);
         if (File.Exists(targetPath))
            File.Delete(targetPath);

         File.Move(tempFile, targetPath);
      }

      public Stream Download(PackageKey key)
      {
         var fullPath = Path.Combine(RootPath, new PackageFileName(key).FileName);

         if (!File.Exists(fullPath))
            throw new FileNotFoundException("package not found");

         return File.OpenRead(fullPath);
      }

      public ICollection<NuGetVersion> GetVersions(UnresolvedPackage package)
      {
         var filePattern = new PackageFileName(package).SearchFileName;

         if (filePattern == null)
            return new NuGetVersion[0];

         return new DirectoryInfo(RootPath).GetFiles(filePattern)
            .Select(i => PackageFileName.GetPackageKeyFromFileName(i.Name).Version).ToArray();
      }

      public PackageManifest GetManifest(PackageKey key)
      {
         var fullPath = Path.Combine(RootPath, new PackageFileName(key).FileName);

         if (!File.Exists(fullPath))
            throw new FileNotFoundException("package not found");

         using (IPackageReader reader = _packageReaderFactory.Get(RepositoryType.Pundit, File.OpenRead(fullPath)))
            return reader.ReadManifest();
      }

      public bool PackageExist(PackageKey package)
      {
         return File.Exists(Path.Combine(RootPath, new PackageFileName(package).FileName));
      }

      public IEnumerable<PackageKey> Search(string substring)
      {
         foreach (var file in new DirectoryInfo(RootPath).GetFiles("*" + substring + "*"))
         {
            PackageKey key = null;

            try
            {
               key = PackageFileName.GetPackageKeyFromFileName(file.Name);
            }
            catch (ArgumentException)
            {
            }

            if (key != null)
               yield return key;
         }
      }
   }
}
