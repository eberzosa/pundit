using System;
using System.Data;
using System.IO;
using System.Reflection;
using Community.CsharpSqlite.SQLiteClient;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Application.Sqlite
{
   class SqliteHelper : SqlHelper
   {
      private const string ManifestTableName = "PackageManifest";
      private const string SelectId = ";select last_insert_rowid()";

      private readonly string _absolutePath;
      private readonly string _emptyDbResourceName;
      private readonly string _absoluteDir;
      private SqliteConnection _conn;

      public SqliteHelper(string dbPath, string emptyDbResourceName)
      {
         if (dbPath == null) throw new ArgumentNullException("dbPath");
         if (emptyDbResourceName == null) throw new ArgumentNullException("emptyDbResourceName");

         _absolutePath = dbPath;
         _emptyDbResourceName = typeof (SqliteHelper).Namespace + "." + emptyDbResourceName + ".db";
         _absoluteDir = Path.GetDirectoryName(_absolutePath);

         if(_absoluteDir == null)
            throw new ArgumentException("cannot get directory from path " + dbPath);

         if (!Directory.Exists(_absoluteDir))
            throw new DirectoryNotFoundException("target folder not found (" + _absoluteDir + ")");
      }

      public string DataSource
      {
         get { return _absolutePath; }
      }

      private string GetConnectionString()
      {
         if (!File.Exists(_absolutePath))
         {
            using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(_emptyDbResourceName))
            {
               if(null == s) throw new FileNotFoundException("resource not found: " + _emptyDbResourceName);

               using (Stream tgt = File.Create(_absolutePath))
               {
                  s.CopyTo(tgt);
               }
            }
         }

         return string.Format("Version=3,uri=file:{0}", _absolutePath);
      }

      protected override IDbConnection Connection
      {
         get
         {
            if (_conn == null)
            {
               _conn = new SqliteConnection(GetConnectionString());
               _conn.Open();

               using(IDbCommand cmd = _conn.CreateCommand())
               {
                  cmd.CommandText = "PRAGMA foreign_keys = ON;";
                  cmd.ExecuteNonQuery();
               }

               new FileInfo(_absolutePath).Attributes |= (FileAttributes.System | FileAttributes.Hidden);
            }

            return _conn;
         }
      }

      protected override void Add(IDbCommand cmd, object value, string name = null)
      {
         if (value is string)
            cmd.Parameters.Add(new SqliteParameter(name, DbType.String) { Value = value});
         else if (value is long || value is int)
            cmd.Parameters.Add(new SqliteParameter(name, DbType.Int32) { Value = value });
         else if (value is bool)
            cmd.Parameters.Add(new SqliteParameter(name, DbType.Boolean) { Value = value });
         else if (value is DateTime)
            cmd.Parameters.Add(new SqliteParameter(name, DbType.DateTime) { Value = value });
         else if (value == null)
            cmd.Parameters.Add(new SqliteParameter());
         else if (value is SqliteParameter)
            cmd.Parameters.Add(value);
         else throw new ArgumentException("type " + value.GetType() + " not supported");
      }

      #region [ Product Related ]

      public long WriteManifest(long repoId, Package manifest)
      {
         long manifestId = Insert("PackageManifest",
                                  new[]
                                     {
                                        "RepositoryId", "PackageId", "Version", "Platform", "HomeUrl", "Author",
                                        "Description", "ReleaseNotes", "License"
                                     },
                                  new object[]
                                     {
                                        repoId, manifest.PackageId, manifest.VersionString,
                                        manifest.Platform, manifest.ProjectUrl, manifest.Author,
                                        manifest.Description, manifest.ReleaseNotes, manifest.License
                                     });

         foreach (PackageDependency dependency in manifest.Dependencies)
         {
            long depId = Insert("PackageDependency",
                                new[]
                                   {
                                      "PackageManifestId", "PackageId", "VersionPattern", "Platform", "Scope",
                                      "TargetFolder"
                                   },
                                new object[]
                                   {
                                      manifestId, dependency.PackageId, dependency.VersionPattern,
                                      dependency.Platform, (long) dependency.Scope, dependency.TargetFolder
                                   });
         }

         return manifestId;
      }

      public long WriteManifest(Package manifest)
      {
         long manifestId = Insert("PackageManifest",
                                  new[]
                                     {
                                        "PackageId", "Version", "Platform", "HomeUrl", "Author",
                                        "Description", "ReleaseNotes", "License", "IsDeleted"
                                     },
                                  new object[]
                                     {
                                        manifest.PackageId, manifest.VersionString,
                                        manifest.Platform, manifest.ProjectUrl, manifest.Author,
                                        manifest.Description, manifest.ReleaseNotes, manifest.License, false
                                     });

         foreach (PackageDependency dependency in manifest.Dependencies)
         {
            long depId = Insert("PackageDependency",
                                new[]
                                   {
                                      "PackageManifestId", "PackageId", "VersionPattern", "Platform", "Scope",
                                      "CreatePlatformFolder"
                                   },
                                new object[]
                                   {
                                      manifestId, dependency.PackageId, dependency.VersionPattern,
                                      dependency.Platform, (long) dependency.Scope, dependency.TargetFolder
                                   });
         }

         return manifestId;
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
            TargetFolder = reader.AsString("TargetFolder")
         };
      }

      public Package GetManifest(long manifestId)
      {
         using(IDataReader reader = ExecuteReader(ManifestTableName, null,
            new[] { "PackageManifestId=(?)"}, manifestId))
         {
            if(reader.Read())
            {
               long dbid;
               return ReadPackage(reader, out dbid);
            }
         }

         throw new FileNotFoundException("manifest " + manifestId + " not found");
      }

      public Package GetManifest(PackageKey key)
      {
         long dbId;
         Package root;

         using (IDataReader reader = ExecuteReader(ManifestTableName, null,
            new[] { "PackageId=(?)", "Version=(?)", "Platform=(?)" },
            new object[] { key.PackageId, key.Version.ToString(), key.Platform }))
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

         using (IDataReader reader = ExecuteReader("PackageDependency", null,
            new[] { "PackageManifestId=(?)" }, new object[] { dbId }))
         {
            while (reader.Read())
            {
               root.Dependencies.Add(ReadDependency(reader));
            }
         }

         return root;
      }

      public long FindManifest(PackageKey key)
      {
         return ExecuteScalar<long>(ManifestTableName, "PackageManifestId",
                                    new[] {"PackageId=(?)", "Version=(?)", "Platform=(?)"},
                                    key.PackageId, key.VersionString, key.Platform);
      }


      #endregion

      protected override string GetSelectId()
      {
         return SelectId;
      }

      protected override string GetParameterName(int idx)
      {
         return "(?)";
      }

      public override void Dispose()
      {
         if (_conn != null)
         {
            _conn.Close();
            _conn.Dispose();
         }
      }
   }
}
