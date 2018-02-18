using System;
using System.Collections.Generic;
using System.IO;
using EBerzosa.Pundit.Core.Application;
using EBerzosa.Utils;
using NuGet.Versioning;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace EBerzosa.Pundit.Core.Repository
{
   public class FileSystemRepository : IRepository
   {
      private readonly PackageReaderFactory _packageReaderFactory;

      public string Name { get; }

      public bool CanPublish { get; set; } = false;

      public string RootPath { get; }

      public FileSystemRepository(PackageReaderFactory packageReaderFactory, string rootPath, string name)
      {
         Guard.NotNull(packageReaderFactory, nameof(packageReaderFactory));
         Guard.NotNull(rootPath, nameof(rootPath));

         if (!Directory.Exists(rootPath))
            throw new ArgumentException($"Root directory '{rootPath}' does not exist");

         _packageReaderFactory = packageReaderFactory;
         RootPath = rootPath;
         Name = name;
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
         using (var pr = _packageReaderFactory.Get(ts))
            manifest = pr.ReadManifest();

         foreach (var relatedBuild in PackageUtils.SearchAllRelatedBuilds(RootPath, manifest))
            File.Delete(relatedBuild);


         var targetPath = Path.Combine(RootPath, PackageUtils.GetFileName(manifest));
         if (File.Exists(targetPath))
            File.Delete(targetPath);

         File.Move(tempFile, targetPath);
      }

      public Stream Download(PackageKey key)
      {
         var fullPath = Path.Combine(RootPath, PackageUtils.GetFileName(key));

         if (!File.Exists(fullPath))
            throw new FileNotFoundException("package not found");

         return File.OpenRead(fullPath);
      }

      public NuGetVersion[] GetVersions(UnresolvedPackage package)
      {
         var versions = new List<NuGetVersion>();

         var filePattern = PackageUtils.GetSearchPattern(package);

         foreach (var file in new DirectoryInfo(RootPath).GetFiles(filePattern))
         {
            var key = PackageUtils.GetPackageKeyFromFileName(file.Name);
            versions.Add(key.Version);
         }

         return versions.ToArray();
      }

      public PackageManifest GetManifest(PackageKey key)
      {
         var fullPath = Path.Combine(RootPath, PackageUtils.GetFileName(key));

         if (!File.Exists(fullPath))
            throw new FileNotFoundException("package not found");

         using (PackageReader reader = _packageReaderFactory.Get(File.OpenRead(fullPath)))
            return reader.ReadManifest();
      }

      public bool[] PackagesExist(PackageKey[] packages)
      {
         var results = new bool[packages.Length];

         for (int i = 0; i < packages.Length; i++)
         {
            string fullPath = Path.Combine(RootPath, PackageUtils.GetFileName(packages[i]));

            results[i] = File.Exists(fullPath);
         }

         return results;
      }

      public PackageKey[] Search(string substring)
      {
         var result = new List<PackageKey>();

         foreach (var file in new DirectoryInfo(RootPath).GetFiles("*" + substring + "*"))
         {
            PackageKey key = null;

            try
            {
               key = PackageUtils.GetPackageKeyFromFileName(file.Name);
            }
            catch (ArgumentException)
            {

            }

            if (key != null)
               result.Add(key);
         }

         return result.ToArray();
      }
   }
}
