using System;
using System.Runtime.Serialization;
using EBerzosa.Pundit.Core.Package;
using EBerzosa.Pundit.Core.Versioning;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.Core.Model
{
   [DataContract]
   public class UnresolvedPackage
   {
      [DataMember]
      public string PackageId { get; set; }

      [DataMember]
      public string Platform { get; set; }
      
      [DataMember]
      public FloatRange AllowedVersions { get; set; }


      public UnresolvedPackage(string packageId, string platform, FloatRange allowedVersions)
      {
         Guard.NotNull(packageId, nameof(packageId));

         PackageId = packageId;
         AllowedVersions = allowedVersions;
         Platform = PackageFileName.TrimPlatformName(platform);
      }

      public override bool Equals(object obj)
      {
         if (obj is UnresolvedPackage that)
         {
            return that.PackageId == PackageId && 
               PackageFileName.TrimPlatformName(Platform).Equals(PackageFileName.TrimPlatformName(that.Platform), StringComparison.OrdinalIgnoreCase);
         }

         return false;
      }

      public override int GetHashCode() 
         => PackageId.GetHashCode() * Platform.GetHashCode();

      public override string ToString()
         => $"package id: [{PackageId}], platform: [{Platform}]";
   }
}
