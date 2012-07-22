using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Pundit.Core.Application.Repository;
using Pundit.Core.Application.Sqlite;
using Pundit.Core.Model;

namespace Pundit.Core.Application
{
   class SqlRepositoryManager : IRepositoryManager
   {
      private const string RepositoryTableName = "Repository";
      private const string ManifestTableName = "PackageManifest";

      private readonly SqliteHelper _sql;
      private ILocalRepository _localRepo;

      public SqlRepositoryManager(SqliteHelper sql)
      {
         if (sql == null) throw new ArgumentNullException("sql");

         _sql = sql;

         Initialize();
      }

      public SqlRepositoryManager(string dbPath)
      {
         _sql = new SqliteHelper(dbPath, "pundit");

         Initialize();
      }

      private void Initialize()
      {
         _localRepo = new SqliteLocalRepository(_sql.DataSource);
      }

      private Repo ReadRepository(IDataReader reader)
      {
         return new Repo(reader.AsLong("RepositoryId"), reader.AsString("Tag"), reader.AsString("Uri"))
         {
            RefreshIntervalInHours = (int)reader.AsLong("RefreshIntervalHours"),
            LastRefreshed = reader.AsDateTime("LastRefreshed"),
            LastChangeId = reader.AsString("LastChangeId"),
            IsEnabled = reader.AsBool("IsEnabled"),
            Login = reader.AsString("Login"),
            ApiKey = reader.AsString("ApiKey")
         };
      }

      public ILocalRepository LocalRepository
      {
         get { return _localRepo; }
      }

      public IEnumerable<Repo> AllRepositories
      {
         get
         {
            var r = new List<Repo>();
            using (IDataReader reader = _sql.ExecuteReader("Repository", null, null))
            {
               while (reader.Read())
               {
                  if (reader.AsString("Tag") != LocalConfiguration.LocalRepositoryTag)
                  {
                     r.Add(ReadRepository(reader));
                  }
               }
            }
            return r;
         }
      }

      public IEnumerable<Repo> ActiveRepositories
      {
         get { return AllRepositories.Where(r => r.IsEnabled); }
      }

      public Repo GetRepositoryByTag(string tag)
      {
         return ActiveRepositories.FirstOrDefault(r => r.Tag == tag);
      }

      public Repo GetRepositoryById(long id)
      {
         return ActiveRepositories.FirstOrDefault(r => r.Id == id);
      }

      public LocalStats Stats
      {
         get
         {
            LocalStats r = new LocalStats();
            r.OccupiedSpaceTotal = new FileInfo(_sql.DataSource).Length;
            r.OccupiedSpaceBinaries = _sql.ExecuteScalar<long>("PackageBinary", "sum(Size)", null);
            r.TotalManifestsCount = _sql.ExecuteScalar<long>(ManifestTableName, "count(*)", null);
            return r;
         }
      }

      public void ZapCache()
      {
         using(IDbCommand cmd = _sql.CreateCommand())
         {
            cmd.CommandText = "delete from PackageManifest where RepositoryId=1";
            cmd.ExecuteNonQuery();
         }

         using(IDbCommand cmd = _sql.CreateCommand())
         {
            cmd.CommandText = "delete from PackageBinary";
            cmd.ExecuteNonQuery();
         }

         using(IDbCommand cmd = _sql.CreateCommand())
         {
            cmd.CommandText = "VACUUM;";
            cmd.ExecuteNonQuery();
         }
      }

      public Repo Register(Repo newRepo)
      {
         if (newRepo == null) throw new ArgumentNullException("newRepo");
         if (!newRepo.IsEnabled) throw new ArgumentException("newRepo", "can't create inactive repository");
         if (newRepo.Tag == null) throw new ArgumentNullException("newRepo", "Tag is required");
         if (newRepo.Uri == null) throw new ArgumentNullException("newRepo", "Uri is required");

         if(AllRepositories.Any(r => r.Tag == newRepo.Tag))
            throw new ApplicationException("repository '" + newRepo.Tag + "' already registered");

         if(AllRepositories.Any(r => r.Uri == newRepo.Uri))
            throw new ApplicationException("there is already repository with the same Uri registered");

         long repoId = _sql.Insert(RepositoryTableName,
                                   new[]
                                      {
                                         "Tag", "Uri", "RefreshIntervalHours", "LastRefreshed", "LastChangeId",
                                         "IsEnabled"
                                      },
                                   newRepo.Tag, newRepo.Uri, (long)newRepo.RefreshIntervalInHours, newRepo.LastRefreshed,
                                   newRepo.LastChangeId, newRepo.IsEnabled);

         return GetRepositoryById(repoId);
      }

      public void Unregister(long repoId)
      {
         _sql.DeleteRecord(RepositoryTableName, repoId);
      }

      public void Update(Repo repo)
      {
         _sql.Update(
            RepositoryTableName,
            new[] { "RefreshIntervalHours", "IsEnabled", "Login", "ApiKey"},
            new object[] { repo.RefreshIntervalInHours, repo.IsEnabled, repo.Login, repo.ApiKey},
            new[] { "RepositoryId=?P5"},
            repo.Id);
      }

      private void DeleteManifest(long repoId, PackageKey key)
      {
         _sql.Delete(
            ManifestTableName,
            new[] { "RepositoryId=(?)", "PackageId=(?)", "Version=(?)", "Platform=(?)" },
            repoId, key.PackageId, key.VersionString, key.Platform);
      }

      public void PlaySnapshot(Repo repo, RemoteSnapshot snapshot)
      {
         if (repo == null) throw new ArgumentNullException("repo");

         using (IDbTransaction tran = _sql.BeginTransaction())
         {
            if (snapshot != null && snapshot.Changes != null && snapshot.Changes.Length > 0)
            {
               if (!snapshot.IsDelta)
               {
                  _sql.Delete(ManifestTableName, new[] {"RepositoryId=(?)"}, repo.Id);
               }

               foreach (PackageSnapshotKey entry in snapshot.Changes)
               {
                  switch (entry.Diff)
                  {
                     case SnapshotPackageDiff.Add:
                        _sql.WriteManifest(repo.Id, entry.Manifest);
                        break;
                     case SnapshotPackageDiff.Del:
                        DeleteManifest(repo.Id, entry.Manifest.Key);
                        break;
                  }
               }
            }

            string nextChangeId = (snapshot != null && snapshot.NextChangeId != null)
                                     ? snapshot.NextChangeId
                                     : repo.LastChangeId;

            _sql.Update(RepositoryTableName,
                        new[] {"LastRefreshed", "LastChangeId"},
                        new object[] {DateTime.UtcNow, nextChangeId},
                        new[] {"RepositoryId=(?)"}, repo.Id);

            tran.Commit();
         }
      }

      #region Implementation of IDisposable

      public void Dispose()
      {
         _localRepo.Dispose();
         _sql.Dispose();
      }

      #endregion
   }
}
