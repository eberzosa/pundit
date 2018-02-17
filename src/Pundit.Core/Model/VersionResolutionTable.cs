using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EBerzosa.Pundit.Core.Model;
using NuGet.Versioning;

namespace Pundit.Core.Model
{
   public class VersionResolutionTable
   {
      private readonly Dictionary<UnresolvedPackage, HashSet<NuGetVersion>> _resolution =
         new Dictionary<UnresolvedPackage, HashSet<NuGetVersion>>();

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

      public NuGetVersion GetActiveVersion(UnresolvedPackage package)
      {
         if(_resolution.ContainsKey(package) && _resolution[package] != null &&
            _resolution[package].Count > 0)
         {
            HashSet<NuGetVersion> v = _resolution[package];
            List<NuGetVersion> sorted = v.ToList();
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
            .Select(r => new UnresolvedPackage(r.Key.PackageId, r.Key.Platform, r.Key.VersionPattern));
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="package"></param>
      /// <param name="versions"></param>
      /// <returns>Indicates if this intersection caused a conflict</returns>
      public bool Intersect(UnresolvedPackage package, IEnumerable<NuGetVersion> versions)
      {
         if(!_resolution.ContainsKey(package))
         {
            var set = new HashSet<NuGetVersion>();
            foreach (NuGetVersion v in versions) set.Add(v);
            
            _resolution[package] = set;

            return set.Count == 0;
         }
         else
         {
            HashSet<NuGetVersion> set = _resolution[package];

            set.IntersectWith(versions);

            return set.Count == 0;
         }
      }
   }
}
