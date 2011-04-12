using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Model
{
   public class VersionResolutionTable
   {
      private readonly Dictionary<UnresolvedPackage, HashSet<Version>> _resolution =
         new Dictionary<UnresolvedPackage, HashSet<Version>>();

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

      public Version GetActiveVersion(UnresolvedPackage package)
      {
         if(_resolution.ContainsKey(package) && _resolution[package] != null &&
            _resolution[package].Count > 0)
         {
            HashSet<Version> v = _resolution[package];
            List<Version> sorted = v.ToList();
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

      /// <summary>
      /// 
      /// </summary>
      /// <param name="package"></param>
      /// <param name="versions"></param>
      /// <returns>Indicates if this intersection caused a conflict</returns>
      public bool Intersect(UnresolvedPackage package, IEnumerable<Version> versions)
      {
         if(!_resolution.ContainsKey(package))
         {
            var set = new HashSet<Version>();
            foreach (Version v in versions) set.Add(v);
            
            _resolution[package] = set;

            return set.Count == 0;
         }
         else
         {
            HashSet<Version> set = _resolution[package];

            set.IntersectWith(versions);

            return set.Count == 0;
         }
      }
   }
}
