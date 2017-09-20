using System;
using System.IO;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Utils;
using Pundit.Core;

namespace EBerzosa.Pundit.Core.Services
{
   public class PublishService
   {
      private readonly LocalRepository _localRepository;
      private readonly RepositoryFactory _repositoryFactory;
      private readonly IWriter _writer;

      public string Repository { get; set; }

      public PublishService(LocalRepository localRepository, RepositoryFactory repositoryFactory, IWriter writer)
      {
         Guard.NotNull(localRepository, nameof(localRepository));
         Guard.NotNull(repositoryFactory, nameof(repositoryFactory));
         Guard.NotNull(writer, nameof(writer));

         _localRepository = localRepository;
         _repositoryFactory = repositoryFactory;
         _writer = writer;
      }

      public void Publish(string packagePath)
      {
         if (!File.Exists(packagePath))
            throw new FileNotFoundException($"Package Manifest '{packagePath}' not found");

         var publishTo = Repository != null
            ? new[] {Repository}
            : _localRepository.Registered.PublishingNames;


         foreach (string repo in publishTo)
         {
            if (!_localRepository.IsValidRepositoryName(repo))
               throw new NotSupportedException($"Repository '{repo}' does not exist");
         }

         _writer.Text($"Publishing package '{packagePath}' to {publishTo.Length} repositor{(publishTo.Length == 1 ? "y" : "ies")}");

         foreach (var repoName in publishTo)
         {
            var repoUri = _localRepository.GetRepositoryUriFromName(repoName);

            _writer.BeginWrite().Text($"Publishing package '{packagePath}' to repository '{repoName}' ('{repoUri}')... ");
            
            var repo = _repositoryFactory.CreateFromUri(repoUri);

            using (Stream packageContents = File.OpenRead(packagePath))
               repo.Publish(packageContents);

            _writer.Success("published").EndWrite();
         }
      }
   }
}
