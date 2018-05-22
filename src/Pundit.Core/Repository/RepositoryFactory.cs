using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Converters;
using EBerzosa.Pundit.Core.Repository.Xml;
using EBerzosa.Pundit.Core.Serializers;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.Core.Repository
{
   public class RepositoryFactory
   {
      public const string PunditCacheRepoName = "punditcache";
      public const string NuGetCacheRepoName = "nugetcache";

      private const string PunditConfigFolder = ".pundit";
      private const string PunditCacheRepoFolder = "repository";
      private const string NuGetCacheRepoFolder = "nugetrepo";
      private const string RepositoriesConfigFile = "repositories.xml";

      private readonly PackageReaderFactory _packageReaderFactory;
      private readonly ISerializer _serializer;

      private readonly string _cacheRepoPath;
      private readonly string _cacheNuGetRepoPath;
      private readonly string _repoConfigPath;

      private RegisteredRepositories _registeredRepositories;
      


      public RepositoryFactory(PackageReaderFactory packageReaderFactory, ISerializer serializer)
      {
         Guard.NotNull(packageReaderFactory, nameof(packageReaderFactory));
         Guard.NotNull(serializer, nameof(serializer));

         _packageReaderFactory = packageReaderFactory;
         _serializer = serializer;

         var punditConfigPath = ResolveRootPath();
         _cacheRepoPath = Path.Combine(punditConfigPath, PunditCacheRepoFolder);
         _cacheNuGetRepoPath = Path.Combine(punditConfigPath, NuGetCacheRepoFolder);
         _repoConfigPath = Path.Combine(punditConfigPath, RepositoriesConfigFile);
      }


      public ICollection<IRepository> TryGetCacheRepos()
      {
         return new[]
         {
            TryCreateRepo(new RegisteredRepository {Uri = _cacheNuGetRepoPath, Name = NuGetCacheRepoName, UseForPublishing = true, Type = RepositoryType.NuGet}),
            TryCreateRepo(new RegisteredRepository {Uri = _cacheRepoPath, Name = PunditCacheRepoName, UseForPublishing = true})
         };
      }
      
      public IRepository TryGetEnabledRepo(string name)
      {
         return TryGetEnabledRepos(true, true).FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
      }

      public IEnumerable<IRepository> TryGetEnabledRepos(bool includeCache, bool includeOther)
      {
         if (includeCache)
            foreach (var repository in TryGetCacheRepos())
               if (repository != null)
                  yield return repository;

         if (includeOther)
            foreach (var repository in GetRegistered().RepositoriesArray.Where(r => !r.Disabled).OrderByDescending(r => r.Type).Select(TryCreateRepo))
               if (repository != null)
               yield return repository;
      }

      public RegisteredRepositories GetRegisteredRepositories()
      {
         return GetRegistered();
      }


      private IRepository TryCreateRepo(RegisteredRepository repo)
      {
         if (!repo.Uri.StartsWith("http") && !Directory.Exists(repo.Uri))
            return null;

         if (repo.Type == RepositoryType.NuGet)
            return new NuGetFileSystemRepo(repo.Uri, repo.Name, RepositoryType.NuGet) {CanPublish = repo.UseForPublishing, ApiKey = repo.ApiKey};

         return new FileSystemRepository(_packageReaderFactory, repo.Uri, repo.Name, RepositoryType.Pundit) {CanPublish = repo.UseForPublishing};
      }

      private RegisteredRepositories GetRegistered()
      {
         if (_registeredRepositories != null)
            return _registeredRepositories;

         if (!File.Exists(_repoConfigPath))
            return new RegisteredRepositories();

         using (var stream = File.OpenRead(_repoConfigPath))
         {
            var repos = _serializer.Read<XmlRegisteredRepositories>(stream);
            _registeredRepositories = repos.ToRegisteredRepositories();
         }

         return _registeredRepositories;
      }

      private string ResolveRootPath()
      {
         var rootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), PunditConfigFolder);

         if (!Directory.Exists(rootPath))
            Directory.CreateDirectory(rootPath).Attributes |= (FileAttributes.System | FileAttributes.Hidden);

         var path = Path.Combine(rootPath, PunditCacheRepoFolder);

         if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

         path = Path.Combine(rootPath, NuGetCacheRepoFolder);

         if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

         return rootPath;
      }
   }
}