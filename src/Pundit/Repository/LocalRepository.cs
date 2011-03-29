using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using Pundit.Core.Application.Repository;
using Pundit.Core.Model;

namespace Pundit.Console.Repository
{
   class LocalRepository : FileRepository
   {
      private const string LocalRepositoryRootEnv = "PUNDIT_ROOT";
      private const string LocalRepositoryDirName = ".pundit";
      private const string LocalRepositoryDataDirName = "repository";
      private const string RepoXmlFileName = "repositories.xml";

      private static string _localRepoRoot;
      private static readonly Dictionary<string, string> _registeredRepositories = new Dictionary<string, string>();

      public LocalRepository() : base(ResolveRootPath())
      {
      }

      private static string ResolveRootPath()
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

         _localRepoRoot = path;

         path = Path.Combine(path, LocalRepositoryDataDirName);

         if (!Directory.Exists(path))
         {
            Directory.CreateDirectory(path);
         }

         return path;
      }

      public static Dictionary<string, string> RegisteredRepositories
      {
         get
         {
            if (_localRepoRoot == null)
               ResolveRootPath();

            if(_registeredRepositories.Count == 0)
            {
               _registeredRepositories["local"] = Path.Combine(_localRepoRoot, LocalRepositoryDataDirName);

               string repoTxtPath = Path.Combine(_localRepoRoot, RepoXmlFileName);

               if(File.Exists(repoTxtPath))
               {
                  foreach(RegisteredRepository rr in Console.RegisteredRepositories.LoadFrom(repoTxtPath).Repositories)
                  {
                     _registeredRepositories[rr.Name] = rr.Uri;
                  }
               }
            }

            return _registeredRepositories;
         }
      }
   }
}
