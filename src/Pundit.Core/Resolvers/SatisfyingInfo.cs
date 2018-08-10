using EBerzosa.Pundit.Core.Repository;

namespace EBerzosa.Pundit.Core.Resolvers
{
   public class SatisfyingInfo
   {
      public NuGet.Versioning.NuGetVersion Version { get; set; }

      public IRepository Repo { get; set; }

      public RepositoryType RepoType => Repo is NuGetFileSystemRepo ? RepositoryType.NuGet : RepositoryType.Pundit;

      public SatisfyingInfo(NuGet.Versioning.NuGetVersion version, IRepository repo)
      {
         Version = version;
         Repo = repo;
      }
      
      public override string ToString() => Version.ToString();

      public override int GetHashCode() => Version.GetHashCode();
      
      public override bool Equals(object obj) => Version.Equals((obj as SatisfyingInfo)?.Version);
   }
}
