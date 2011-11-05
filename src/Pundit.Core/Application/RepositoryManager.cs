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
      private readonly SqliteHelper _sql;
      private IRepository _localRepo;

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
         _localRepo = new SqliteRepository(_sql.DataSource);
      }

      private Repo ReadRepository(IDataReader reader)
      {
         return new Repo(reader.AsLong("RepositoryId"), reader.AsString("Tag"), reader.AsString("Uri"))
         {
            RefreshIntervalInHours = (int)reader.AsLong("RefreshIntervalHours"),
            LastRefreshed = reader.AsDateTime("LastRefreshed"),
            LastChangeId = reader.AsLong("LastChangeId"),
            IsEnabled = reader.AsBool("IsEnabled"),
            UseForPublishing = reader.AsBool("UseForPublishing")
         };
      }

      public IRepository LocalRepository
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
                  r.Add(ReadRepository(reader));
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

      public long OccupiedSpace
      {
         get { return new FileInfo(_sql.DataSource).Length; }
      }

      public long OccupiedBinarySpace
      {
         get { return _sql.ExecuteScalar<long>("PackageBinary", "sum(Size)", null); }
      }

      public void ZapBinarySpace()
      {
         throw new NotImplementedException();
      }

      public IEnumerable<PackageKey> SearchPackages(string substring)
      {
         throw new NotImplementedException();
      }
   }
}
