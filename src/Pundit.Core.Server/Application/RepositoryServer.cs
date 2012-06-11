using System;
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

      public RepositoryServer(IPackageRepository pr, string rootDir)
      {
         if (pr == null) throw new ArgumentNullException("pr");
         _pr = pr;

         Initialize(rootDir);
      }

      private void Initialize(string rootDirOpt)
      {
         string rootDir = rootDirOpt ?? AppDomain.CurrentDomain.BaseDirectory;
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

      private PackageSnapshotKey ReadPackageSnapshotKey(IDataReader reader)
      {
         long modType = (long) reader["ModType"];
         long manifestId = (long) reader["PackageManifestId"];

         var diff = (SnapshotPackageDiff)modType;
         Package manifest = _pr.GetPackage(manifestId);
         
         return new PackageSnapshotKey(manifest, diff);
      }

      public RemoteSnapshot GetSnapshot(string changeId)
      {
         /*_log.Debug("snapshot requested for changeId [" + changeId + "]");

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

         return new RemoteSnapshot(isDelta, keys, nextChangeId == 0 ? null : nextChangeId.ToString());*/
         throw new NotImplementedException();
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

      #region Implementation of IDisposable

      public void Dispose()
      {
         _pr.Dispose();
      }

      #endregion
   }
}
