using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Application.Repository
{
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

      public PackageSnapshotKey[] GetSnapshot(string changeId, out string nextChangeId)
      {
         throw new NotImplementedException();
      }
   }
}
