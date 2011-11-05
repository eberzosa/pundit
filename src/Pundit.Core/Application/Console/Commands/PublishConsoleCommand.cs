using System;
using System.IO;
using System.Linq;
using Pundit.Core.Model;

namespace Pundit.Core.Application.Console.Commands
{
   class PublishConsoleCommand : BaseConsoleCommand
   {
      public PublishConsoleCommand(IConsoleOutput console, string currentDirectory, string[] args)
         : base(console, currentDirectory, args)
      {}

      private void ResolveParameters(out string[] packages, out Repo[] repos)
      {
         string packageName = GetParameter("p:|package:", 0);
         string repoName = GetParameter("r:|repository:");

         //get package
         if(packageName != null)
         {
            packageName = Path.GetFullPath(packageName);

            if(!File.Exists(packageName))
               throw new ArgumentException("package [" + packageName + "] does not exist");

            packages = new[] { packageName };
         }
         else
         {
            FileInfo[] packedFiles =
               new DirectoryInfo(CurrentDirectory).GetFiles("*" + Package.PackedExtension);

            if(packedFiles.Length == 0)
               throw new ApplicationException("no packages found for publishing");

            packages = packedFiles.Select(fi => fi.FullName).ToArray();
         }

         //get repo uri
         if(repoName == null)
         {
            repos = LocalConfiguration.RepositoryManager.PublishingRepositories.ToArray();
         }
         else
         {
            Repo r = LocalConfiguration.RepositoryManager.GetRepositoryByTag(repoName);
            if(r == null) throw new FileNotFoundException("repository " + repoName + " does not exist");
            if(!r.UseForPublishing) throw new ApplicationException("repository does not support publishing");
            repos = new[] {r};
         }
      }

      public override void Execute()
      {
         string[] packages;
         Repo[] repos;

         ResolveParameters(out packages, out repos);

         console.WriteLine("Publishing package to {0} repository(ies)", repos.Length);

         foreach (Repo repo in repos)
         {
            foreach (string packagePath in packages)
            {
               console.WriteLine(string.Format("publishing package {0} to repository [{1}]", packagePath, repo.Tag));

               IRepository ir = RepositoryFactory.Create(repo);

               using (Stream package = File.OpenRead(packagePath))
               {
                  ir.Publish(package);
               }

               console.WriteLine("published");
            }
         }
      }
   }
}
