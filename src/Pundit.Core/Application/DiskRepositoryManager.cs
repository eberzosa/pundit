using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Pundit.Core.Application.Repositories;
using Pundit.Core.Model;

namespace Pundit.Core.Application
{
   class DiskRepositoryManager : IRepositoryManager
   {
      private readonly string _homeFolder;
      private ILocalRepository _localRepo;

      public DiskRepositoryManager(string homeFolder)
      {
         if (homeFolder == null) throw new ArgumentNullException("homeFolder");
         if (!Directory.Exists(homeFolder)) throw new DirectoryNotFoundException(homeFolder);

         _homeFolder = homeFolder;
         Initialize();
      }

      private void Initialize()
      {
         _localRepo = new LocalDiskRepository(_homeFolder);
      }

      public ILocalRepository LocalRepository
      {
         get { return _localRepo; }
      }

      public IEnumerable<Repo> AllRepositories
      {
         get
         {
            throw new NotImplementedException();
         }
      }

      public IEnumerable<Repo> ActiveRepositories
      {
         get { return AllRepositories.Where(r => r.IsEnabled); }
      }

      public Repo GetRepositoryByTag(string tag)
      {
         return ActiveRepositories.FirstOrDefault(r => r.Tag == tag);
      }

      public LocalStats Stats
      {
         get
         {
            throw new NotImplementedException();
         }
      }

      public ZapStats ZapCache()
      {
         throw new NotImplementedException();
      }

      public Repo Register(Repo newRepo)
      {
         throw new NotImplementedException();
      }

      public void Unregister(long repoId)
      {
         throw new NotImplementedException();
      }

      public void Update(Repo repo)
      {
         throw new NotImplementedException();
      }

      public void PlaySnapshot(Repo repo, RemoteSnapshot snapshot)
      {
         if (repo == null) throw new ArgumentNullException("repo");
         throw new NotImplementedException();
      }

      #region Implementation of IDisposable

      public void Dispose()
      {
         _localRepo.Dispose();
      }

      #endregion
   }
}
