using EBerzosa.Pundit.Core.Model.Package;

namespace EBerzosa.Pundit.Core.Resolvers
{
   public class SatisfyingInfoExtended : SatisfyingInfo
   {
      public string PackageId { get; set; }

      public string Framework { get; }

      
      public SatisfyingInfoExtended(SatisfyingInfo satisfying, string packageId, string framework)
         : base(satisfying.Version, satisfying.Repo)
      {
         PackageId = packageId;
         Framework = framework;
      }

      public PackageKey GetPackageKey() => new PackageKey(PackageId, Version, Framework);
   }
}