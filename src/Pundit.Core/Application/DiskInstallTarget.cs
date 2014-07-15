using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pundit.Core.Model;

namespace Pundit.Core.Application
{
   /// <summary>
   /// Installs to filesystem
   /// </summary>
   class DiskInstallTarget : IInstallTarget
   {
      private readonly string _rootFolder;

      public DiskInstallTarget(string rootFolder)
      {
         if (rootFolder == null) throw new ArgumentNullException("rootFolder");
         if (!Directory.Exists(rootFolder)) throw new IOException(string.Format(Ex.DiskInstallTarget_NoRoot, rootFolder));

         _rootFolder = rootFolder;
      }

      public Stream CreateTargetStream(IEnumerable<string> destinationPath)
      {
         if (destinationPath == null) throw new ArgumentNullException("destinationPath");
         string[] parts = destinationPath.ToArray();
         if(parts.Length == 0) throw new ArgumentException(Ex.DiskInstallTarget_NoParts);
         string fullPath = _rootFolder;
         
         //check all directories exist
         for(int i = 0; i < parts.Length - 1; i++)
         {
            string part = parts[i];
            fullPath = Path.Combine(fullPath, part);
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
         }
         
         //create a return file stream
         fullPath = Path.Combine(fullPath, parts[parts.Length - 1]);
         if(File.Exists(fullPath)) File.Delete(fullPath);
         return File.Create(fullPath);
      }
   }
}
