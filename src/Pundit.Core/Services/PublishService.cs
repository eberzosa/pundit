using System;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.Core.Services
{
   public class PublishService
   {
      private readonly RepositoryFactory _repositoryFactory;
      private readonly IWriter _writer;

      public string Repository { get; set; }


      public PublishService(RepositoryFactory repositoryFactory, IWriter writer)
      {
         Guard.NotNull(repositoryFactory, nameof(repositoryFactory));
         Guard.NotNull(writer, nameof(writer));
         
         _repositoryFactory = repositoryFactory;
         _writer = writer;
      }

      public void Publish(string packagePath)
      {
         if (!File.Exists(packagePath))
            throw new FileNotFoundException($"Package Manifest '{packagePath}' not found");
         
         IRepository[] publishTo;
         
         if (Repository != null)
         {
            var repo = _repositoryFactory.TryGetEnabledRepo(Repository);
            if (repo == null)
            {
               _writer.Error($"Repository '{Repository}' does not exist");
               return;
            }

            if (!repo.CanPublish)
            {
               _writer.Error($"Cannot publish in repository '{Repository}'");
               return;
            }

            publishTo = new[] {repo};
         }
         else
         {
            throw new NotSupportedException();
            //publishTo = _repositoryFactory.TryGetEnabledRepos(true, true).Where(r => r.CanPublish).ToArray();
         }

         if (publishTo.Length == 0)
         {
            _writer.Error("There are no repositories to publish to");
            return;
         }

         _writer.Text($"Publishing package '{packagePath}' to {publishTo.Length} repositor{(publishTo.Length == 1 ? "y" : "ies")}");

         foreach (var repo in publishTo)
         {
            _writer.BeginWrite().Text($"Publishing package '{packagePath}' to repository '{repo.Name}' ('{repo.RootPath}')... ");
            
            repo.Publish(packagePath);

            _writer.Success("published").EndWrite();
         }
      }
   }
}
