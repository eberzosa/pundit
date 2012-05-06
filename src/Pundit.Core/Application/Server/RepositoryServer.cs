using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Pundit.Core.Application.Sqlite;
using Pundit.Core.Model;
using log4net;

namespace Pundit.Core.Application.Server
{
   class RepositoryServer : IRemoteRepository
   {
      private const string ManifestTableName = "PackageManifest";
      private const string HistoryTableName = "ManifestHistory";

      private readonly ILog _log = LogManager.GetLogger(typeof (RepositoryServer));
      private SqliteHelper _sql;
      private StreamsProvider _streams;

      //MUST have old-style parameterless constructor for IoC and WCF
      public RepositoryServer() : this(null)
      {
         
      }

      public RepositoryServer(string rootDir)
      {
         Initialize(rootDir);
      }

      private void Initialize(string rootDirOpt)
      {
         string rootDir = rootDirOpt ?? AppDomain.CurrentDomain.BaseDirectory;
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
         if (key == null) throw new ArgumentNullException("key");

         //check that package exists in the index first
         long count = _sql.ExecuteScalar<long>(ManifestTableName, "count(*)",
                                               new[] {"PackageId=(?)", "Platform=(?)", "Version=(?)"},
                                               key.PackageId, key.Platform, key.VersionString);
         if (count == 0) throw new FileNotFoundException(string.Format(Ex.RepositoryServer_NoPackageInIndex, key));

         return _streams.Read(key);
      }

      private PackageSnapshotKey ReadPackageSnapshotKey(IDataReader reader)
      {
         long modType = (long) reader["ModType"];
         long manifestId = (long) reader["PackageManifestId"];

         var diff = (SnapshotPackageDiff)modType;
         Package manifest = _sql.GetManifest(manifestId);
         
         return new PackageSnapshotKey(manifest, diff);
      }

      private long GetChangeId(string changeId)
      {
         long inputChangeId;
         if(long.TryParse(changeId, out inputChangeId))
         {
            long dbid = _sql.ExecuteScalar<long>(HistoryTableName, "ManifestHistoryId",
                                                 new[] {"ManifestHistoryId=(?)"}, inputChangeId);

            if (dbid > 0) return dbid;
         }

         return -1;
      }

      public RemoteSnapshot GetSnapshot(string changeId)
      {
         _log.Debug("snapshot requested for changeId [" + changeId + "]");

         long internalChangeId = GetChangeId(changeId);
         bool isDelta = internalChangeId != -1;
         long nextChangeId = 0;

         var keys = new List<PackageSnapshotKey>();
         using (IDataReader reader = _sql.ExecuteReader(HistoryTableName,
            new[] { "ManifestHistoryId", "ModType", "ModTime", "PackageManifestId" },
            new[] { "ManifestHistoryId > (?)" },
            internalChangeId == -1 ? 0 : internalChangeId))
         {
            while(reader.Read())
            {
               isDelta = true;
               nextChangeId = reader.AsLong("ManifestHistoryId");
               keys.Add(ReadPackageSnapshotKey(reader));
            }
         }

         return new RemoteSnapshot(isDelta, keys, nextChangeId == 0 ? null : nextChangeId.ToString());
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

      #region Implementation of IDisposable

      public void Dispose()
      {
         _sql.Dispose();
      }

      #endregion
   }
}
