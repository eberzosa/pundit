using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Model
{
   public interface IRepositoryManager
   {
      ILocalRepository LocalRepository { get; }
      IEnumerable<Repo> AllRepositories { get; }
      IEnumerable<Repo> ActiveRepositories { get; }
      IEnumerable<Repo> PublishingRepositories { get; }
      Repo GetRepositoryByTag(string tag);
      Repo GetRepositoryById(long id);

      /// <summary>
      /// Returns the size occupied by local data in total in bytes. It's never shrinkable to zero.
      /// </summary>
      long OccupiedSpace { get; }

      /// <summary>
      /// Returns size occupied by binary files
      /// </summary>
      long OccupiedBinarySpace { get; }

      /// <summary>
      /// Deletes all binaries from the local cache
      /// </summary>
      void ZapBinarySpace();

      void Register(Repo newRepo);

      void Unregister(long repoId);

      void Update(Repo repo);
   }
}
