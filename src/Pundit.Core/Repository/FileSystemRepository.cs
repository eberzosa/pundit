﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Converters;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Package;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.Core.Repository
{
   public class FileSystemRepository : Repository, IRepository
   {
      private readonly PackageReaderFactory _packageReaderFactory;
      
      public FileSystemRepository(PackageReaderFactory packageReaderFactory, string rootPath, string name)
         : base(rootPath, name, RepositoryType.Pundit)
      {
         Guard.NotNull(packageReaderFactory, nameof(packageReaderFactory));

         _packageReaderFactory = packageReaderFactory;
      }

      public void Publish(string packagePath)
      {
         if (!CanPublish)
            throw new Exception("Publish is not allowed");
                     
         var fileName = Path.GetFileName(packagePath);
         
         var searchPattern = PackageConverterExtensions.GetPackageKeyFromFileName(fileName).GetRelatedSearchFileName();

         foreach (var relatedBuild in Directory.GetFiles(RootPath, searchPattern))
            File.Delete(relatedBuild);

         var targetPath = Path.Combine(RootPath, fileName);
         if (File.Exists(targetPath))
            File.Delete(targetPath);

         File.Move(packagePath, targetPath);
      }

      public Stream Download(PackageKey key)
      {
         var fullPath = Path.Combine(RootPath, key.GetFileName());

         if (!File.Exists(fullPath))
            throw new FileNotFoundException("package not found");

         return File.OpenRead(fullPath);
      }

      public ICollection<NuGet.Versioning.NuGetVersion> GetVersions(UnresolvedPackage package)
      {
         var filePattern = package.GetSearchFileName();

         return new DirectoryInfo(RootPath).GetFiles(filePattern)
            .Select(i => PackageExtensions.GetPackageKeyFromFileName(i.Name).Version).ToArray();
      }

      public PackageManifest GetManifest(PackageKey key, NuGet.Frameworks.NuGetFramework projectFramework = null)
      {
         var fullPath = Path.Combine(RootPath, key.Framework != null
            ? key.GetFileName()
            : key.GetNoFrameworkFileName());
         
         if (!File.Exists(fullPath))
            throw new FileNotFoundException("package not found");

         using (IPackageReader reader = _packageReaderFactory.Get(RepositoryType.Pundit, File.OpenRead(fullPath)))
            return reader.ReadManifest();
      }

      public bool PackageExist(PackageKey package)
      {
         return File.Exists(Path.Combine(RootPath, package.GetFileName()));
      }

      public IEnumerable<PackageKey> Search(string substring)
      {
         foreach (var file in new DirectoryInfo(RootPath).GetFiles("*" + substring + "*"))
         {
            PackageKey key = null;

            try
            {
               key = PackageExtensions.GetPackageKeyFromFileName(file.Name);
            }
            catch (ArgumentException)
            {
            }

            if (key != null)
               yield return key;
         }
      }

      public override string ToString() => $"{Name} [{RootPath}]";
   }
}
