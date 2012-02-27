using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Data.SQLite;
using System.Text;
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
         return _sql.ExecuteScalar<long>("PackageBinary", "PackageBinaryId",
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
         

         var bin = new SQLiteParameter(DbType.Binary);
         byte[] binBytes = File.ReadAllBytes(filePath);
         bin.Value = binBytes;

         return _sql.Insert("PackageBinary",
                            new[] {"PackageId", "Version", "Platform", "Data", "Size"},
                            new object[] {key.PackageId, key.VersionString, key.Platform, bin, (long)binBytes.Length});
      }

      private long GetLocalRepositoryId()
      {
         long id = _sql.ExecuteScalar<long>("Repository", "RepositoryId", new[] {"Tag=(?)"},
                                            LocalConfiguration.LocalRepositoryTag);

         if (id == 0)
         {
            id = _sql.Insert("Repository", new[] {"Tag", "Uri"},
                             LocalConfiguration.LocalRepositoryTag,
                             LocalConfiguration.LocalRepositoryUri);
         }

         return id;
      }

      public void Put(Stream packageStream)
      {
         string tempFile = Path.Combine(Path.GetTempPath(), TempFilePrefix + Guid.NewGuid());

         try
         {
            using (IDbTransaction tran = _sql.BeginTransaction())
            {
               //download file
               using (Stream ts = File.Create(tempFile))
               {
                  packageStream.CopyTo(ts);
               }

               //validate manifest
               Package manifest;
               using (Stream ts = File.OpenRead(tempFile))
               {
                  using (var pr = new PackageReader(ts))
                  {
                     manifest = pr.ReadManifest();
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
         using(IDbCommand cmd = _sql.CreateCommand())
         {
            cmd.CommandText = "select Data from PackageBinary where PackageId=(?) and Version=(?) and Platform=(?) LIMIT 1";
            cmd
               .Add(key.PackageId)
               .Add(key.VersionString)
               .Add(key.Platform);

            using(IDataReader reader = cmd.ExecuteReader())
            {
               if (!reader.Read()) throw new FileNotFoundException("package not found: " + key);

               byte[] data = (byte[])reader["Data"];

               return new MemoryStream(data);
            }
         }
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

      private Package ReadPackage(IDataReader reader, out long dbId)
      {
         dbId = reader.AsLong("PackageManifestId");

         return new Package(reader.AsString("PackageId"), new Version(reader.AsString("Version")))
                   {
                      Platform = reader.AsString("Platform"),
                      ProjectUrl = reader.AsString("HomeUrl"),
                      Author = reader.AsString("Author"),
                      Description = reader.AsString("Description"),
                      ReleaseNotes = reader.AsString("ReleaseNotes"),
                      License = reader.AsString("License")
                   };
      }

      private PackageDependency ReadDependency(IDataReader reader)
      {
         return new PackageDependency(reader.AsString("PackageId"), reader.AsString("VersionPattern"))
                   {
                      Platform = reader.AsString("Platform"),
                      Scope = (DependencyScope)reader.AsLong("Scope"),
                      CreatePlatformFolder = reader.AsBool("CreatePlatformFolder")
                   };
      }

      public Package GetManifest(PackageKey key)
      {
         long dbId;
         Package root;

         using(IDataReader reader = _sql.ExecuteReader("PackageManifest", null,
            new[] { "PackageId=(?)", "Version=(?)", "Platform=(?)" },
            new object[] {key.PackageId, key.Version.ToString(), key.Platform}))
         {
            if (reader.Read())
            {
               root = ReadPackage(reader, out dbId);
            }
            else
            {
               throw new FileNotFoundException("package " + key + " not found");
            }
         }

         using(IDataReader reader = _sql.ExecuteReader("PackageDependency", null,
            new[] {"PackageManifestId=(?)"}, new object[] {dbId}))
         {
            while(reader.Read())
            {
               root.Dependencies.Add(ReadDependency(reader));
            }
         }

         return root;
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
         var txt = new StringBuilder();
         txt.Append("select RepositoryId from PackageManifest ");
         txt.Append("where PackageId=(?) and Version=(?) and Platform=(?) ");
         txt.Append("order by RepositoryId asc limit 0,1");

         using(IDbCommand cmd = _sql.CreateCommand())
         {
            cmd.CommandText = txt.ToString();
            cmd.Add(key.PackageId);
            cmd.Add(key.VersionString);
            cmd.Add(key.Platform);

            object repoId = cmd.ExecuteScalar();

            return (repoId is long) ? (long) repoId : 0;
         }
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
