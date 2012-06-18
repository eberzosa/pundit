using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pundit.Core.Model;

namespace Pundit.Core.Server.Model
{
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

      IEnumerable<DbPackage> GetPackages(long offset, long count, bool active, out long totalCount);

      IEnumerable<DbPackage> GetAllRevisions(PackageKey key);

      void DeactivatePackage(long packageId);
      
      IEnumerable<DbPackage> ReadLog(long startRecordId, int maxRecords, bool includePackages);
   }
}
