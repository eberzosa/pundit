using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Application.Repository
{
   public class FileRepository : IRepository
   {
      private readonly string _rootPath;

      public FileRepository(string rootPath)
      {
         if (rootPath == null) throw new ArgumentNullException("rootPath");
         if (!Directory.Exists(rootPath))
            throw new ArgumentException("root directory [" + rootPath + "] does not exist", "rootPath");

         _rootPath = rootPath;
      }

      public string RootPath
      {
         get { return _rootPath; }
      }

      public void Publish(Stream packageStream)
      {
         //download file
         string tempFile = Path.Combine(_rootPath, "download-" + Guid.NewGuid());

         using(Stream ts = File.Create(tempFile))
         {
            packageStream.CopyTo(ts);
         }

         //get manifest
         Package manifest;
         using (Stream ts = File.OpenRead(tempFile))
         {
            using (var pr = new PackageReader(ts))
            {
               manifest = pr.ReadManifest();
            }
         }

         //remove all related builds
         foreach(string rb in PackageUtils.SearchAllRelatedBuilds(_rootPath, manifest))
         {
            File.Delete(rb);
         }

         //write to disk
         string targetFileName = PackageUtils.GetFileName(manifest);
         string targetPath = Path.Combine(_rootPath, targetFileName);
         if(File.Exists(targetPath)) File.Delete(targetPath);
         File.Move(tempFile, targetPath);
      }

      public Stream Download(PackageKey key)
      {
         string fullPath = Path.Combine(_rootPath, PackageUtils.GetFileName(key));

         if(!File.Exists(fullPath))
            throw new FileNotFoundException("package not found");

         return File.OpenRead(fullPath);
      }

      public Version[] GetVersions(UnresolvedPackage package, VersionPattern pattern)
      {
         var versions = new List<Version>();

         string filePattern = PackageUtils.GetSearchPattern(package, pattern);
         
         foreach(FileInfo file in new DirectoryInfo(_rootPath).GetFiles(filePattern))
         {
            PackageKey key = PackageUtils.GetPackageKeyFromFileName(file.Name);

            versions.Add(key.Version);
         }

         return versions.ToArray();
      }

      public Package GetManifest(PackageKey key)
      {
         string fullPath = Path.Combine(_rootPath, PackageUtils.GetFileName(key));

         if(!File.Exists(fullPath))
            throw new FileNotFoundException("package not found");

         using(PackageReader reader = new PackageReader(File.OpenRead(fullPath)))
         {
            return reader.ReadManifest();
         }
      }

      public bool[] PackagesExist(PackageKey[] packages)
      {
         var results = new bool[packages.Length];

         for (int i = 0; i < packages.Length; i++ )
         {
            string fullPath = Path.Combine(_rootPath, PackageUtils.GetFileName(packages[i]));

            results[i] = File.Exists(fullPath);
         }

         return results;
      }
   }
}
