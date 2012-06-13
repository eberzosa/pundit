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
      /// <param name="recordHistory">When true saves action to the log</param>
      /// <returns>Saved package ID</returns>
      long SavePackage(Package p, bool recordHistory);

      void DeletePackage(long packageId);

      void DeletePackage(PackageKey key);

      Package GetPackage(long packageId);

      Package GetPackage(PackageKey key);

      bool Exists(PackageKey key);

      IEnumerable<Package> GetPackages(long offset, long count, out long totalCount);

      RemoteSnapshot ReadLog(long startRecordId, bool includePackages);
   }
}
