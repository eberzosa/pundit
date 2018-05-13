using System;
using System.Collections.Generic;
using System.Linq;

namespace EBerzosa.Pundit.Core.Resolvers
{
   public class DependencyNodeComparer : IEqualityComparer<DependencyNode>
   {
      private static DependencyNodeComparer _packageIdComparer;

      public static DependencyNodeComparer PackageId => _packageIdComparer ?? (_packageIdComparer = new DependencyNodeComparer());

      private DependencyNodeComparer()
      {
      }

      public bool Equals(DependencyNode x, DependencyNode y)
      {
         if (x == null && y == null)
            return true;

         if (x == null || y == null)
            return false;

         if (x.PackageId != y.PackageId)
            return false;

         var xVersions = x.AllVersions.ToArray();
         var yVersions = y.AllVersions.ToArray();

         if (xVersions.Length != yVersions.Length)
            return false;

         return xVersions.All(xVersion => yVersions.Contains(xVersion));
      }

      public int GetHashCode(DependencyNode obj) => obj.GetHashCode();
   }
}