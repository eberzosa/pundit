using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Model
{
   public class ZapStats
   {
      public ZapStats(long bytesDeleted, long packagesDeleted)
      {
         BytesDeleted = bytesDeleted;
         PackagesDeleted = packagesDeleted;
      }

      public long BytesDeleted { get; private set; }

      public long PackagesDeleted { get; private set; }
   }

   /// <summary>
   /// Sets of methods to work with local configuration
   /// </summary>
   public interface IRepositoryManager : IDisposable
   {
      ILocalRepository LocalRepository { get; }

      IEnumerable<Repo> AllRepositories { get; }

      IEnumerable<Repo> ActiveRepositories { get; }

      /// <summary>
      /// Gets repository by it's name
      /// </summary>
      /// <param name="tag"></param>
      /// <returns>Found repository or null if not found</returns>
      Repo GetRepositoryByTag(string tag);

      LocalStats Stats { get; }

      /// <summary>
      /// Deletes all binaries from the local cache
      /// </summary>
      ZapStats ZapCache();

      Repo Register(Repo newRepo);

      void Unregister(long repoId);

      void Update(Repo repo);

      /// <summary>
      /// Plays remote snapshot on local repository
      /// </summary>
      /// <param name="repo"></param>
      /// <param name="snapshot"></param>
      void PlaySnapshot(Repo repo, RemoteSnapshot snapshot);
   }
}
