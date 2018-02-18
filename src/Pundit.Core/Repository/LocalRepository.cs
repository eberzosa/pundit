using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Resolvers;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;

namespace EBerzosa.Pundit.Core.Repository
{
   // TO DELETE
   public class LocalRepository
   {
      private readonly IRepository _repo;
      
      /// <summary>
      /// Occurs when a package is started downloading to the local repository
      /// </summary>
      public event EventHandler<PackageKeyEventArgs> PackageDownloadToLocalRepositoryStarted;

      /// <summary>
      /// Occurs when a package is finished downloading to the local repository
      /// </summary>
      public event EventHandler<PackageKeyEventArgs> PackageDownloadToLocalRepositoryFinished;

      public LocalRepository(IRepository repo)
      {
         _repo = repo;
      }

      /// <summary>
      /// Downloads specified packages to the local repository
      /// </summary>
      /// <param name="packages"></param>
      public void DownloadLocally(IEnumerable<SatisfyingInfoExtended> packages)
      {
         SatisfyingInfoExtended[] packagesArray = packages.ToArray();
         bool[] existance = _repo.PackagesExist(packagesArray.Select(p => p.GetPackageKey()).ToArray());

         for(int i = 0; i < packagesArray.Length; i++)
         {
            if (existance[i])
               continue;

            bool downloaded = false;

            try
            {
               using (Stream pckStream = packagesArray[i].Repo.Download(packagesArray[i].GetPackageKey()))
               {
                  PackageDownloadToLocalRepositoryStarted?.Invoke(null, new PackageKeyEventArgs(packagesArray[i].GetPackageKey(), true));
                  _repo.Publish(pckStream);

                  downloaded = true;
               }
            }
            catch (FileNotFoundException)
            {
            }

            PackageDownloadToLocalRepositoryFinished?.Invoke(null, new PackageKeyEventArgs(packagesArray[i].GetPackageKey(), downloaded));
         }
      }
   }
}
