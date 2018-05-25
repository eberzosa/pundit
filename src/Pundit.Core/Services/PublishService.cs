using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Model.Package;
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
         if (!Directory.Exists(packagePath))
         {
            PublishInternal(packagePath);
            return;
         }

         foreach (var file in Directory.GetFiles(packagePath, "*", SearchOption.TopDirectoryOnly))
            PublishInternal(file);
      }

      private void PublishInternal(string packagePath)
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
            _writer.Info("Publishing with no repo specified. Trying to publish to cache repos only");
            publishTo = _repositoryFactory.TryGetEnabledRepos(RepositoryScope.Cache).Where(r => r.CanPublish).ToArray();
         }

         if (publishTo.Length == 0)
         {
            _writer.Error("There are no repositories to publish to");
            return;
         }

         foreach (var repo in publishTo)
         {
            _writer.BeginWrite().Text($"Publishing package '{packagePath}' to repository '{repo.Name}'... ");

            if ((Path.GetExtension(packagePath) == PackageManifest.PunditPackageExtension && repo.Type == RepositoryType.Pundit) ||
                (Path.GetExtension(packagePath) == PackageManifest.NuGetPackageExtension && repo.Type == RepositoryType.NuGet))
            {
               repo.Publish(packagePath);
               _writer.Success("published").EndWrite();
               return;
            }
         }

         _writer.Success("failed, no repo found").EndWrite();
      }
   }
}
