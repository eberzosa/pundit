using System;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Utils;
using Pundit.Core;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Services
{
   public class SearchService
   {
      private readonly LocalRepository _localRepository;
      private readonly RepositoryFactory _repositoryFactory;
      private readonly IWriter _writer;

      public bool LocalRepoOnly { get; set; }

      public bool ToXml { get; set; }

      public SearchService(LocalRepository localRepository, RepositoryFactory repositoryFactory, IWriter writer)
      {
         Guard.NotNull(localRepository, nameof(localRepository));
         Guard.NotNull(repositoryFactory, nameof(repositoryFactory));
         Guard.NotNull(writer, nameof(writer));

         _localRepository = localRepository;
         _repositoryFactory = repositoryFactory;
         _writer = writer;
      }

      public void Search(string text)
      {
         if (ToXml)
            throw new NotImplementedException("Feature not available");

         var repoNames = _localRepository.TakeFirstRegisteredNames(LocalRepoOnly ? 0 : int.MaxValue, true);

         var anyFound = false;
         
         foreach (string repoName in repoNames)
         {
            _writer.Text($"Searching '{repoName}'...");

            IRepository repo = _repositoryFactory.CreateFromUri(_localRepository.GetRepositoryUriFromName(repoName));

            foreach (var packageKey in repo.Search(text))
            {
               anyFound = true;

               var outputText = packageKey.ToString();

               var pos = outputText.IndexOf(text, StringComparison.OrdinalIgnoreCase);
               if (pos < 0)
               {
                  _writer.Text(outputText);
                  continue;
               }

               _writer.BeginWrite()
                  .Text(outputText.Substring(0, pos))
                  .HightLight(outputText.Substring(pos, text.Length))
                  .Text(outputText.Substring(pos + text.Length))
                  .EndWrite();
            }

            _writer.Empty();
         }

         //display results
         if (!anyFound)
         {
            _writer.Error($"No packages found that contain '{text}'");
         }
      }
   }
}
