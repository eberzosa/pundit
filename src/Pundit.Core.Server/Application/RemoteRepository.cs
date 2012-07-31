using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Core.Server.Model;
using log4net;

namespace Pundit.Core.Server.Application
{
   class RemoteRepository : IRemoteRepository
   {
      private readonly IPackageRepository _pr;

      private readonly ILog _log = LogManager.GetLogger(typeof (RemoteRepository));
      private StreamsProvider _streams;

      public RemoteRepository(IPackageRepository pr) : this(pr, null)
      {
         
      }

      public RemoteRepository(IPackageRepository pr, string rootDir)
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
               if (!_pr.Exists(p.Key))
               {
                  _log.Debug("package read, " + p.Key + ", saving binary...");

                  using (Stream fs = File.OpenRead(tmpFile))
                  {
                     _streams.Save(p.Key, fs);
                  }

                  _log.Debug("binary saved, persisting metadata...");
                  _pr.SavePackage(p, tmpInfo.Length, true);
                  _log.Debug("success");
               }
               else
               {
                  if(_log.IsDebugEnabled) _log.Debug("package already exists");
               }
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

      private RemoteSnapshot BuildSnapshot(IEnumerable<DbPackage> packages, string lastChangeId, bool isDelta, bool compress)
      {
         List<DbPackage> packagesList =
            packages == null
               ? null
               : (packages is List<DbPackage>
                     ? (List<DbPackage>)packages
                     : packages.ToList());
         IEnumerable<PackageSnapshotKey> keys =
            packagesList == null
               ? null
               : new List<PackageSnapshotKey>(packagesList.Select(p => new PackageSnapshotKey(p.Package, SnapshotPackageDiff.Add, p.Id.ToString())));
         string nextChangeId = (packagesList == null || packagesList.Count == 0)
                                  ? null
                                  : (packagesList[packagesList.Count - 1].Id + 1).ToString();

         return new RemoteSnapshot(isDelta, keys, nextChangeId ?? lastChangeId)
                   {
                      Count = packagesList == null ? 0 : packagesList.Count
                   };
      }

      private RemoteSnapshot GetNonDeltaSnapshot()
      {
         PackagesResult result = _pr.GetPackages(new PackagesQuery(-1, -1));
         return BuildSnapshot(result.Packages, null, false, false);
      }

      private RemoteSnapshot GetDeltaSnapshot(long firstRecordId)
      {
         IEnumerable<DbPackage> packages = _pr.ReadLog(firstRecordId, int.MaxValue, true);
         return BuildSnapshot(packages, firstRecordId.ToString(), true, true);
      }

      public RemoteSnapshot GetSnapshot(string changeId)
      {
         long id;
         long.TryParse(changeId, out id);
         if (id < 0) id = 0;
         bool isDelta = id > 0;

         return isDelta ? GetDeltaSnapshot(id) : GetNonDeltaSnapshot();
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
