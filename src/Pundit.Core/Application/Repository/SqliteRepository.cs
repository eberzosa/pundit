using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Data.SQLite;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Application.Repository
{
   public class SqliteRepository : IRepository, IDisposable
   {
      private const string SelectId = ";select last_insert_rowid()";

      public const string UriPrefix = "sqlite://";
      private string _absolutePath;
      private string _absoluteDir;
      private SQLiteConnection _conn;

      public SqliteRepository(string repoPath)
      {
         _absolutePath = repoPath.Substring(UriPrefix.Length);
         _absoluteDir = new FileInfo(_absolutePath).Directory.FullName;
      }

      private string GetConnectionString()
      {
         if(!File.Exists(_absolutePath))
         {
            using(Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(SqliteRepository).Namespace + ".pundit.db"))
            {
               using(Stream tgt = File.Create(_absolutePath))
               {
                  s.CopyTo(tgt);
               }
            }
         }

         return "Data Source=" + _absolutePath;
      }

      private SQLiteConnection Connection
      {
         get
         {
            if(_conn == null)
            {
               _conn = new SQLiteConnection(GetConnectionString());
               _conn.Open();
            }

            return _conn;
         }
      }

      private void WriteBinaryStream(string filePath, Package manifest)
      {
         //write to db
         using (SQLiteCommand cmd = Connection.CreateCommand())
         {
            cmd.CommandText =
               "insert into [PackageBinary] (PackageId, Version, Platform, Data) values ((?), (?), (?), (?))";

            SQLiteParameter packageId = new SQLiteParameter(DbType.String);
            SQLiteParameter version = new SQLiteParameter(DbType.String);
            SQLiteParameter platform = new SQLiteParameter(DbType.String);
            SQLiteParameter bin = new SQLiteParameter(DbType.Binary);
            packageId.Value = manifest.PackageId;
            version.Value = manifest.VersionString;
            platform.Value = manifest.Platform;
            byte[] binBytes = File.ReadAllBytes(filePath);
            bin.Value = binBytes;

            cmd.Parameters.Add(packageId);
            cmd.Parameters.Add(version);
            cmd.Parameters.Add(platform);
            cmd.Parameters.Add(bin);

            cmd.ExecuteNonQuery();
         }
      }

      private long GetLocalRepositoryId()
      {
         long id = -1;
         using(SQLiteCommand cmd = Connection.CreateCommand())
         {
            cmd.CommandText = "select RepositoryId from Repository where Tag='local'";
            object objid = cmd.ExecuteScalar();
            if (objid != null) id = (long) objid;
         }

         if(id == -1)
         {
            using(SQLiteCommand cmd = Connection.CreateCommand())
            {
               cmd.CommandText = "insert into Repository (Tag) values('local')" + SelectId;
               object objid = cmd.ExecuteScalar();
               id = (long) objid;
            }
         }

         return id;
      }

      private long WriteManifest(Package manifest)
      {
         long manifestId;

         using(SQLiteCommand cmd = Connection.CreateCommand())
         {
            cmd.CommandText = "insert into PackageManifest " +
                              "(RepositoryId, PackageId, Version, Platform, HomeUrl, Author, Description, ReleaseNotes, License) " +
                              "values ((?), (?), (?), (?), (?), (?), (?), (?), (?))" + SelectId;

            cmd
               .Add(GetLocalRepositoryId())
               .Add(manifest.PackageId)
               .Add(manifest.VersionString)
               .Add(manifest.Platform)
               .Add(manifest.ProjectUrl)
               .Add(manifest.Author)
               .Add(manifest.Description)
               .Add(manifest.ReleaseNotes)
               .Add(manifest.ReleaseNotes);

            manifestId = (long) cmd.ExecuteScalar();
         }

         foreach(PackageDependency dependency in manifest.Dependencies)
         {
            using(SQLiteCommand cmd = Connection.CreateCommand())
            {
               cmd.CommandText =
                  "insert into PackageDependency (PackageManifestId, PackageId, VersionPattern, Platform, Scope, CreatePlatformFolder) " +
                  "values ((?), (?), (?), (?), (?), (?))";
               cmd
                  .Add(manifestId)
                  .Add(dependency.PackageId)
                  .Add(dependency.VersionPattern)
                  .Add(dependency.Platform)
                  .Add((long) dependency.Scope)
                  .Add(dependency.CreatePlatformFolder);
               cmd.ExecuteNonQuery();
            }
         }

         return manifestId;
      }

      public void Publish(Stream packageStream)
      {
         string tempFile = Path.Combine(_absoluteDir, "download-" + Guid.NewGuid());

         try
         {
            using (SQLiteTransaction tran = Connection.BeginTransaction())
            {
               //download file
               using (Stream ts = File.Create(tempFile))
               {
                  packageStream.CopyTo(ts);
               }

               //get manifest
               Package manifest;
               using (Stream ts = File.OpenRead(tempFile))
               {
                  using (var pr = new PackageReader(ts))
                  {
                     manifest = pr.ReadManifest();
                  }
               }

               WriteBinaryStream(tempFile, manifest);
               WriteManifest(manifest);

               tran.Commit();
            }
         }
         finally
         {
            if(File.Exists(tempFile))
            {
               try
               {
                  File.Delete(tempFile);
               }
               catch
               {
               }
            }
         }
      }

      public Stream Download(PackageKey key)
      {
         using(SQLiteCommand cmd = Connection.CreateCommand())
         {
            cmd.CommandText = "select Data from PackageBinary where PackageId=(?) and Version=(?) and Platform=(?)";
            cmd
               .Add(key.PackageId)
               .Add(key.VersionString)
               .Add(key.Platform);

            using(SQLiteDataReader reader = cmd.ExecuteReader())
            {
               if (!reader.Read()) throw new FileNotFoundException("package not found");

               byte[] data = (byte[])reader["Data"];

               return new MemoryStream(data);
            }
         }
      }

      public Version[] GetVersions(UnresolvedPackage package, VersionPattern pattern)
      {
         List<Version> r = new List<Version>();

         using(SQLiteCommand cmd = Connection.CreateCommand())
         {
            cmd.CommandText = "select distinct Version where PackageId=(?) and Platform=(?)";
            cmd.Add(package.PackageId).Add(package.Platform);

            using(SQLiteDataReader reader = cmd.ExecuteReader())
            {
               while(reader.Read())
               {
                  Version v = new Version((string) reader["Version"]);

                  if(pattern.Matches(v)) r.Add(v);
               }
            }
         }

         return r.ToArray();
      }

      public Package GetManifest(PackageKey key)
      {
         throw new NotImplementedException();
      }

      public bool[] PackagesExist(PackageKey[] packages)
      {
         throw new NotImplementedException();
      }

      public PackageKey[] Search(string substring)
      {
         throw new NotImplementedException();
      }

      public void Dispose()
      {
         if(_conn != null)
         {
            _conn.Dispose();
            _conn = null;
         }
      }
   }
}
