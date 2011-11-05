using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Application.Console.Commands
{
   class PublishConsoleCommand : BaseConsoleCommand
   {
      public PublishConsoleCommand(IConsoleOutput console, string currentDirectory, string[] args)
         : base(console, currentDirectory, args)
      {}

      private ICollection<string> GetPackages()
      {
         string packageName = GetParameter("p:|package:", 0);
         //get package
         if (packageName != null)
         {
            packageName = Path.GetFullPath(packageName);

            if (!File.Exists(packageName))
               throw new ArgumentException("package [" + packageName + "] does not exist");

            return new[] { packageName };
         }
         else
         {
            FileInfo[] packedFiles =
               new DirectoryInfo(CurrentDirectory).GetFiles("*" + Package.PackedExtension);

            if (packedFiles.Length == 0)
               throw new ApplicationException("no packages found for publishing");

            return new List<string>(packedFiles.Select(fi => fi.FullName));
         }

      }

      private ICollection<Repo> GetRepos()
      {
         string repoName = GetParameter("r:|repository:");

         //get repo uri
         if(repoName == null)
         {
            var r = new List<Repo>(LocalConfiguration.RepositoryManager.PublishingRepositories);
            if(r.Count == 0) throw new ApplicationException("no repositories found for publishing");
            return r;
         }
         else
         {
            Repo r = LocalConfiguration.RepositoryManager.GetRepositoryByTag(repoName);
            if(r == null) throw new FileNotFoundException("repository " + repoName + " does not exist");
            if(!r.UseForPublishing) throw new ApplicationException("repository does not support publishing");
            return new[] {r};
         }
      }

      public override void Execute()
      {
         ICollection<string> packages = GetPackages();
         bool localOnly = GetBoolParameter(false, "l|local");

         if (localOnly)
         {
            console.WriteLine("publishing to local repository...");
            long sizeBefore = LocalConfiguration.RepositoryManager.OccupiedBinarySpace;

            foreach (string packagePath in packages)
            {
               using (Stream package = File.OpenRead(packagePath))
               {
                  LocalConfiguration.RepositoryManager.LocalRepository.Put(package);
               }
            }

            long sizeAfter = LocalConfiguration.RepositoryManager.OccupiedBinarySpace;

            console.WriteLine("done, local cache increased from {0} to {1} (+{2})",
               PathUtils.FileSizeToString(sizeBefore),
               PathUtils.FileSizeToString(sizeAfter),
               PathUtils.FileSizeToString(sizeAfter - sizeBefore));
         }
         else
         {
            ICollection<Repo> repos = GetRepos();

            console.WriteLine("Publishing package to {0} repository(ies)", repos.Count);

            foreach (Repo repo in repos)
            {
               foreach (string packagePath in packages)
               {
                  console.WriteLine(string.Format("publishing package {0} to repository [{1}]", packagePath, repo.Tag));

                  throw new NotImplementedException();

                  /*IRepository ir = RepositoryFactory.Create(repo);

                  using (Stream package = File.OpenRead(packagePath))
                  {
                     ir.Publish(package);
                  }*/

                  console.WriteLine("published");
               }
            }
         }
      }
   }
}
