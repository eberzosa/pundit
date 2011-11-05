using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Model
{
   /// <summary>
   /// 
   /// </summary>
   public class VersionResolutionTable
   {
      private readonly Dictionary<UnresolvedPackage, HashSet<Version>> _resolution =
         new Dictionary<UnresolvedPackage, HashSet<Version>>();

      /// <summary>
      /// 
      /// </summary>
      public VersionResolutionTable()
      {
         
      }

      /// <summary>
      /// 
      /// </summary>
      public int ConflictCount
      {
         get
         {
            return _resolution
               .Where(r => r.Value == null || r.Value.Count == 0)
               .Count();
         }
      }

      /// <summary>
      /// 
      /// </summary>
      public bool HasConflicts
      {
         get { return ConflictCount > 0; }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="package"></param>
      /// <returns></returns>
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

      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
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
      /// <returns></returns>
      public IEnumerable<UnresolvedPackage> GetConflictedPackages()
      {
         return _resolution
            .Where(r => r.Value.Count == 0)
            .Select(r => new UnresolvedPackage(r.Key.PackageId, r.Key.Platform));
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
