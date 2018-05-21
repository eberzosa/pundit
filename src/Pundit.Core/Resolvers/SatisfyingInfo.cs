using System;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Versioning;

namespace EBerzosa.Pundit.Core.Resolvers
{
   public class SatisfyingInfo : IComparable<SatisfyingInfo>, IComparable, IEquatable<SatisfyingInfo>
   {
      public NuGet.Versioning.NuGetVersion Version { get; set; }

      public IRepository Repo { get; set; }

      public RepositoryType RepoType => Repo is NuGetFileSystemRepo ? RepositoryType.NuGet : RepositoryType.Pundit;

      public SatisfyingInfo(NuGet.Versioning.NuGetVersion version, IRepository repo)
      {
         Version = version;
         Repo = repo;
      }

      public int CompareTo(SatisfyingInfo other) => VersionComparer.Compare(Version, other.Version, VersionComparison.PunditVersion);

      public int CompareTo(object obj) => Version.CompareTo(obj);

      public bool Equals(SatisfyingInfo other) => VersionComparer.Compare(Version, other?.Version, VersionComparison.PunditVersion) == 0;

      public override string ToString() => Version.ToString();

      public override int GetHashCode() => Version.GetHashCode();
      
      public override bool Equals(object obj) => Version.Equals(obj);
   }
}
