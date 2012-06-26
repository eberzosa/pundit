using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pundit.Core.Model;

namespace Pundit.Core.Server.Model
{
   public enum PackageSortOrder
   {
      None,
      CreatedDate
   }

   public class PackagesQuery
   {
      public long Offset { get; set; }
      public long Count { get; set; }
      public bool Active { get; set; }
      public PackageSortOrder SortOrder { get; set; }
      public bool SortAscending { get; set; }

      public PackagesQuery(long offset, long count)
      {
         Offset = offset;
         Count = count;
         Active = true;
         SortOrder = PackageSortOrder.None;
         SortAscending = true;
      }
   }

   public class PackagesResult
   {
      public IEnumerable<DbPackage> Packages { get; set; }
      public long Count { get; set; }
      public long TotalCount { get; set; }
   }

   public interface IPackageRepository : IDisposable
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="p"></param>
      /// <param name="fileSize"> </param>
      /// <param name="recordHistory">When true saves action to the log</param>
      /// <returns>Saved package ID</returns>
      long SavePackage(Package p, long fileSize, bool recordHistory);

      void DeletePackage(long packageId);

      void DeletePackage(PackageKey key);

      DbPackage GetPackage(long packageId);

      DbPackage GetPackage(PackageKey key);

      bool Exists(PackageKey key);

      PackagesResult GetPackages(PackagesQuery query);

      IEnumerable<DbPackage> GetAllRevisions(PackageKey key);

      void DeactivatePackage(long packageId);
      
      IEnumerable<DbPackage> ReadLog(long startRecordId, int maxRecords, bool includePackages);
   }
}
