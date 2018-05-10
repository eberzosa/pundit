﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Application;
using EBerzosa.Pundit.Core.Repository.Xml;
using EBerzosa.Pundit.Core.Serializers;
using EBerzosa.Utils;
using Mapster;
using NuGet.Configuration;

namespace EBerzosa.Pundit.Core.Repository
{
   public class RepositoryFactory
   {
      private const string PunditConfigFolder = ".pundit";
      private const string LocalRepoFolder = "repository";
      private const string RepositoriesConfigFile = "repositories.xml";

      private readonly PackageReaderFactory _packageReaderFactory;
      private readonly ISerializer _serializer;

      private readonly string _cacheRepoPath;
      private readonly string _repoConfigPath;
      
      private RegisteredRepositories _registeredRepositories;


      public RepositoryFactory(PackageReaderFactory packageReaderFactory, ISerializer serializer)
      {
         Guard.NotNull(packageReaderFactory, nameof(packageReaderFactory));
         Guard.NotNull(serializer, nameof(serializer));

         _packageReaderFactory = packageReaderFactory;
         _serializer = serializer;

         var punditConfigPath = ResolveRootPath(PunditConfigFolder, LocalRepoFolder);
         _cacheRepoPath = Path.Combine(punditConfigPath, LocalRepoFolder);
         _repoConfigPath = Path.Combine(punditConfigPath, RepositoriesConfigFile);
      }


      public ICollection<IRepository> GetCacheRepos()
      {
         return new[]
         {
            CreateRepo(new RegisteredRepository {Uri = SettingsUtility.GetGlobalPackagesFolder(NullSettings.Instance), Name = "nugetlocal", UseForPublishing = true, Type = RepositoryType.NuGet}),
            CreateRepo(new RegisteredRepository {Uri = _cacheRepoPath, Name = "local", UseForPublishing = true})
         };
      }
      
      public IRepository GetRepo(string name)
      {
         var repo = GetRegistered().RepositoriesArray.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

         return repo == null ? null : CreateRepo(repo);
      }

      public IEnumerable<IRepository> GetEnabledRepos(bool includeCache, bool includeOther)
      {
         if (includeCache)
            foreach (var repository in GetCacheRepos())
               yield return repository;

         if (includeOther)
            foreach (var repository in GetRegistered().RepositoriesArray.Where(r => !r.Disabled).OrderByDescending(r => r.Type).Select(CreateRepo))
               yield return repository;
      }

      public RegisteredRepositories GetRegisteredRepositories()
      {
         return GetRegistered();
      }


      private IRepository CreateRepo(RegisteredRepository repo)
      {
          if (repo.Type == RepositoryType.NuGet)
              return new NuGetFileSystemRepo(repo.Uri, repo.Name, RepositoryType.NuGet) {CanPublish = repo.UseForPublishing};

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
            _registeredRepositories = repos.Adapt<XmlRegisteredRepositories, RegisteredRepositories>();
         }

         return _registeredRepositories;
      }

      private string ResolveRootPath(string punditConfigFolder, string localRepoFolder)
      {
         var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), punditConfigFolder);

         if (!Directory.Exists(path))
            Directory.CreateDirectory(path).Attributes |= (FileAttributes.System | FileAttributes.Hidden);

         var rootPath = path;

         path = Path.Combine(path, localRepoFolder);

         if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

         return rootPath;
      }
   }
}
