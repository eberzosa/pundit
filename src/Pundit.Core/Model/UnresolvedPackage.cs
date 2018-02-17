using System.Runtime.Serialization;
using EBerzosa.Pundit.Core;
using EBerzosa.Utils;
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
      public string VersionPattern { get; set; }

      public UnresolvedPackage(string packageId, string platform, VersionRange versionRange, bool isDeveloper)
         : this(packageId, platform, VersionUtils.ToPunditSearchVersion(versionRange, isDeveloper))
      {
      }

      public UnresolvedPackage(string packageId, string platform, string versionPattern)
      {
         Guard.NotNull(packageId, nameof(packageId));

         PackageId = packageId;
         VersionPattern = versionPattern;
         Platform = PackageUtils.TrimPlatformName(platform);
      }

      public override bool Equals(object obj)
      {
         if (obj is UnresolvedPackage that)
         {
            return that.PackageId == PackageId &&
                   PackageUtils.ArePlatformsEqual(Platform, that.Platform);
         }

         return false;
      }

      public override int GetHashCode() 
         => PackageId.GetHashCode() * Platform.GetHashCode();

      public override string ToString()
         => $"package id: [{PackageId}], platform: [{Platform}]";
   }
}
