using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EBerzosa.Pundit.Core.Model;

namespace Pundit.Core.Model
{
   public class VersionResolutionTable
   {
      private readonly Dictionary<UnresolvedPackage, HashSet<PunditVersion>> _resolution =
         new Dictionary<UnresolvedPackage, HashSet<PunditVersion>>();

      public VersionResolutionTable()
      {
         
      }

      public int ConflictCount
      {
         get
         {
            return _resolution
               .Where(r => r.Value == null || r.Value.Count == 0)
               .Count();
         }
      }

      public bool HasConflicts
      {
         get { return ConflictCount > 0; }
      }

      public PunditVersion GetActiveVersion(UnresolvedPackage package)
      {
         if(_resolution.ContainsKey(package) && _resolution[package] != null &&
            _resolution[package].Count > 0)
         {
            HashSet<PunditVersion> v = _resolution[package];
            List<PunditVersion> sorted = v.ToList();
            sorted.Sort();

            return sorted[sorted.Count - 1];
         }

         return null;
      }

      public IEnumerable<PackageKey> GetPackages()
      {
         return from package in _resolution.Keys
                let v = GetActiveVersion(package)
                where v != null
                select new PackageKey(package.PackageId, v, package.Platform);
      }

      public IEnumerable<UnresolvedPackage> GetConflictedPackages()
      {
         return _resolution
            .Where(r => r.Value.Count == 0)
            .Select(r => new UnresolvedPackage(r.Key.PackageId, r.Key.Platform) {IsDeveloper = r.Key.IsDeveloper});
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="package"></param>
      /// <param name="versions"></param>
      /// <returns>Indicates if this intersection caused a conflict</returns>
      public bool Intersect(UnresolvedPackage package, IEnumerable<PunditVersion> versions)
      {
         if(!_resolution.ContainsKey(package))
         {
            var set = new HashSet<PunditVersion>();
            foreach (PunditVersion v in versions) set.Add(v);
            
            _resolution[package] = set;

            return set.Count == 0;
         }
         else
         {
            HashSet<PunditVersion> set = _resolution[package];

            set.IntersectWith(versions);

            return set.Count == 0;
         }
      }
   }
}
