using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Package;
using EBerzosa.Pundit.Core.Resolvers;
using Pundit.Core.Model.EventArguments;

namespace EBerzosa.Pundit.Core.Repository
{
   // TO DELETE
   public class CacheRepository
   {
      private readonly ICollection<IRepository> _repos;
      
      /// <summary>
      /// Occurs when a package is started downloading to the local repository
      /// </summary>
      public event EventHandler<PackageKeyEventArgs> PackageDownloadToCacheRepositoryStarted;

      /// <summary>
      /// Occurs when a package is finished downloading to the local repository
      /// </summary>
      public event EventHandler<PackageKeyEventArgs> PackageDownloadToCacheRepositoryFinished;

      public CacheRepository(ICollection<IRepository> repos)
      {
         _repos = repos;
      }

      /// <summary>
      /// Downloads specified packages to the local repository
      /// </summary>
      /// <param name="packages"></param>
      public void DownloadLocally(IEnumerable<SatisfyingInfoExtended> packages)
      {
         SatisfyingInfoExtended[] packagesArray = packages.ToArray();

         foreach (SatisfyingInfoExtended info in packagesArray)
         {
            bool downloaded = false;

            PackageDownloadToCacheRepositoryStarted?.Invoke(null, new PackageKeyEventArgs(info.GetPackageKey(), true));

            foreach (var repo in _repos)
            {
               if (!CanCache(info, repo))
                  continue;

               if (repo.PackageExist(info.GetPackageKey()))
               {
                  downloaded = true;
                  continue;
               }

               string tempFileName = null;

               try
               {
                  using (var pckStream = info.Repo.Download(info.GetPackageKey()))
                  {
                     tempFileName = Path.Combine(Path.GetTempPath(), info.GetPackageKey().GetFileName());

                     if (File.Exists(tempFileName))
                        File.Delete(tempFileName);

                     using (var writer = File.OpenWrite(tempFileName))
                        pckStream.CopyTo(writer);

                     repo.Publish(tempFileName);

                     downloaded = true;
                  }
               }
               catch 
               {
                  if (tempFileName != null && File.Exists(tempFileName))
                     File.Delete(tempFileName);
               }

               break;
            }

            PackageDownloadToCacheRepositoryFinished?.Invoke(null, new PackageKeyEventArgs(info.GetPackageKey(), downloaded));
         }
      }

      private bool CanCache(SatisfyingInfoExtended info, IRepository repo) => info.Repo.Type == repo.Type;
   }
}
