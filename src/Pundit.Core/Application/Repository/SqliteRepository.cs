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

      public void Publish(Stream packageStream)
      {
         string tempFile = Path.Combine(_absoluteDir, "download-" + Guid.NewGuid());

         try
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
               byte[] binBytes = File.ReadAllBytes(tempFile);
               bin.Value = binBytes;

               cmd.Parameters.Add(packageId);
               cmd.Parameters.Add(version);
               cmd.Parameters.Add(platform);
               cmd.Parameters.Add(bin);

               cmd.ExecuteNonQuery();
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
            cmd.Parameters.Add(new SQLiteParameter(DbType.String, (object) key.PackageId));
            cmd.Parameters.Add(new SQLiteParameter(DbType.String, (object) key.VersionString));
            cmd.Parameters.Add(new SQLiteParameter(DbType.String, (object) key.Platform));

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
         throw new NotImplementedException();
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
