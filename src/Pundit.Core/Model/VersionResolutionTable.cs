using System.Collections.Generic;
using System.Linq;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Resolvers;

namespace Pundit.Core.Model
{
   public class VersionResolutionTable
   {
      private readonly Dictionary<UnresolvedPackage, HashSet<SatisfyingInfo>> _resolution;

      public VersionResolutionTable()
      {
         _resolution = new Dictionary<UnresolvedPackage, HashSet<SatisfyingInfo>>();
      }

      public int ConflictCount => _resolution.Count(r => r.Value == null || r.Value.Count == 0);

      public bool HasConflicts => ConflictCount > 0;


      /// <summary>
      /// 
      /// </summary>
      /// <param name="package"></param>
      /// <param name="satisfayingInfos"></param>
      /// <returns>Indicates if this intersection caused a conflict</returns>
      public bool Intersect(UnresolvedPackage package, IEnumerable<SatisfyingInfo> satisfayingInfos)
      {
         if (!_resolution.ContainsKey(package))
         {
            var set = new HashSet<SatisfyingInfo>();

            foreach (var s in satisfayingInfos)
               set.Add(s);

            _resolution[package] = set;

            return set.Count == 0;
         }
         else
         {
            var set = _resolution[package];

            set.IntersectWith(satisfayingInfos);

            return set.Count == 0;
         }
      }

      public SatisfyingInfo GetActiveSatisfayingInfo(UnresolvedPackage package)
      {
         if(_resolution.ContainsKey(package) && _resolution[package] != null &&
            _resolution[package].Count > 0)
         {
            var v = _resolution[package];
            var sorted = v.ToList();
            sorted.Sort();

            return sorted[sorted.Count - 1];
         }

         return null;
      }

      public IEnumerable<PackageKey> GetPackages()
      {
         return from package in _resolution.Keys
                let v = GetActiveSatisfayingInfo(package)
                where v != null
                select new PackageKey(package.PackageId, v.Version, package.Platform);
      }

      public IEnumerable<SatisfyingInfoExtended> GetSatisfyingInfos()
      { 
         return from package in _resolution.Keys
            let v = GetActiveSatisfayingInfo(package)
            where v != null
            select new SatisfyingInfoExtended(v, package.PackageId, package.Platform);
      }

      public IEnumerable<UnresolvedPackage> GetConflictedPackages()
      {
         return _resolution
            .Where(r => r.Value.Count == 0)
            .Select(r => new UnresolvedPackage(r.Key.PackageId, r.Key.Platform, r.Key.AllowedVersions));
      }

   }
}
