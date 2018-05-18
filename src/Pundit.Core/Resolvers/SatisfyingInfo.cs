using System;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Versioning;

namespace EBerzosa.Pundit.Core.Resolvers
{
   public class SatisfyingInfo : IComparable<SatisfyingInfo>, IComparable, IEquatable<SatisfyingInfo>
   {
      public PunditVersion Version { get; set; }

      public IRepository Repo { get; set; }

      public RepositoryType RepoType => Repo is NuGetFileSystemRepo ? RepositoryType.NuGet : RepositoryType.Pundit;

      public SatisfyingInfo(PunditVersion version, IRepository repo)
      {
         Version = version;
         Repo = repo;
      }

      public int CompareTo(SatisfyingInfo other) => Version.CompareTo(other.Version, VersionComparison.PunditVersion);

      public int CompareTo(object obj) => Version.CompareTo(obj);

      public bool Equals(SatisfyingInfo other) => Version.Equals(other?.Version, VersionComparison.PunditVersion);

      public override string ToString() => Version.ToString();

      public override int GetHashCode() => Version.GetHashCode();
      
      public override bool Equals(object obj) => Version.Equals(obj);
   }
}
