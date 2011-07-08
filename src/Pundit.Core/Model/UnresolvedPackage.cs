using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Pundit.Core.Utils;

namespace Pundit.Core.Model
{
   [DataContract]
   public class UnresolvedPackage
   {
      [DataMember]
      public string PackageId { get; set; }

      [DataMember]
      public string Platform { get; set; }

      public UnresolvedPackage(string packageId, string platform)
      {
         if (packageId == null) throw new ArgumentNullException("packageId");

         PackageId = packageId;
         Platform = PackageUtils.TrimPlatformName(platform);
      }

      public override bool Equals(object obj)
      {
         if(obj is UnresolvedPackage)
         {
            UnresolvedPackage that = (UnresolvedPackage) obj;

            return that.PackageId == this.PackageId &&
                   PackageUtils.ArePlatformsEqual(this.Platform, that.Platform);
         }

         return false;
      }

      public override int GetHashCode()
      {
         return PackageId.GetHashCode()*Platform.GetHashCode();
      }

      public override string ToString()
      {
         return string.Format("package id: [{0}], platform: [{1}]",
                              PackageId, Platform);
      }
   }
}
