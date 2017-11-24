using System.Runtime.Serialization;
using EBerzosa.Utils;
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
      
      [DataMember]
      public bool IsDeveloper { get; set; }

      public UnresolvedPackage(string packageId, string platform)
      {
         Guard.NotNull(packageId, nameof(packageId));

         PackageId = packageId;
         Platform = PackageUtils.TrimPlatformName(platform);
      }

      public override bool Equals(object obj)
      {
         if (obj is UnresolvedPackage that)
         {
            return that.PackageId == PackageId &&
                   PackageUtils.ArePlatformsEqual(Platform, that.Platform) &&
                   IsDeveloper == that.IsDeveloper;
         }

         return false;
      }

      public override int GetHashCode() 
         => PackageId.GetHashCode() * Platform.GetHashCode() * IsDeveloper.GetHashCode();

      public override string ToString()
         => $"package id: [{PackageId}], platform: [{Platform}], developer: [{IsDeveloper}]";
   }
}
