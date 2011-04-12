using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using log4net;
using Pundit.Core.Application.Repository;
using Pundit.Core.Model;
using log4net;

namespace Pundit.Console.Repository
{
   class LocalRepository : FileRepository
   {
      private const string LocalRepositoryRootEnv = "PUNDIT_ROOT";
      private const string LocalRepositoryDirName = ".pundit";
      private const string LocalRepositoryDataDirName = "repository";
      private const string RepoXmlFileName = "repositories.xml";

      private static readonly string LocalRepoRoot;     //path to local repository root (not the file folder)
      private static readonly string LocalRepoFileRoot; //path for file repository
      private static readonly RegisteredRepositories Repos;
      private static readonly ILog Log = LogManager.GetLogger(typeof (LocalRepository));

      public LocalRepository() : base(LocalRepoFileRoot)
      {
      }

      static LocalRepository()
      {
         ResolveRootPath(out LocalRepoRoot, out LocalRepoFileRoot);

         Repos = LoadRegisteredRepositories();

         if(Log.IsDebugEnabled) Log.Debug("registered repositories: " + Repos.Names.Length);
      }

      public static RegisteredRepositories Registered { get { return Repos; } }

      private static void ResolveRootPath(out string localRepoRoot, out string localRepoFileRoot)
      {
         string path = Environment.GetEnvironmentVariable(LocalRepositoryRootEnv);

         if(!string.IsNullOrEmpty(path) && Directory.Exists(path))
         {
         }
         else
         {
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), LocalRepositoryDirName);

            if(!Directory.Exists(path))
            {
               DirectoryInfo dir1 = Directory.CreateDirectory(path);
               dir1.Attributes |= (FileAttributes.System | FileAttributes.Hidden);
            }
         }

         localRepoRoot = path;

         path = Path.Combine(path, LocalRepositoryDataDirName);

         if (!Directory.Exists(path))
         {
            Directory.CreateDirectory(path);
         }

         localRepoFileRoot = path;
      }

      private static RegisteredRepositories LoadRegisteredRepositories()
      {
         string repoTxtPath = Path.Combine(LocalRepoRoot, RepoXmlFileName);

         if(Log.IsDebugEnabled) Log.Debug("Loading registered repositories from " + repoTxtPath);

         if (File.Exists(repoTxtPath))
         {
            return RegisteredRepositories.LoadFrom(repoTxtPath);
         }

         return new RegisteredRepositories();
      }

      public static bool IsValidRepositoryName(string name)
      {
         return name != null && (name == RegisteredRepositories.LocalRepositoryName || Repos.ContainsRepository(name));
      }

      public static string GetRepositoryUriFromName(string name)
      {
         if(!IsValidRepositoryName(name))
            throw new ArgumentException("Invalid repository name [" + name + "]", "name");

         if (RegisteredRepositories.LocalRepositoryName == name)
            return LocalRepoFileRoot;

         return Repos[name];
      }
   }
}
