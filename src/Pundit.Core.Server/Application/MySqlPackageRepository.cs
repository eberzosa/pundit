using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Pundit.Core.Model;
using Pundit.Core.Server.Model;

namespace Pundit.Core.Server.Application
{
   class MySqlPackageRepository : MySqlRepositoryBase, IPackageRepository
   {
      private const string ManifestTableName = "PackageManifest";
      private const string DependencyTableName = "PackageDependency";
      private const string LogTableName = "PackageLog";
      private static readonly string[] KeyColumns = new[] {"PackageId", "Version", "Platform"};
      private static readonly string[] KeyRestriction = new[] { "PackageId=?P0", "Platform=?P1", "VMaj=?P2", "VMin=?P3", "VBld=?P4", "VRev=?P5" };

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

      private void RecordHistory(long recordId, Package p)
      {
         Insert(LogTableName,
                new[]
                   {
                      "ModType", "ModTime", "PackageManifestId",
                      "PackageId", "Version", "Platform"
                   },
                (long) SnapshotPackageDiff.Add, DateTime.UtcNow, recordId,
                p.Key.PackageId, FormatVersion(p.Key.Version), p.Key.Platform);
      }

      #region Implementation of IPackageRepository

      public long SavePackage(Package p, long fileSize, bool recordHistory)
      {
         if (p == null) throw new ArgumentNullException("p");

         using (IDbTransaction tranny = BeginTransaction())
         {
            long id = Insert(
               ManifestTableName,
               new[]
                  {
                     "PackageId", "Platform",
                     "ProjectUrl", "Author", "Description", "ReleaseNotes", "License",
                     "IsActive", "CreatedDate", "FileSize",
                     "VMaj", "VMin", "VBld", "VRev"
                  },
               p.PackageId, p.Platform,
               p.ProjectUrl, p.Author, p.Description, p.ReleaseNotes, p.License,
               true, DateTime.UtcNow, fileSize,
               Math.Max(0, p.Version.Major),
               Math.Max(0, p.Version.Minor),
               Math.Max(0, p.Version.Build),
               Math.Max(0, p.Version.Revision));

            //insert dependencies
            foreach(PackageDependency pd in p.Dependencies)
            {
               Insert(DependencyTableName,
                      new[]
                         {
                            "PackageManifestId",
                            "PackageId", "VersionPattern", "Platform",
                            "Scope", "TargetFolder"
                         },
                      id,
                      pd.PackageId, pd.VersionPattern, pd.Platform,
                      (long)pd.Scope, pd.TargetFolder);
            }

            if(recordHistory) RecordHistory(id, p);

            tranny.Commit();

            return id;
         }
      }

      public void DeletePackage(long packageId)
      {
         var ids = new List<long>();
         using(IDataReader reader = ExecuteReader(
            LogTableName, new[] { "PackageLogId" },
            new[] { "PackageManifestId"}, packageId ))
         {
            while (reader.Read())
            {
               ids.Add(reader.AsLong("PackageLogId"));
            }
         }
         foreach(long id in ids) DeleteRecord(LogTableName, id);

         DeleteRecord(ManifestTableName, packageId);
      }

      public void DeletePackage(PackageKey key)
      {
         var ids = new List<long>();
         using(IDataReader reader = ExecuteReader(
            ManifestTableName,
            new[] { "PackageManifestId"},
            new[] { "PackageId=?P0", "Platform=?P1", "VMaj=?P2", "VMin=?P3", "VBld=?P4", "VRev=?P5"},
            key.PackageId, key.Platform,
            Math.Max(0, key.Version.Major),
            Math.Max(0, key.Version.Minor),
            Math.Max(0, key.Version.Build),
            Math.Max(0, key.Version.Revision)))
         {
            while (reader.Read())
            {
               ids.Add(reader.AsLong("PackageManifestId"));
            }
         }

         foreach(long id in ids) DeletePackage(id);
      }

      private void AddPackageDependencies(DbPackage p)
      {
         using (IDataReader reader = ExecuteReader(DependencyTableName, null,
                                                    new[] {"PackageManifestId=?P0"}, p.Id))
         {
            while (reader.Read())
            {
               var pd = new PackageDependency(reader.AsString("PackageId"), reader.AsString("VersionPattern"));
               pd.Platform = reader.AsString("Platform");
               pd.Scope = (DependencyScope) reader.AsLong("Scope");
               pd.TargetFolder = reader.AsString("TargetFolder");
               p.Package.Dependencies.Add(pd);
            }
         }
      }

      private DbPackage ReadPackage(IDataReader reader)
      {
         long major = reader.AsLong("VMaj");
         long minor = reader.AsLong("VMin");
         long build = reader.AsLong("VBld");
         long revision = reader.AsLong("VRev");
         var v = new Version((int)major, (int)minor, (int)build, (int)revision);

         Package p = new Package(reader.AsString("PackageId"), v);
         p.Platform = reader.AsString("Platform");
         p.ProjectUrl = reader.AsString("ProjectUrl");
         p.Author = reader.AsString("Author");
         p.Description = reader.AsString("Description");
         p.ReleaseNotes = reader.AsString("ReleaseNotes");
         p.License = reader.AsString("License");

         var dp = new DbPackage(reader.AsLong("PackageManifestId"), p);
         dp.CreatedDate = reader.AsDateTime("CreatedDate");
         dp.FileSize = reader.AsLong("FileSize");
         return dp;
      }

      private DbPackage ReadFullPackage(IDataReader reader)
      {
         DbPackage p = null;

         try
         {
            if (reader.Read())
            {
               p = ReadPackage(reader);
            }
         }
         finally
         {
            reader.Dispose();
         }
         if(p != null) AddPackageDependencies(p);
         return p;
      }

      private IEnumerable<DbPackage> ReadFullPackages(IDataReader reader)
      {
         var r = new List<DbPackage>();

         try
         {
            while (reader.Read())
            {
               r.Add(ReadPackage(reader));
            }
         }
         finally
         {
            reader.Dispose();
         }

         foreach(var dbp in r) AddPackageDependencies(dbp);

         return r;
      }

      public DbPackage GetPackage(long packageId)
      {
         IDataReader reader = ExecuteReader(ManifestTableName, null, new[] {"PackageManifestId=?P0"}, packageId);
         return ReadFullPackage(reader);
      }

      public DbPackage GetPackage(PackageKey key)
      {
         IDataReader reader = ExecuteReader(ManifestTableName, null,
            new[] { "PackageId=?P0", "Platform=?P1", "VMaj=?P2", "VMin=?P3", "VBld=?P4", "VRev=?P5" },
            key.PackageId, key.Platform,
            Math.Max(0, key.Version.Major),
            Math.Max(0, key.Version.Minor),
            Math.Max(0, key.Version.Build),
            Math.Max(0, key.Version.Revision));
         return ReadFullPackage(reader);
      }

      public bool Exists(PackageKey key)
      {
         uint id = ExecuteScalar<uint>(ManifestTableName, "PackageManifestId",
                                       KeyRestriction, key.PackageId, key.Platform,
                                       Math.Max(0, key.Version.Major),
                                       Math.Max(0, key.Version.Minor),
                                       Math.Max(0, key.Version.Build),
                                       Math.Max(0, key.Version.Revision));
         return id != 0;
      }

      private string GetSortColumn(PackageSortOrder order)
      {
         switch (order)
         {
            case PackageSortOrder.CreatedDate:
               return "CreatedDate";
            default:
               return null;
         }
      }

      public PackagesResult GetPackages(PackagesQuery query)
      {
         var result = new PackagesResult();
         result.TotalCount = ExecuteScalar<long>(ManifestTableName, "count(*)", new[] {"IsActive=?P0"}, query.Active);

         if(result.TotalCount > 0)
         {
            IDataReader reader = ExecuteSortedReaderPage(
               ManifestTableName, query.Offset, query.Count,
               GetSortColumn(query.SortOrder), query.SortAscending,
               null, new[] {"IsActive=?P0"}, query.Active);

            result.Packages = ReadFullPackages(reader);
            result.Count = result.Packages.Count();
         }

         return result;
      }

      public IEnumerable<DbPackage> GetAllRevisions(PackageKey key)
      {
         using(IDataReader reader = ExecuteReader(ManifestTableName,
            null,
            new[] { "PackageId=?P0", "Platform=?P1", "VMaj=?P2", "VMin=?P3", "VBld=?P4" },
            key.PackageId, key.Platform,
            Math.Max(0, key.Version.Major),
            Math.Max(0, key.Version.Minor),
            Math.Max(0, key.Version.Build)))
         {
            return ReadFullPackages(reader);
         }
      }

      public void DeactivatePackage(long packageId)
      {
         throw new NotImplementedException();
      }

      public IEnumerable<DbPackage> ReadLog(long startRecordId, int maxRecords, bool includePackages)
      {
         using(IDataReader reader = ExecuteReaderPage(ManifestTableName,
            0, maxRecords,
            null, new[] { "IsActive=?P0", "PackageManifestId>=?P1" },
            true, startRecordId))
         {
            return ReadFullPackages(reader);
         }
      }

      #endregion
   }
}
