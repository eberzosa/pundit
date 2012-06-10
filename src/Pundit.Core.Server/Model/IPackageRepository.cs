using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pundit.Core.Model;

namespace Pundit.Core.Server.Model
{
   public interface IPackageRepository : IDisposable
   {
      long SavePackage(Package p);

      Package GetPackage(long packageId);

      Package GetPackage(PackageKey key);

      bool Exists(PackageKey key);
   }
}
