using System.Collections.Generic;
using System.Linq;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;
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

      public SatisfyingInfo GetActiveSatisfyingInfo(UnresolvedPackage package)
      {
         if (_resolution.ContainsKey(package) && _resolution[package] != null && _resolution[package].Count > 0)
            return _resolution[package].OrderByDescending(p => p.Version, package.AllowedVersions.Comparer).FirstOrDefault();
         
         return null;
      }

      public IEnumerable<PackageKey> GetPackages()
      {
         foreach (var package in _resolution.Keys)
         {
            var satisfyingInfo = GetActiveSatisfyingInfo(package);
            if (satisfyingInfo != null)
               yield return new PackageKey(package.PackageId, satisfyingInfo.Version, package.Framework);
         }
      }

      public IEnumerable<SatisfyingInfoExtended> GetSatisfyingInfos()
      {
         foreach (var package in _resolution.Keys)
         {
            var satisfyingInfo = GetActiveSatisfyingInfo(package);
            if (satisfyingInfo != null)
               yield return new SatisfyingInfoExtended(satisfyingInfo, package.PackageId, package.Framework);
         }
      }

      public IEnumerable<UnresolvedPackage> GetConflictedPackages()
      {
         return _resolution
            .Where(r => r.Value.Count == 0)
            .Select(r => new UnresolvedPackage(r.Key.PackageId, r.Key.Framework, r.Key.AllowedVersions));
      }

   }
}
