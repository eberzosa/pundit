using System;
using System.Runtime.Serialization;
using EBerzosa.Pundit.Core;
using EBerzosa.Pundit.Core.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Utils;
using NuGet.Packaging.Core;
using NuGet.Versioning;
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
      public VersionRange VersionPattern { get; set; }


      public UnresolvedPackage(string packageId, string platform, VersionRange versionRange)
      {
         Guard.NotNull(packageId, nameof(packageId));

         PackageId = packageId;
         VersionPattern = versionRange;
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
