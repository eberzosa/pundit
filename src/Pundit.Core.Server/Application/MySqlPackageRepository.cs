using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Pundit.Core.Model;
using Pundit.Core.Server.Model;

namespace Pundit.Core.Server.Application
{
   class MySqlPackageRepository : MySqlRepositoryBase, IPackageRepository
   {
      private const string ManifestTableName = "PackageManifest";
      private const string DependencyTableName = "PackageDependency";
      private const string HistoryTableName = "ManifestHistory";
      private const string LiveManifestTableName = "LivePackageManifest";

      public MySqlPackageRepository() : this(null)
      {
         
      }

      public MySqlPackageRepository(string connectionString) : base(connectionString)
      {
         
      }

      private string FormatVersion(Version v)
      {
         return string.Format("{0}.{1}.{2}.{3}",
            Math.Max(0, v.Major),
            Math.Max(0, v.Minor),
            Math.Max(0, v.Build),
            Math.Max(0, v.Revision));
      }

      #region Implementation of IPackageRepository

      public long SavePackage(Package p, bool recordHistory)
      {
         if (p == null) throw new ArgumentNullException("p");

         using (IDbTransaction tranny = BeginTransaction())
         {
            long id = Insert(
               ManifestTableName,
               new[] {"PackageId", "Version", "Platform", "ProjectUrl", "Author", "Description", "ReleaseNotes", "License"},
               p.PackageId, FormatVersion(p.Version), p.Platform, p.ProjectUrl, p.Author, p.Description, p.ReleaseNotes,
               p.License);

            //insert dependencies
            foreach(PackageDependency pd in p.Dependencies)
            {
               Insert(DependencyTableName,
                      new[]
                         {
                            "PackageManifestId",
                            "PackageId", "VersionPattern", "Platform",
                            "Scope", "CreatePlatformFolder"
                         },
                      id,
                      pd.PackageId, pd.VersionPattern, pd.Platform,
                      (long)pd.Scope, pd.CreatePlatformFolder);
            }

            tranny.Commit();

            return id;
         }
      }

      public void DeletePackage(long packageId)
      {
         DeleteRecord(ManifestTableName, packageId);
      }

      public void DeletePackage(PackageKey key)
      {

      }

      private void AddPackageDependencies(long packageId, Package p)
      {
         using (IDataReader reader = ExecuteReader(DependencyTableName, null,
                                                    new[] {"PackageManifestId=?P0"}, packageId))
         {
            while (reader.Read())
            {
               var pd = new PackageDependency(reader.AsString("PackageId"), reader.AsString("VersionPattern"));
               pd.Platform = reader.AsString("Platform");
               pd.Scope = (DependencyScope) reader.AsLong("Scope");
               pd.CreatePlatformFolder = reader.AsBool("CreatePlatformFolder");
               p.Dependencies.Add(pd);
            }
         }
      }

      private Package ReadPackage(IDataReader reader, out long packageId)
      {
         Package p = new Package(reader.AsString("PackageId"), new Version(reader.AsString("Version")));
         p.Platform = reader.AsString("Platform");
         p.ProjectUrl = reader.AsString("ProjectUrl");
         p.Author = reader.AsString("Author");
         p.Description = reader.AsString("Description");
         p.ReleaseNotes = reader.AsString("ReleaseNotes");
         p.License = reader.AsString("License");
         packageId = reader.AsLong("PackageManifestId");
         return p;
      }

      private Package ReadFullPackage(IDataReader reader)
      {
         Package p = null;
         long id = 0;

         try
         {
            if (reader.Read())
            {
               p = ReadPackage(reader, out id);
            }
         }
         finally
         {
            reader.Dispose();
         }
         if(p != null) AddPackageDependencies(id, p);
         return p;
      }

      private IEnumerable<Package> ReadFullPackages(IDataReader reader)
      {
         var r = new List<Package>();
         var ids = new List<long>();

         try
         {
            while (reader.Read())
            {
               long id;
               Package p = ReadPackage(reader, out id);
               ids.Add(id);
               r.Add(p);
            }
         }
         finally
         {
            reader.Dispose();
         }

         for (int i = 0; i < r.Count; i++ )
         {
            AddPackageDependencies(ids[i], r[i]);
         }

         return r;
      }

      public Package GetPackage(long packageId)
      {
         IDataReader reader = ExecuteReader(ManifestTableName, null, new[] {"PackageManifestId=?P0"}, packageId);
         return ReadFullPackage(reader);
      }

      public Package GetPackage(PackageKey key)
      {
         IDataReader reader = ExecuteReader(ManifestTableName, null,
            new[] { "PackageId=?P0", "Version=?P1", "Platform=?P2" },
            key.PackageId, FormatVersion(key.Version), key.Platform);
         return ReadFullPackage(reader);
      }

      public bool Exists(PackageKey key)
      {
         throw new NotImplementedException();
      }

      public IEnumerable<Package> GetPackages(long offset, long count, out long totalCount)
      {
         totalCount = ExecuteScalar<long>(ManifestTableName, "count(*)", null, null);

         if(totalCount > 0)
         {
            IDataReader reader = ExecuteReaderPage(ManifestTableName, offset, count, null, null);
            return ReadFullPackages(reader);
         }

         return new Package[0];
      }

      #endregion
   }
}
