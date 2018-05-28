using System;
using System.Collections.Generic;
using System.Linq;

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

      public bool Equals(NuGet.Versioning.NuGetVersion x, NuGet.Versioning.NuGetVersion y)
      {
         if (ReferenceEquals(x, y))
            return true;

         if (ReferenceEquals(y, null))
            return false;

         if (ReferenceEquals(x, null))
            return false;

         if (_comparer != VersionComparer.Pundit)
            return _comparer.Compare(x, y) == 0;

         return x.RemoveRevision().Equals(y.RemoveRevision(), NuGet.Versioning.VersionComparison.Version)
                && CompareLabels(x.ReleaseLabels.ToArray(), y.ReleaseLabels.ToArray());
      }

      private bool CompareLabels(string[] labels1, string[] labels2)
      {
         var limit1 = labels1.Length - 1;
         var limit2 = labels2.Length - 1;

         if (limit1 > -1 && int.TryParse(labels1[limit1], out _))
            limit1--;

         if (limit2 > -1 && int.TryParse(labels2[limit2], out _))
            limit2--;

         if (limit1 == -1 && limit2 == -1)
            return true;

         if (limit1 != limit2)
            return false;

         for (int i = 0; i < limit1; i++)
            if (!labels1[i].Equals(labels2[i], StringComparison.OrdinalIgnoreCase))
               return false;

         return true;
      }
   }
}
