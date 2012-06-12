using System;
using System.Configuration;
using System.Data;
using System.IO;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Core.Server.Model;
using log4net;

namespace Pundit.Core.Server.Application
{
   class RepositoryServer : IRemoteRepository
   {
      private readonly IPackageRepository _pr;

      private readonly ILog _log = LogManager.GetLogger(typeof (RepositoryServer));
      private StreamsProvider _streams;

      public RepositoryServer(IPackageRepository pr) : this(pr, null)
      {
         
      }

      public RepositoryServer(IPackageRepository pr, string rootDir)
      {
         if (pr == null) throw new ArgumentNullException("pr");
         _pr = pr;

         Initialize(rootDir);
      }

      private void Initialize(string rootDirOpt)
      {
         string rootDir = rootDirOpt ?? ConfigurationManager.AppSettings["FilesRootDir"];
         if(!Directory.Exists(rootDir)) throw new DirectoryNotFoundException("repository root (" + rootDir + ") does not exist");
         string dataLocation = Path.Combine(rootDir, "files");
         if (!Directory.Exists(dataLocation)) Directory.CreateDirectory(dataLocation);

         _log.Info("data location: " + dataLocation);
         _streams = new StreamsProvider(dataLocation);
      }

      public void Publish(Stream packageStream)
      {
         try
         {
            _log.Debug("downloading package for publishing...");
            string tmpFile = DownloadToTemp(packageStream);

            try
            {
               var tmpInfo = new FileInfo(tmpFile);
               _log.Debug("downloaded, size: " + tmpInfo.Length + ", reading package...");

               Package p = GetManifest(tmpFile);
               _log.Debug("package read, " + p.Key + ", saving binary...");

               using (Stream fs = File.OpenRead(tmpFile))
               {
                  _streams.Save(p.Key, fs);
               }

               _log.Debug("binary saved, persisting metadata...");

               /*PackageKey oldKey = PersistPublish(p);
               if(oldKey != null)
               {
                  _streams.Delete(oldKey);
               }*/
               _log.Debug("success");
            }
            finally
            {
               try
               {
                  File.Delete(tmpFile);
               }
               catch
               {

               }
            }
         }
         catch(Exception ex)
         {
            _log.Error("failed to publish", ex);
            throw;
         }
      }

      public Stream Download(string platform, string packageId, string version)
      {
         return Download(new PackageKey(packageId, new Version(version), platform));
      }

      private Stream Download(PackageKey key)
      {
         if (key == null) throw new ArgumentNullException("key");
         if (!_pr.Exists(key)) throw new FileNotFoundException(string.Format(Ex.RepositoryServer_NoPackageInIndex, key));
         return _streams.Read(key);
      }

      public RemoteSnapshot GetSnapshot(string changeId)
      {
         var p = new Package("fakery", new Version(1, 2, 3, 4));
         var result = new RemoteSnapshot(true, new[] {new PackageSnapshotKey(p, SnapshotPackageDiff.Add)}, "123");
         return result;
      }

      private string DownloadToTemp(Stream httpStream)
      {
         string tmpFile = Path.GetTempFileName();

         try
         {
            using (Stream s = File.OpenWrite(tmpFile))
            {
               httpStream.CopyTo(s);
            }
         }
         catch
         {
            try
            {
               File.Delete(tmpFile);
            }
            catch
            {
               
            }

            throw;
         }

         return tmpFile;
      }

      private Package GetManifest(string filePath)
      {
         using (Stream fs = File.OpenRead(filePath))
         {
            Package p;

            using (var rdr = new PackageReader(fs))
            {
               p = rdr.Manifest;
            }

            return p;
         }
      }

      #region Implementation of IDisposable

      public void Dispose()
      {
         _pr.Dispose();
      }

      #endregion
   }
}
