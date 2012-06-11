using Pundit.Core.Model;
using Pundit.Core.Server.Application;
using Pundit.Core.Server.Model;

namespace Pundit.Core.Server
{
   /// <summary>
   /// 
   /// </summary>
   public static class RepositoryFactory
   {
      /// <summary>
      /// Creates <see cref="IRemoteRepository"/> implementtion using sqlite for metadata storage and disk for package storage
      /// </summary>
      /// <param name="packageReposotiry"> </param>
      /// <param name="dataRootDir">Root directory for both sql db and data which must exist. If it's empty, a new db will be created.</param>
      /// <returns></returns>
      public static IRemoteRepository CreateSqlDiskServer(IPackageRepository packageReposotiry, string dataRootDir)
      {
         return new RepositoryServer(packageReposotiry, dataRootDir);
      }
   }
}
