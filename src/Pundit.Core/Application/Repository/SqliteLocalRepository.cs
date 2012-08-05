using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Community.CsharpSqlite.SQLiteClient;
using Pundit.Core.Application.Sqlite;
using Pundit.Core.Model;

namespace Pundit.Core.Application.Repository
{
   class SqliteLocalRepository : ILocalRepository, IDisposable
   {
      private const string TempFilePrefix = "pundit-download-part-";

      private readonly SqliteHelper _sql;
      private readonly long _repoId;

      public SqliteLocalRepository(string dbPath)
      {
         _sql = new SqliteHelper(dbPath, "pundit");
         _repoId = GetLocalRepositoryId();
      }

      private long GetBinaryId(PackageKey key)
      {
         return _sql.ExecuteScalar<int>("PackageBinary", "PackageBinaryId",
                                        new[] {"PackageId=(?)", "Version=(?)", "Platform=(?)"},
                                        key.PackageId, key.VersionString, key.Platform);
      }

      private long WriteBinaryStream(string filePath, PackageKey key)
      {
         //check if binary already exists
         long binaryId;

         do
         {
            binaryId = GetBinaryId(key);

            if (binaryId != 0) _sql.DeleteRecord("PackageBinary", binaryId);
         } while (binaryId != 0);
         

         var bin = new SqliteParameter("Data", DbType.Binary);
         byte[] binBytes = File.ReadAllBytes(filePath);
         bin.Value = binBytes;

         return _sql.Insert("PackageBinary",
                            new[] {"PackageId", "Version", "Platform", "Data", "Size"},
                            new object[] {key.PackageId, key.VersionString, key.Platform, bin, (long)binBytes.Length});
      }

      private long GetLocalRepositoryId()
      {
         int id = _sql.ExecuteScalar<int>("Repository", "RepositoryId", new[] {"Tag=(?)"},
                                            LocalConfiguration.LocalRepositoryTag);

         if (id == 0)
         {
            id = (int) _sql.Insert("Repository", new[] {"Tag", "Uri"},
                                   LocalConfiguration.LocalRepositoryTag,
                                   LocalConfiguration.LocalRepositoryUri);
         }

         return id;
      }

      public void Put(Stream packageStream, Action<long> readCallback)
      {
         string tempFile = Path.Combine(Path.GetTempPath(), TempFilePrefix + Guid.NewGuid());

         try
         {
            using (IDbTransaction tran = _sql.BeginTransaction())
            {
               //download file
               using (Stream ts = File.Create(tempFile))
               {
                  int read;
                  int readTotal = 0;
                  byte[] buffer = new byte[8000];
                  while((read = packageStream.Read(buffer, 0, buffer.Length)) != 0)
                  {
                     readTotal += read;
                     if (readCallback != null) readCallback(readTotal);
                     ts.Write(buffer, 0, read);
                  }
                  if (readCallback != null) readCallback(readTotal);
               }

               //validate manifest
               Package manifest;
               using (Stream ts = File.OpenRead(tempFile))
               {
                  using (var pr = new PackageReader(ts))
                  {
                     manifest = pr.Manifest;
                  }
               }
               manifest.Validate();

               WriteBinaryStream(tempFile, manifest.Key);
               _sql.WriteManifest(_repoId, manifest); //manifest is still needed for local-only packages

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

      public Stream Get(PackageKey key)
      {
         object sdata = _sql.ExecuteScalar<object>(
            "PackageBinary", "Data",
            new[] { "PackageId=(?)", "Version=(?)", "Platform=(?)" },
            key.PackageId, key.VersionString, key.Platform);

         byte[] data = (byte[]) sdata;

         return new MemoryStream(data);
      }

      public ICollection<Version> GetVersions(UnresolvedPackage package, VersionPattern pattern)
      {
         var r = new HashSet<Version>();

         using (IDataReader reader = _sql.ExecuteReader("PackageManifest",
            new[] { "Version" },
            new[] { "PackageId=(?)", "Platform=(?)" },
            new object[] { package.PackageId, package.Platform }))
         {
            while (reader.Read())
            {
               var v = new Version(reader.AsString("Version"));

               if (pattern.Matches(v)) r.Add(v);
            }
         }

         return r.ToArray();
      }

      public Package GetManifest(PackageKey key)
      {
         return _sql.GetManifest(key);
      }

      public ICollection<bool> BinariesExists(IEnumerable<PackageKey> packages)
      {
         if (packages == null) return null;

         var r = new List<bool>();
         
         foreach(PackageKey key in packages)
         {
            r.Add(GetBinaryId(key) != 0);
         }

         return r;
      }

      public long GetClosestRepositoryId(PackageKey key)
      {
         return _sql.ExecuteScalar<int>(
            "PackageManifest", "RepositoryId",
            new[] { "PackageId=(?)", "Version=(?)", "Platform=(?)" },
            key.PackageId, key.VersionString, key.Platform);
      }

      public ICollection<PackageKey> Search(string substring)
      {
         var r = new HashSet<PackageKey>();

         using(IDataReader reader = _sql.ExecuteReader("PackageManifest", null,
            new[] { "PackageId like (?)"},
            "%" + substring + "%"))
         {
            while(reader.Read())
            {
               r.Add(new PackageKey(reader.AsString("PackageId"),
                                    new Version(reader.AsString("Version")),
                                    reader.AsString("Platform")));
            }
         }

         return r;
      }

      public void Dispose()
      {
         _sql.Dispose();
      }
   }
}
