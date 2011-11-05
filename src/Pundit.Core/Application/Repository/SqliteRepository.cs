using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Data.SQLite;
using Pundit.Core.Application.Sqlite;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Application.Repository
{
   public class SqliteRepository : IRepository, IDisposable
   {
      private const string LocalRepoName = "local";
      private const string TempFilePrefix = "pundit-download-part-";

      private readonly SqliteHelper _sql;
      private long _repoId;

      public SqliteRepository(string dbPath)
      {
         _sql = new SqliteHelper(dbPath);
         _repoId = GetLocalRepositoryId();
      }

      private long WriteBinaryStream(string filePath, Package manifest)
      {
         var bin = new SQLiteParameter(DbType.Binary);
         byte[] binBytes = File.ReadAllBytes(filePath);
         bin.Value = binBytes;

         return _sql.Insert("PackageBinary",
                            new[] {"PackageId", "Version", "Platform", "Data"},
                            new object[] {manifest.PackageId, manifest.VersionString, manifest.Platform, bin});
      }

      private long GetLocalRepositoryId()
      {
         long id = -1;
         using(IDbCommand cmd = _sql.CreateCommand())
         {
            cmd.CommandText = "select RepositoryId from Repository where Tag='local'";
            object objid = cmd.ExecuteScalar();
            if (objid != null) id = (long) objid;
         }

         if(id == -1)
         {
            id = _sql.Insert("Repository", new[] {"Tag"}, new[] {LocalRepoName});
         }

         return id;
      }

      private long WriteManifest(Package manifest)
      {
         long manifestId = _sql.Insert("PackageManifest",
                                       new[]
                                          {
                                             "RepositoryId", "PackageId", "Version", "Platform", "HomeUrl", "Author",
                                             "Description", "ReleaseNotes", "License"
                                          },
                                       new object[]
                                          {
                                             _repoId, manifest.PackageId, manifest.VersionString,
                                             manifest.Platform, manifest.ProjectUrl, manifest.Author,
                                             manifest.Description, manifest.ReleaseNotes, manifest.License
                                          });

         foreach(PackageDependency dependency in manifest.Dependencies)
         {
            long depId = _sql.Insert("PackageDependency",
                                     new[]
                                        {
                                           "PackageManifestId", "PackageId", "VersionPattern", "Platform", "Scope",
                                           "CreatePlatformFolder"
                                        },
                                     new object[]
                                        {
                                           manifestId, dependency.PackageId, dependency.VersionPattern,
                                           dependency.Platform, (long) dependency.Scope
                                        });
         }

         return manifestId;
      }

      public void Publish(Stream packageStream)
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
         using(IDbCommand cmd = _sql.CreateCommand())
         {
            cmd.CommandText = "select Data from PackageBinary where PackageId=(?) and Version=(?) and Platform=(?)";
            cmd
               .Add(key.PackageId)
               .Add(key.VersionString)
               .Add(key.Platform);

            using(IDataReader reader = cmd.ExecuteReader())
            {
               if (!reader.Read()) throw new FileNotFoundException("package not found");

               byte[] data = (byte[])reader["Data"];

               return new MemoryStream(data);
            }
         }
      }

      public Version[] GetVersions(UnresolvedPackage package, VersionPattern pattern)
      {
         var r = new HashSet<Version>();

         using (IDataReader reader = _sql.ExecuteReader("PackageManifest",
            new[] { "Version" },
            new[] { "PackageId=(?)", "Platform=(?)", "RepositoryId=(?)" },
            new object[] { package.PackageId, package.Platform, _repoId }))
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
            new[] { "RepositoryId=(?)", "PackageId=(?)", "Version=(?)", "Platform=(?)" },
            new object[] {_repoId, key.PackageId, key.Version.ToString(), key.Platform}))
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

      public bool[] PackagesExist(PackageKey[] packages)
      {
         if (packages == null) return null;

         bool[] r = new bool[packages.Length];

         for (int i = 0; i < packages.Length; i++ )
         {
            long id = _sql.ExecuteScalar<long>("PackageManifest", "PackageManifestId",
                                               new[] {"RepositoryId=(?)", "PackageId=(?)", "Version=(?)", "Platform=(?)"},
                                               _repoId, packages[i].PackageId, packages[i].Version.ToString(),
                                               packages[i].Platform);

            r[i] = id != 0;
         }

         return r;
      }

      public PackageKey[] Search(string substring)
      {
         var r = new HashSet<PackageKey>();

         using(IDataReader reader = _sql.ExecuteReader("PackageManifest", null,
            new[] { "RepositoryId=(?)", "PackageId like (?)"}, _repoId,
            "%" + substring + "%"))
         {
            while(reader.Read())
            {
               r.Add(new PackageKey(reader.AsString("PackageId"),
                                    new Version(reader.AsString("Version")),
                                    reader.AsString("Platform")));
            }
         }

         return r.ToArray();
      }

      public void Dispose()
      {
         _sql.Dispose();
      }
   }
}
