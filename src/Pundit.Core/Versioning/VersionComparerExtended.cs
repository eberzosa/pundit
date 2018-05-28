using System.Collections.Generic;

namespace EBerzosa.Pundit.Core.Versioning
{
   public class VersionComparerExtended : IComparer<NuGet.Versioning.NuGetVersion>
   {
      private readonly NuGet.Versioning.IVersionComparer _comparer;

      public VersionComparerExtended(NuGet.Versioning.IVersionComparer comparer)
      {
         _comparer = comparer;
      }

      public int Compare(NuGet.Versioning.NuGetVersion x, NuGet.Versioning.NuGetVersion y)
      {
         if (ReferenceEquals(x, y))
            return 0;

         if (ReferenceEquals(y, null))
            return -1;
         
         if (ReferenceEquals(x, null))
            return 1;

         if (_comparer != VersionComparer.Pundit)
            return _comparer.Compare(x, y);

         // We don't want to compare the revision number as ReleaseLabel versions do not use it as main version
         if (!y.IsPrerelease && x.IsPrerelease)
            y = y.Append("-").RevisionToLabel();

         if (!x.IsPrerelease && y.IsPrerelease)
            x = x.Append("-").RevisionToLabel();

         return _comparer.Compare(x, y);
      }
   }
}
