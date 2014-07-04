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
      //private const string LocalRepositoryFileName = ".punditdb";
      //internal const string LocalRepositoryTag = "local";
      //internal const string LocalRepositoryUri = "local";

      private static string _localDir;
      //private static IRepositoryManager _mgr;

      /// <summary>
      /// Occurs on various events when a package is downloading to local repository
      /// </summary>
      public static event EventHandler<PackageDownloadEventArgs> PackageDownloadToLocalRepository;

      static LocalConfiguration()
      {
         Initialize();
      }

      /// <summary>
      /// Repository management functions
      /// </summary>
      public static IRepositoryManager RepositoryManager
      {
         get { return null; }
      }

      private static void Initialize()
      {
         _localDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

         //_mgr = new DiskRepositoryManager(_localDir);
      }

      /// <summary>
      /// Filters packages giving only ones that are missing from the local archive
      /// </summary>
      /// <param name="packages"></param>
      /// <returns></returns>
      /// <exception cref="ArgumentNullException"></exception>
      public static ICollection<PackageKey> GetForDownload(IEnumerable<PackageKey> packages)
      {
         return null;

         /*if (packages == null) throw new ArgumentNullException("packages");

         ILocalRepository localRepo = _mgr.LocalRepository;

         PackageKey[] packagesArray = packages.ToArray();
         bool[] existance = localRepo.BinariesExists(packagesArray).ToArray();
         var r = new List<PackageKey>();

         for (int i = 0; i < packagesArray.Length; i++)
         {
            PackageKey pck = packagesArray[i];
            if(!existance[i]) r.Add(pck);
         }

         return r;*/
      }

      /// <summary>
      /// Downloads specified packages to the local repository, doesn't check if they already exist
      /// </summary>
      /// <param name="packages"></param>
      public static void DownloadLocally(IEnumerable<PackageKey> packages)
      {
         /*if (packages == null) throw new ArgumentNullException("packages");
         ILocalRepository localRepo = _mgr.LocalRepository;

         foreach (PackageKey pck in packages)
         {
            Repo downRepoMeta = localRepo.FindOnlineRepository(pck);
            if(downRepoMeta == null)
            {
               throw new InvalidOperationException("can't find repository for key " + pck);
            }
            IRemoteRepository downRepo = RemoteRepositoryFactory.Create(downRepoMeta.Uri, downRepoMeta.Login, downRepoMeta.ApiKey);

            using (Stream pckStream = downRepo.Download(pck.Platform, pck.PackageId, pck.VersionString))
            {
               long sourceLen = pckStream.Length;
               var closure = new DownloadProgressClosure(pck, sourceLen, Raise);
               localRepo.Put(pckStream, closure.PutCallback);
            }
         }*/
      }

      private static void Raise(PackageDownloadEventArgs args)
      {
         if(PackageDownloadToLocalRepository != null && args != null)
         {
            PackageDownloadToLocalRepository(null, args);
         }
      }
   }

   class DownloadProgressClosure
   {
      private readonly PackageKey _key;
      private readonly long _totalSize;
      private readonly Action<PackageDownloadEventArgs> _eventForwarder;
      private long _readBefore;
      private readonly AvgSpeedMeasure _speed = new AvgSpeedMeasure();

      public DownloadProgressClosure(PackageKey key, long totalSize, Action<PackageDownloadEventArgs> eventForwarder)
      {
         if (key == null) throw new ArgumentNullException("key");
         if (eventForwarder == null) throw new ArgumentNullException("eventForwarder");

         _key = key;
         _totalSize = totalSize;
         _eventForwarder = eventForwarder;
      }

      public void PutCallback(long read)
      {
         _speed.Slice(read - _readBefore);
         _readBefore = read;
         var args = new PackageDownloadEventArgs(_key, true, _totalSize, read, _speed.BytesPerSecond);
         _eventForwarder(args);
      }
   }
}
