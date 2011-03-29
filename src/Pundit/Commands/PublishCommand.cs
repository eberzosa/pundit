using System;
using System.IO;
using log4net;
using NDesk.Options;
using Pundit.Console.Repository;
using Pundit.Core;
using Pundit.Core.Model;

namespace Pundit.Console.Commands
{
   class PublishCommand : ICommand
   {
      private static readonly ILog Log = LogManager.GetLogger(typeof (PublishCommand));
      private readonly string[] _cmdline;

      public PublishCommand(string[] args)
      {
         _cmdline = args;
      }

      private void ResolveParameters(out string packagePath, out string repositoryName)
      {
         string pi = null;
         string ri = null;

         OptionSet oset = new OptionSet()
            .Add("i:|input:", i => pi = i)
            .Add("r:|repository:", r => ri = r);

         oset.Parse(_cmdline);

         //get package)))
         if(pi != null)
         {
            packagePath = pi;

            if(!File.Exists(pi))
               throw new ArgumentException("package [" + pi + "] does not exist");
         }
         else
         {
            FileInfo[] packedFiles =
               new DirectoryInfo(Environment.CurrentDirectory).GetFiles("*" + Package.PackedExtension);

            if (packedFiles.Length > 1)
               throw new ArgumentException("more than one candidate found for publishing, did you mean " +
                                           packedFiles[0].Name + "?");

            packagePath = packedFiles[0].FullName;
         }

         //get repo uri
         if(ri == null)
         {
            repositoryName = "local";
         }
         else
         {
            repositoryName = ri;
         }
      }

      public void Execute()
      {
         string packagePath, repoName;

         ResolveParameters(out packagePath, out repoName);

         if(!LocalRepository.RegisteredRepositories.ContainsKey(repoName))
            throw new ArgumentException("repository [" + repoName + "] is not registered");

         Log.Info(string.Format("publishing package {0} to repository [{1}]", packagePath, repoName));

         string uri = LocalRepository.RegisteredRepositories[repoName];

         Log.Info(string.Format("repository URI: {0}", uri));

         IRepository repo = RepositoryFactory.CreateFromUri(uri);

         using(Stream package = File.OpenRead(packagePath))
         {
            repo.Publish(package);   
         }

         Log.Info("published");
      }
   }
}
