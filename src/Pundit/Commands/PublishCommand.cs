using System;
using System.IO;
using System.Linq;
using log4net;
using NDesk.Options;
using Pundit.Console.Repository;
using Pundit.Core;
using Pundit.Core.Model;

namespace Pundit.Console.Commands
{
   class PublishCommand : BaseCommand
   {
      public PublishCommand(string[] args) : base(args)
      {}

      private void ResolveParameters(out string[] packages, out string[] repositoryName)
      {
         string pi = null;
         string ri = null;

         OptionSet oset = new OptionSet()
            .Add("p:|package:", i => pi = i)
            .Add("r:|repository:", r => ri = r);

         oset.Parse(GetCommandLine());

         //get package)))
         if(pi != null)
         {
            pi = Path.GetFullPath(pi);

            if(!File.Exists(pi))
               throw new ArgumentException("package [" + pi + "] does not exist");

            packages = new[] { pi };
         }
         else
         {
            FileInfo[] packedFiles =
               new DirectoryInfo(Environment.CurrentDirectory).GetFiles("*" + Package.PackedExtension);

            if(packedFiles.Length == 0)
               throw new ApplicationException("no packages found for publishing");

            packages = packedFiles.Select(fi => fi.FullName).ToArray();
         }

         //get repo uri
         if(ri == null)
         {
            repositoryName = LocalRepository.Registered.PublishingNames;
         }
         else
         {
            repositoryName = new[] {ri};
         }
      }

      public override void Execute()
      {
         string[] packages;
         string[] repoNames;

         ResolveParameters(out packages, out repoNames);

         foreach (string rn in repoNames)
         {
            if (!LocalRepository.IsValidRepositoryName(rn))
               throw new ArgumentException("repository [" + rn + "] does not exist");
         }

         Log.InfoFormat("Publishing package to {0} repository(ies)", repoNames.Length);

         foreach (string rn in repoNames)
         {
            foreach (string packagePath in packages)
            {
               Log.Info(string.Format("publishing package {0} to repository [{1}]", packagePath, rn));

               string uri = LocalRepository.GetRepositoryUriFromName(rn);

               Log.Info(string.Format("repository URI: {0}", uri));

               IRepository repo = RepositoryFactory.CreateFromUri(uri);

               using (Stream package = File.OpenRead(packagePath))
               {
                  repo.Publish(package);
               }

               Log.Info("published");
            }
         }
      }
   }
}
