using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;

namespace Pundit.Core
{
   /// <summary>
   /// Represents local configuration and is the entry point to any other functionality.
   /// </summary>
   public static class LocalConfiguration
   {
      /// <summary>
      /// Environment variable storing absolute directory path to local database (excluding file name).
      /// If set, pundit will prefer this directory
      /// </summary>
      public const string LocalRepositoryRootVar = "PUNDIT_ROOT";

      private const string LocalRepositoryFileName = ".punditdb";
      internal const string LocalRepositoryTag = "local";
      internal const string LocalRepositoryUri = "local";

      private static string _localDir;
      private static string _localFile;
      private static IRepositoryManager _mgr;

      /// <summary>
      /// Occurs on various events when a package is downloading to local repository
      /// </summary>
      public static event EventHandler<PackageDownloadEventArgs> PackageDownloadToLocalRepository;

      static LocalConfiguration()
      {
         Initialize();
      }

      /// <summary>
      /// Gets local database location string. Use only for display purposes, never rely on it in logic
      /// </summary>
      public static string DbLocation
      {
         get { return _localFile; }
      }

      /// <summary>
      /// Repository management functions
      /// </summary>
      public static IRepositoryManager RepositoryManager
      {
         get { return _mgr; }
      }

      private static void Initialize()
      {
         _localDir = Environment.GetEnvironmentVariable(LocalRepositoryRootVar);

         if(!string.IsNullOrEmpty(_localDir) && Directory.Exists(_localDir))
         {
         }
         else
         {
            _localDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
         }

         _localFile = Path.Combine(_localDir, LocalRepositoryFileName);
         _mgr = new RepositoryManager(_localFile);
      }

      /// <summary>
      /// Filters packages giving only ones that are missing from the local archive
      /// </summary>
      /// <param name="packages"></param>
      /// <returns></returns>
      /// <exception cref="ArgumentNullException"></exception>
      public static ICollection<PackageKey> GetForDownload(IEnumerable<PackageKey> packages)
      {
         if (packages == null) throw new ArgumentNullException("packages");

         ILocalRepository localRepo = _mgr.LocalRepository;

         PackageKey[] packagesArray = packages.ToArray();
         bool[] existance = localRepo.BinariesExists(packagesArray).ToArray();
         var r = new List<PackageKey>();

         for (int i = 0; i < packagesArray.Length; i++)
         {
            PackageKey pck = packagesArray[i];
            if(!existance[i]) r.Add(pck);
         }

         return r;
      }

      /// <summary>
      /// Downloads specified packages to the local repository, doesn't check if they already exist
      /// </summary>
      /// <param name="packages"></param>
      public static void DownloadLocally(IEnumerable<PackageKey> packages)
      {
         if (packages == null) throw new ArgumentNullException("packages");
         ILocalRepository localRepo = _mgr.LocalRepository;

         foreach (PackageKey pck in packages)
         {
            long repoId = localRepo.GetClosestRepositoryId(pck);
            Repo downRepoMeta = _mgr.GetRepositoryById(repoId);
            IRemoteRepository downRepo = RemoteRepositoryFactory.Create(downRepoMeta.Uri);

            using (Stream pckStream = downRepo.Download(pck.Platform, pck.PackageId, pck.VersionString))
            {
               if (PackageDownloadToLocalRepository != null)
                  PackageDownloadToLocalRepository(null, new PackageDownloadEventArgs(pck, true, 1, 0));

               localRepo.Put(pckStream);
            }

            if (PackageDownloadToLocalRepository != null)
               PackageDownloadToLocalRepository(null, new PackageDownloadEventArgs(pck, true, 1, 1));
         }
      }
   }
}
