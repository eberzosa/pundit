using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using Pundit.Core.Application.Repository;
using Pundit.Core.Model;

namespace Pundit.Core
{
   public class LocalRepository : FileRepository
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

      public static string GlobalRootPath
      {
         get { return LocalRepoRoot; }
      }

      public static string GlobalRootFilePath
      {
         get { return LocalRepoFileRoot; }
      }

      public static string GlobalSettingsFilePath
      {
         get { return Path.Combine(LocalRepoRoot, RepoXmlFileName); }
      }

      public static long OccupiedSpace
      {
         get
         {
            long space = 0;

            string repoTxtPath = Path.Combine(LocalRepoRoot, RepoXmlFileName);

            if (File.Exists(repoTxtPath)) space += new FileInfo(repoTxtPath).Length;

            foreach(FileInfo fi in new DirectoryInfo(GlobalRootFilePath).GetFiles("*", SearchOption.AllDirectories))
            {
               space += fi.Length;
            }

            return space;
         }
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

      public static IEnumerable<string> TakeFirstRegisteredNames(int reposToTake, bool includeLocal)
      {
         var names = new List<string>();

         if(includeLocal)
         {
            names.Add(RegisteredRepositories.LocalRepositoryName);
            reposToTake--;
         }

         for(int i = 0; i < Math.Min(reposToTake, Registered.TotalCount); i++)
         {
            names.Add(Registered[i]);
         }

         return names;
      }

      /// <summary>
      /// Downloads specified packages to the local repository
      /// </summary>
      /// <param name="packages"></param>
      /// <param name="activeRepositories"></param>
      public static void DownloadLocally(IEnumerable<PackageKey> packages, IEnumerable<IRepository> activeRepositories)
      {
         IRepository localRepo =
            RepositoryFactory.CreateFromUri(GetRepositoryUriFromName(RegisteredRepositories.LocalRepositoryName));

         PackageKey[] packagesArray = packages.ToArray();
         bool[] existance = localRepo.PackagesExist(packagesArray);

         for(int i = 0; i < packagesArray.Length; i++)
         {
            PackageKey pck = packagesArray[i];

            if(existance[i])
            {
               Log.InfoFormat("[+] {0}", pck);
            }
            else
            {
               bool downloaded = false;

               foreach(IRepository activeRepository in activeRepositories)
               {
                  try
                  {
                     using (Stream pckStream = activeRepository.Download(pck))
                     {
                        Log.InfoFormat("Downoading {0} to the local repository", pck);

                        localRepo.Publish(pckStream);
                     }

                     downloaded = true;
                     break;
                  }
                  catch(FileNotFoundException)
                  {
                     
                  }
               }

               if(!downloaded)
               {
                  throw new ApplicationException("could not find package in any repository: " + pck);
               }
            }
         }
      }
   }
}
