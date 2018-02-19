using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Resolvers
{
   public class SatisfyingInfoExtended : SatisfyingInfo
   {
      public string PackageId { get; set; }

      public string Platform { get; }

      
      public SatisfyingInfoExtended(SatisfyingInfo satisfying, string packageId, string platform)
         : base(satisfying.Version, satisfying.Repo)
      {
         PackageId = packageId;
         Platform = platform;
      }

      public PackageKey GetPackageKey() => new PackageKey(PackageId, Version, Platform);
   }
}