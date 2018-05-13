using System;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Versioning;

namespace EBerzosa.Pundit.Core.Resolvers
{
   public class SatisfyingInfo : IComparable<SatisfyingInfo>
   {
      public PunditVersion Version { get; set; }

      public IRepository Repo { get; set; }
        

      public SatisfyingInfo(PunditVersion version, IRepository repo)
      {
         Version = version;
         Repo = repo;
      }

      public int CompareTo(SatisfyingInfo other)
      {
         return Version.CompareTo(other.Version);
      }
      

      public override string ToString() => Version.ToString();

      public override bool Equals(object obj) => Version.Equals(obj);

      public override int GetHashCode() => Version.GetHashCode();
   }
}
