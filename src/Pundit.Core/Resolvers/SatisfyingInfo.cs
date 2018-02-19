using System;
using EBerzosa.Pundit.Core.Repository;
using NuGet.Versioning;

namespace EBerzosa.Pundit.Core.Resolvers
{
   public class SatisfyingInfo : IComparable<SatisfyingInfo>
   {
      public NuGetVersion Version { get; set; }

      public IRepository Repo { get; set; }
        

      public SatisfyingInfo(NuGetVersion version, IRepository repo)
      {
         Version = version;
         Repo = repo;
      }

      public int CompareTo(SatisfyingInfo other)
      {
         return Version.CompareTo(other.Version);
      }
   }
}
