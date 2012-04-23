using System;
using System.Data;
using System.IO;
using Pundit.Core.Application;
using Pundit.Core.Application.Sqlite;
using Pundit.Core.Model;
using log4net;

namespace Pundit.Server
{
   public class RepositoryServer : IRemoteRepository
   {
      private readonly ILog _log = LogManager.GetLogger(typeof (RepositoryServer));
      private SqliteHelper _sql;
      private StreamsProvider _streams;

      public RepositoryServer()
      {
         Initialize();
      }

      private void Initialize()
      {
         string rootDir = AppDomain.CurrentDomain.BaseDirectory;
         string dbLocation = Path.Combine(rootDir, "meta.db");
         string dataLocation = Path.Combine(rootDir, "files");

         if (!Directory.Exists(dataLocation)) Directory.CreateDirectory(dataLocation);

         _log.Info("db location: " + dbLocation);
         _log.Info("data location: " + dataLocation);

         _sql = new SqliteHelper(dbLocation, "server");
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

               PersistPublish(p);
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
         return null;
      }

      public RemoteSnapshot GetSnapshot(string changeId)
      {
         _log.Debug("snapshot requested for changeId " + changeId);

         PackageSnapshotKey[] keys = new[] {new PackageSnapshotKey(new Package("test1", new Version("1.2.10")))};

         return new RemoteSnapshot {Changes = keys, NextChangeId = "testnext"};
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

            using (PackageReader rdr = new PackageReader(fs))
            {
               p = rdr.Manifest;
            }

            return p;
         }
      }

      private long WriteHistory(long manifestId, SnapshotPackageDiff diff)
      {
         return _sql.Insert("ManifestHistory",
                            new[] {"ModType", "ModTime", "PackageManifestId"},
                            (long) diff, DateTime.Now, manifestId);
      }

      private void PersistPublish(Package p)
      {
         using(IDbTransaction trans = _sql.BeginTransaction())
         {
            long manifestId = _sql.WriteManifest(p);
            _log.Debug("done (" + manifestId + "), writing history...");

            long historyId = WriteHistory(manifestId, SnapshotPackageDiff.Add);
            _log.Debug("done, history record id: " + historyId);

            trans.Commit();
         }
      }
   }
}
