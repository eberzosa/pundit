using System;
using System.Configuration;
using System.IO;
using Pundit.Core.Model;
using Pundit.Core.Utils;
using log4net;

namespace Pundit.Core.Server.Application
{
   /// <summary>
   /// Saves and retrieves packages binary data.
   /// </summary>
   class StreamsProvider
   {
      private readonly string _rootDir;
      private readonly ILog _log = LogManager.GetLogger(typeof (StreamsProvider));
      private readonly int _maxRevisions;

      public StreamsProvider(string rootDir)
      {
         if (rootDir == null) throw new ArgumentNullException("rootDir");

         _rootDir = rootDir;

         string s = ConfigurationManager.AppSettings["Pundit.Server.Store.MaxRevisions"];
         if (!int.TryParse(s, out _maxRevisions)) _maxRevisions = 10;
      }

      public Stream Read(PackageKey key)
      {
         string sourceDir = ResolveFolder(key, false);
         string sourceFile = null;
         if (sourceDir != null) sourceFile = Path.Combine(sourceDir, PackageUtils.GetFileName(key));            
         if(sourceFile == null || !File.Exists(sourceFile)) throw new FileNotFoundException("package does not exist: " + key);

         return File.OpenRead(sourceFile);
      }

      public void Delete(PackageKey key)
      {
         string sourceDir = ResolveFolder(key, false);
         if(sourceDir != null)
         {
            string sourceFile = Path.Combine(sourceDir, PackageUtils.GetFileName(key));
            if(File.Exists(sourceFile))
            {
               try
               {
                  File.Delete(sourceFile);
               }
               catch
               {
               }
            }
         }
      }

      public void Save(PackageKey key, Stream data)
      {
         string targetDir = ResolveFolder(key, true);
         string targetFile = Path.Combine(targetDir, PackageUtils.GetFileName(key));

         _log.Debug("saving package to " + targetFile);

         using(Stream s = File.OpenWrite(targetFile))
         {
            data.CopyTo(s);
         }
      }

      private string ResolveFolder(PackageKey key, bool createIfMissing)
      {
         string folder = Path.Combine(_rootDir, key.PackageId);

         if(!Directory.Exists(folder))
         {
            if (createIfMissing) Directory.CreateDirectory(folder);
            else return null;
         }

         folder = Path.Combine(folder, key.Platform);

         if(!Directory.Exists(folder))
         {
            if (createIfMissing) Directory.CreateDirectory(folder);
            else return null;
         }

         return folder;
      }
   }
}
