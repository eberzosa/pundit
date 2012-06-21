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

      private string GetPackageFullName()
      {
         string name = GetParameter("p:|package:", 0);
         if (name != null)
         {
            name = Path.GetFullPath(name);

            if (!File.Exists(name))
               throw new ArgumentException("package [" + name + "] does not exist", "package");
         }
         else
         {
            throw new ArgumentException("package not specified", "package");
         }
         return name;
      }
      private Repo GetRepository()
      {
         string tag = GetParameter("r:|repository-name:");
         if(tag == null) throw new ArgumentNullException("repository name not specified", "repository-name");
         Repo r = LocalConfiguration.RepositoryManager.GetRepositoryByTag(tag);
         if(r == null) throw new ArgumentOutOfRangeException("repository [" + tag + "] not registered", "repository-name");
         return r;
      }

      public override void Execute()
      {
         //System.Console.ReadKey();
         string packagePath = GetPackageFullName();
         Repo repo = GetRepository();

         console.WriteLine(string.Format("publishing package {0} to repository [{1}]", packagePath, repo.Tag));

         IRemoteRepository remote = RemoteRepositoryFactory.Create(repo.Uri);
         using (Stream package = File.OpenRead(packagePath))
         {
            remote.Publish(package);
         }

         console.WriteLine("published!");
      }
   }
}
