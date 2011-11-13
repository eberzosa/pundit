using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Model
{
   /// <summary>
   /// Sets of methods to work with local configuration
   /// </summary>
   public interface IRepositoryManager
   {
      ILocalRepository LocalRepository { get; }

      IEnumerable<Repo> AllRepositories { get; }

      IEnumerable<Repo> ActiveRepositories { get; }

      IEnumerable<Repo> PublishingRepositories { get; }

      Repo GetRepositoryByTag(string tag);

      Repo GetRepositoryById(long id);

      LocalStats Stats { get; }

      /// <summary>
      /// Deletes all binaries from the local cache
      /// </summary>
      void ZapCache();

      Repo Register(Repo newRepo);

      void Unregister(long repoId);

      void Update(Repo repo);

      void PlaySnapshot(Repo repo, IEnumerable<PackageSnapshotKey> snapshot, string nextChangeId);
   }
}
