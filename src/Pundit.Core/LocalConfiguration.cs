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
      /// Downloads specified packages to the local repository
      /// </summary>
      /// <param name="packages"></param>
      /// <param name="activeRepositories"></param>
      public static void DownloadLocally(IEnumerable<PackageKey> packages, IEnumerable<IRepository> activeRepositories)
      {
         IRepository localRepo = _mgr.LocalRepository;

         PackageKey[] packagesArray = packages.ToArray();
         bool[] existance = localRepo.PackagesExist(packagesArray);

         for(int i = 0; i < packagesArray.Length; i++)
         {
            PackageKey pck = packagesArray[i];

            if(existance[i])
            {
               //package already here
               //Log.InfoFormat("[+] {0}", pck);
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
                        if(PackageDownloadToLocalRepository != null)
                           PackageDownloadToLocalRepository(null, new PackageDownloadEventArgs(pck, true, 1, 0));

                        localRepo.Publish(pckStream);
                     }

                     downloaded = true;

                     break;
                  }
                  catch(FileNotFoundException)
                  {
                     
                  }
               }

               if (PackageDownloadToLocalRepository != null)
                  PackageDownloadToLocalRepository(null, new PackageDownloadEventArgs(pck, downloaded, 1, 1));
            }
         }
      }
   }
}
