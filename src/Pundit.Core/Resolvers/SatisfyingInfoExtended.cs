using EBerzosa.Pundit.Core.Framework;
using EBerzosa.Pundit.Core.Model.Package;

namespace EBerzosa.Pundit.Core.Resolvers
{
   public class SatisfyingInfoExtended : SatisfyingInfo
   {
      public string PackageId { get; set; }

      public PunditFramework Framework { get; }

      
      public SatisfyingInfoExtended(SatisfyingInfo satisfying, string packageId, PunditFramework framework)
         : base(satisfying.Version, satisfying.Repo)
      {
         PackageId = packageId;
         Framework = framework;
      }

      public PackageKey GetPackageKey() => new PackageKey(PackageId, Version, Framework);
   }
}