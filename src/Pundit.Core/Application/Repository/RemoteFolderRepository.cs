using System;
using System.Collections.Generic;
using System.IO;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Application.Repository
{
   /// <summary>
   /// Extremely slow and ineffective directory-based remote repository.
   /// AVOID WHENEVER POSSIBLE!!!
   /// Does not support deltas.
   /// Will list all files in the remote folder and download each of them to unpack manifest.
   /// Still exist only for compatibility with ancient versions.
   /// </summary>
   class RemoteFolderRepository : IRemoteRepository
   {
      private readonly string _rootPath;

      public RemoteFolderRepository(string rootPath)
      {
         if (rootPath == null) throw new ArgumentNullException("rootPath");
         if (!Directory.Exists(rootPath))
            throw new ArgumentException("root directory [" + rootPath + "] does not exist", "rootPath");

         _rootPath = rootPath;
      }

      private static string GetBuildsSearchFilePattern(Package pkg)
      {
         pkg.Validate();

         return String.Format(PackageUtils.PackageFileNamePattern,
                              pkg.PackageId,
                              pkg.Version.Major, pkg.Version.Minor, pkg.Version.Build,
                              "*",
                              String.IsNullOrEmpty(pkg.Platform) ? "noarch" : pkg.Platform,
                              Package.PackedExtension);
      }


      private static string[] SearchAllRelatedBuilds(string sourceDirectory, Package pkg)
      {
         return Directory.GetFiles(sourceDirectory, GetBuildsSearchFilePattern(pkg));
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
         foreach(string rb in SearchAllRelatedBuilds(_rootPath, manifest))
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

      public PackageSnapshotKey[] GetSnapshot(string changeId, out string nextChangeId)
      {
         if(changeId != null) throw new NotSupportedException("deltas not supported, start from null");
         List<PackageSnapshotKey> r = new List<PackageSnapshotKey>();

         foreach(string path in Directory.GetFiles(_rootPath, "*" + Package.PackedExtension))
         {
            using(Stream s = File.OpenRead(path))
            {
               using (PackageReader rdr = new PackageReader(s))
               {
                  r.Add(new PackageSnapshotKey(rdr.ReadManifest()));
               }
            }
         }

         nextChangeId = null;
         return r.ToArray();
      }
   }
}
