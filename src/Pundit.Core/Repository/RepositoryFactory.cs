using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Converters;
using EBerzosa.Pundit.Core.Package;
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
      private ICollection<IRepository> _cacheRepositories;
      private ICollection<IRepository> _standardRepositories;


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


      public IRepository TryGetEnabledRepo(string name)
      {
         return TryGetEnabledRepos(RepositoryScope.Any).FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
      }

      public ICollection<IRepository> TryGetEnabledRepos(RepositoryScope scope)
      {
         var repos = new List<IRepository>();

         if ((scope & RepositoryScope.Cache) == RepositoryScope.Cache)
            repos.AddRange(GetCacheRepos());

         if ((scope & RepositoryScope.Standard) == RepositoryScope.Standard)
            repos.AddRange(GetStandardRepos());

         return repos;
      }

      public IRepository GetCustomRepository(string path)
      {
         return TryCreateRepo(new RegisteredRepository {Uri = path, Name = "ManualRepo", Type = RepositoryType.NuGet});
      }

      public RegisteredRepositories GetRegisteredRepositories()
      {
         return GetRegistered();
      }


      private ICollection<IRepository> GetCacheRepos()
      {
         if (_cacheRepositories != null)
            return _cacheRepositories;

         _cacheRepositories = new[]
            {
               TryCreateRepo(new RegisteredRepository
               {
                  Uri = _cacheNuGetRepoPath,
                  Name = NuGetCacheRepoName,
                  UseForPublishing = true,
                  Type = RepositoryType.NuGet
               }),
               TryCreateRepo(new RegisteredRepository
               {
                  Uri = _cacheRepoPath,
                  Name = PunditCacheRepoName,
                  UseForPublishing = true
               })
            }
            .Where(r => r != null)
            .ToArray();

         return _cacheRepositories;
      }

      private ICollection<IRepository> GetStandardRepos()
      {
         if (_standardRepositories != null)
            return _standardRepositories;

         _standardRepositories = GetRegistered().RepositoriesArray
            .Where(r => !r.Disabled)
            .OrderByDescending(r => r.Type)
            .Select(TryCreateRepo)
            .Where(r => r != null)
            .ToArray();

         return _standardRepositories;
      }

      private IRepository TryCreateRepo(RegisteredRepository repo)
      {
         if (!repo.Uri.StartsWith("http") && !Directory.Exists(repo.Uri))
            return null;

         if (repo.Type == RepositoryType.NuGet)
            return new NuGetFileSystemRepo(repo.Uri, repo.Name) {CanPublish = repo.UseForPublishing, ApiKey = repo.ApiKey, TimeOut = TimeSpan.FromMinutes(5)};

         return new FileSystemRepository(_packageReaderFactory, repo.Uri, repo.Name) {CanPublish = repo.UseForPublishing};
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