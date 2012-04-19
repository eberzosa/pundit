using System;
using System.Configuration;
using System.IO;
using Pundit.Core.Model;
using Pundit.Core.Utils;
using log4net;

namespace Pundit.Server
{
   /// <summary>
   /// Saves and retrieves packages binary data.
   /// </summary>
   internal class StreamsProvider
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

      public bool Exists(PackageKey key)
      {
         return false;
      }

      public Stream Read(PackageKey key)
      {
         return null;
      }

      public void Delete(PackageKey key)
      {
         
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
