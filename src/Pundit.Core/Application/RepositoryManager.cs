using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using Pundit.Core.Application.Repository;
using Pundit.Core.Application.Sqlite;
using Pundit.Core.Model;

namespace Pundit.Core.Application
{
   class RepositoryManager : IRepositoryManager
   {
      private const string RepositoryTableName = "Repository";
      private const string ManifestTableName = "PackageManifest";

      private readonly SqliteHelper _sql;
      private ILocalRepository _localRepo;

      public RepositoryManager(SqliteHelper sql)
      {
         if (sql == null) throw new ArgumentNullException("sql");

         _sql = sql;

         Initialize();
      }

      public RepositoryManager(string dbPath)
      {
         _sql = new SqliteHelper(dbPath);

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
            UseForPublishing = reader.AsBool("UseForPublishing")
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

      public IEnumerable<Repo> PublishingRepositories
      {
         get { return ActiveRepositories.Where(r => r.UseForPublishing); }
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

         if(newRepo.Tag == null) throw new ArgumentNullException("newRepo", "Tag is required");
         if(newRepo.Uri == null) throw new ArgumentNullException("newRepo", "Uri is required");

         if(AllRepositories.Any(r => r.Tag == newRepo.Tag))
            throw new ApplicationException("repository '" + newRepo.Tag + "' already registered");

         if(AllRepositories.Any(r => r.Uri == newRepo.Uri))
            throw new ApplicationException("there is already repository with the same Uri registered");

         long repoId = _sql.Insert(RepositoryTableName,
                                   new[]
                                      {
                                         "Tag", "Uri", "RefreshIntervalHours", "LastRefreshed", "LastChangeId",
                                         "IsEnabled",
                                         "UseForPublishing"
                                      },
                                   newRepo.Tag, newRepo.Uri, (long)newRepo.RefreshIntervalInHours, newRepo.LastRefreshed,
                                   newRepo.LastChangeId, newRepo.IsEnabled, newRepo.UseForPublishing);

         return GetRepositoryById(repoId);
      }

      public void Unregister(long repoId)
      {
         _sql.DeleteRecord(RepositoryTableName, repoId);
      }

      public void Update(Repo repo)
      {
         using(IDbCommand cmd = _sql.CreateCommand())
         {
            cmd.CommandText = "update " + RepositoryTableName + " set " +
                              "RefreshIntervalHours=(?), IsEnabled=(?), UseForPublishing=(?) where RepositoryId=(?)";
            cmd.Add(repo.RefreshIntervalInHours).Add(repo.IsEnabled).Add(repo.UseForPublishing);
            cmd.Add(repo.Id);
            cmd.ExecuteNonQuery();
         }
      }

      private void DeleteManifest(long repoId, PackageKey key)
      {
         using (IDbCommand cmd = _sql.CreateCommand())
         {
            cmd.CommandText = "delete from " + ManifestTableName + " where RepositoryId=(?) and " +
                              "PackageId=(?) and Version=(?) and Platform=(?)";
            cmd.Add(repoId);
            cmd.Add(key.PackageId).Add(key.Version.ToString()).Add(key.Platform);
            cmd.ExecuteNonQuery();
         }
      }

      public void PlaySnapshot(Repo repo, IEnumerable<PackageSnapshotKey> snapshot, string nextChangeId)
      {
         if (repo == null) throw new ArgumentNullException("repo");

         if(snapshot != null)
         {
            using (IDbTransaction tran = _sql.BeginTransaction())
            {
               if(nextChangeId == null)
               {
                  using(IDbCommand cmd = _sql.CreateCommand())
                  {
                     cmd.CommandText = "delete from " + ManifestTableName + " where RepositoryId=(?)";
                     cmd.Add(repo.Id);
                     cmd.ExecuteNonQuery();
                  }
               }

               foreach (PackageSnapshotKey entry in snapshot)
               {
                  switch (entry.Diff)
                  {
                     case DiffType.Add:
                        _sql.WriteManifest(repo.Id, entry.Manifest);
                        break;
                     case DiffType.Mod:
                        DeleteManifest(repo.Id, entry.Manifest.Key);
                        _sql.WriteManifest(repo.Id, entry.Manifest);
                        break;
                     case DiffType.Del:
                        DeleteManifest(repo.Id, entry.Manifest.Key);
                        break;
                  }
               }

               using(IDbCommand cmd = _sql.CreateCommand())
               {
                  cmd.CommandText = "update " + RepositoryTableName + " set LastRefreshed=(?), LastChangeId=(?) " +
                                    "where RepositoryId=(?)";
                  cmd.Add(DateTime.Now).Add(nextChangeId);
                  cmd.Add(repo.Id);
                  cmd.ExecuteNonQuery();
               }

               tran.Commit();
            }
         }
      }
   }
}
