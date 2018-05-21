using System.Runtime.Serialization;
using EBerzosa.Pundit.Core.Framework;
using EBerzosa.Pundit.Core.Versioning;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.Core.Model
{
   [DataContract]
   public class UnresolvedPackage
   {
      [DataMember]
      public string PackageId { get; }

      [DataMember]
      public PunditFramework Framework { get; }
      
      [DataMember]
      public VersionRangeExtended AllowedVersions { get; }


      public UnresolvedPackage(string packageId, PunditFramework framework, VersionRangeExtended allowedVersions)
      {
         Guard.NotNull(packageId, nameof(packageId));

         PackageId = packageId;
         AllowedVersions = allowedVersions;
         Framework = framework;
      }

      public override bool Equals(object obj)
      {
          if (obj is UnresolvedPackage that)
              return that.PackageId == PackageId && Framework == that.Framework;

          return false;
      }

      public override int GetHashCode()  => PackageId.GetHashCode() * Framework.GetHashCode();

      public override string ToString() => $"{PackageId} [{AllowedVersions}] [{Framework}]";
   }
}
