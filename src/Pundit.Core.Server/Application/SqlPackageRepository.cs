using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Pundit.Core.Model;
using Pundit.Core.Server.Model;

namespace Pundit.Core.Server.Application
{
   class SqlPackageRepository : MySqlRepositoryBase, IPackageRepository
   {
      private const string ManifestTableName = "PackageManifest";
      private const string DependencyTableName = "PackageDependency";
      private const string HistoryTableName = "ManifestHistory";
      private const string LiveManifestTableName = "LivePackageManifest";

      public SqlPackageRepository(string connectionString = null) : base(connectionString)
      {
         
      }

      #region Implementation of IPackageRepository

      public long SavePackage(Package p)
      {
         using (IDbTransaction tranny = BeginTransaction())
         {
            long id = Insert(
               ManifestTableName,
               new[] {"PackageId", "Version", "Platform", "ProjectUrl", "Author", "Description", "ReleaseNotes", "License"},
               p.PackageId, p.VersionString, p.Platform, p.ProjectUrl, p.Author, p.Description, p.ReleaseNotes,
               p.License);

            //insert dependencies

            tranny.Commit();
            return id;
         }
      }

      private Package ReadPackage(IDataReader reader)
      {
         Package p;
         long id;
         try
         {
            if (reader.Read())
            {
               p = new Package(reader.AsString("PackageId"), new Version(reader.AsString("Version")));
               p.Platform = reader.AsString("Platform");
               p.ProjectUrl = reader.AsString("ProjectUrl");
               p.Author = reader.AsString("Author");
               p.Description = reader.AsString("Description");
               p.ReleaseNotes = reader.AsString("ReleaseNotes");
               p.License = reader.AsString("License");
               id = reader.AsLong("PackageManifestId");
            }
            else
            {
               p = null;
               id = 0;
            }
         }
         finally
         {
            reader.Dispose();
         }

         if (p != null)
         {
            using (IDataReader reader2 = ExecuteReader(DependencyTableName, null,
               new[] { "PackageManifestId=?P0" }, id))
            {
               while (reader2.Read())
               {
                  var pd = new PackageDependency(reader2.AsString("PackageId"), reader2.AsString("VersionPattern"));
                  pd.Platform = reader2.AsString("Platform");
                  pd.Scope = (DependencyScope)reader2.AsLong("Scope");
                  pd.CreatePlatformFolder = reader2.AsBool("CreatePlatformFolder");
                  p.Dependencies.Add(pd);
               }
            }            
         }

         return p;
      }

      public Package GetPackage(long packageId)
      {
         IDataReader reader = ExecuteReader(ManifestTableName, null, new[] {"PackageManifestId=?P0"}, packageId);
         return ReadPackage(reader);
      }

      public Package GetPackage(PackageKey key)
      {
         IDataReader reader = ExecuteReader(ManifestTableName, null,
            new[] { "PackageId=?P0", "Version=?P1", "Platform=?P2" },
            key.PackageId, key.VersionString, key.Platform);
         return ReadPackage(reader);
      }

      public bool Exists(PackageKey key)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}
