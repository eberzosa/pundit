using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Model
{
   public class UnresolvedPackage
   {
      public string PackageId { get; set; }
      public string Platform { get; set; }

      public UnresolvedPackage(string packageId, string platform)
      {
         PackageId = packageId;
         Platform = platform;
      }

      public override bool Equals(object obj)
      {
         if(obj is UnresolvedPackage)
         {
            UnresolvedPackage that = (UnresolvedPackage) obj;

            return that.PackageId == this.PackageId && that.Platform == this.Platform;
         }

         return false;
      }

      public override int GetHashCode()
      {
         return base.GetHashCode()*12;
      }
   }
}
