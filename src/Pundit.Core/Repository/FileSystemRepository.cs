﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Package;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.Core.Repository
{
   public class FileSystemRepository : IRepository
   {
      private readonly PackageReaderFactory _packageReaderFactory;

      public string Name { get; }

      public bool CanPublish { get; set; } = false;

      public string RootPath { get; }

      public string ApiKey { get; }

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

      public void Publish(string packagePath)
      {
         using (var packageStream = File.Open(packagePath, FileMode.Open, FileAccess.Read))
            Publish(packageStream);
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

         Regex regEx;
         if (string.IsNullOrEmpty(manifest.Version.Release))
            regEx = new Regex(manifest.PackageId + "-[0-9.]+-[^A-Z].*", RegexOptions.IgnoreCase);
         else
         {
            var parts = manifest.Version.Release.Split('.');
            var releaseLabel = parts.Length > 0 ? parts[0] + "." : manifest.Version.Release;

            regEx = new Regex(manifest.PackageId + "-[0-9.]+-" + releaseLabel + ".*", RegexOptions.IgnoreCase);
         }

         foreach (var relatedBuild in Directory.GetFiles(RootPath, manifest.GetRelatedSearchFileName()))
         {
            if (regEx.Match(Path.GetFileName(relatedBuild)).Success)
               File.Delete(relatedBuild);
         }


         var targetPath = Path.Combine(RootPath, manifest.GetFileName());
         if (File.Exists(targetPath))
            File.Delete(targetPath);

         File.Move(tempFile, targetPath);
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

         if (filePattern == null)
            return new NuGet.Versioning.NuGetVersion[0];

         return new DirectoryInfo(RootPath).GetFiles(filePattern)
            .Select(i => PackageExtensions.GetPackageKeyFromFileName(i.Name).Version).ToArray();
      }

      public PackageManifest GetManifest(PackageKey key)
      {
         var fullPath = Path.Combine(RootPath, key.GetFileName());

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
