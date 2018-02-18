using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Repository.Xml;
using EBerzosa.Pundit.Core.Serializers;
using Mapster;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;

namespace EBerzosa.Pundit.Core.Repository
{
   /// <summary>
   /// Represents local repository and utility methods to work with it
   /// </summary>
   public class LocalRepository
   {
      private readonly RepositoryFactory _repositoryFactory;
      private readonly ISerializer _serializer;
      private const string LocalRepositoryRootEnv = "PUNDIT_ROOT";
      private const string LocalRepositoryDirName = ".pundit";
      private const string LocalRepositoryDataDirName = "repository";
      private const string RepoXmlFileName = "repositories.xml";

      private readonly string LocalRepoRoot;     //path to local repository root (not the file folder)
      private readonly string LocalRepoFileRoot; //path for file repository
      private readonly RegisteredRepositories Repos;

      /// <summary>
      /// Occurs when a package is started downloading to the local repository
      /// </summary>
      public event EventHandler<PackageKeyEventArgs> PackageDownloadToLocalRepositoryStarted;

      /// <summary>
      /// Occurs when a package is finished downloading to the local repository
      /// </summary>
      public event EventHandler<PackageKeyEventArgs> PackageDownloadToLocalRepositoryFinished;

      public LocalRepository(RepositoryFactory repositoryFactory, ISerializer serializer)
      {
         _repositoryFactory = repositoryFactory;
         _serializer = serializer;
         ResolveRootPath(out LocalRepoRoot, out LocalRepoFileRoot);

         Repos = LoadRegisteredRepositories();
      }

      public string GlobalRootPath => LocalRepoRoot;

      public string GlobalRootFilePath => LocalRepoFileRoot;

      public string GlobalSettingsFilePath => Path.Combine(LocalRepoRoot, RepoXmlFileName);

      public long OccupiedSpace
      {
         get
         {
            long space = 0;

            var repoTxtPath = Path.Combine(LocalRepoRoot, RepoXmlFileName);

            if (File.Exists(repoTxtPath))
               space += new FileInfo(repoTxtPath).Length;

            space += new DirectoryInfo(GlobalRootFilePath).GetFiles("*", SearchOption.AllDirectories).Sum(fi => fi.Length);

            return space;
         }
      }

      public RegisteredRepositories Registered => Repos;


      public bool IsValidRepositoryName(string name) 
         => name != null && (name == RegisteredRepositories.LocalRepositoryName || Repos.ContainsRepository(name));

      public bool CanPublish(string name) 
         => Repos.RepositoriesArray.First(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.UseForPublishing ?? false;

      public string GetRepositoryUriFromName(string name)
      {
         if(!IsValidRepositoryName(name))
            throw new ArgumentException("Invalid repository name [" + name + "]", nameof(name));

         return RegisteredRepositories.LocalRepositoryName == name ? LocalRepoFileRoot : Repos[name];
      }

      public IEnumerable<string> TakeFirstRegisteredNames(int reposToTake, bool includeLocal)
      {
         var names = new List<string>();

         if (includeLocal)
         {
            names.Add(RegisteredRepositories.LocalRepositoryName);
            reposToTake--;
         }

         for (int i = 0; i < Math.Min(reposToTake, Registered.TotalCount); i++)
            names.Add(Registered[i]);

         return names;
      }

      /// <summary>
      /// Downloads specified packages to the local repository
      /// </summary>
      /// <param name="packages"></param>
      /// <param name="activeRepositories"></param>
      public void DownloadLocally(IEnumerable<PackageKey> packages, IEnumerable<IRepository> activeRepositories)
      {
         IRepository localRepo =
            _repositoryFactory.CreateFromUri(GetRepositoryUriFromName(RegisteredRepositories.LocalRepositoryName));

         PackageKey[] packagesArray = packages.ToArray();
         bool[] existance = localRepo.PackagesExist(packagesArray);

         for(int i = 0; i < packagesArray.Length; i++)
         {
            PackageKey pck = packagesArray[i];

            if (existance[i])
               continue;

            bool downloaded = false;

            foreach (var activeRepository in activeRepositories)
            {
               try
               {
                  using (Stream pckStream = activeRepository.Download(pck))
                  {
                     PackageDownloadToLocalRepositoryStarted?.Invoke(null, new PackageKeyEventArgs(pck, true));
                     localRepo.Publish(pckStream);
                  }

                  downloaded = true;
                  break;
               }
               catch (FileNotFoundException)
               {
               }
            }

            PackageDownloadToLocalRepositoryFinished?.Invoke(null, new PackageKeyEventArgs(pck, downloaded));
         }
      }

      private void ResolveRootPath(out string localRepoRoot, out string localRepoFileRoot)
      {
         string path = Environment.GetEnvironmentVariable(LocalRepositoryRootEnv);

         if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
         {
         }
         else
         {
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), LocalRepositoryDirName);

            if (!Directory.Exists(path))
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

      private RegisteredRepositories LoadRegisteredRepositories()
      {
         var repoTxtPath = Path.Combine(LocalRepoRoot, RepoXmlFileName);

         if (!File.Exists(repoTxtPath))
            return new RegisteredRepositories();

         using (var stream = File.OpenRead(repoTxtPath))
         {
            var repos = _serializer.Read<XmlRegisteredRepositories>(stream);
            return repos.Adapt<XmlRegisteredRepositories, RegisteredRepositories>();
         }
      }
   }
}
