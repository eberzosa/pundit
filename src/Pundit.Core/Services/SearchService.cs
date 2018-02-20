using System;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.Core.Services
{
   public class SearchService
   {
      private readonly RepositoryFactory _repositoryFactory;
      private readonly IWriter _writer;

      public bool LocalRepoOnly { get; set; }

      public bool ToXml { get; set; }

      public SearchService(RepositoryFactory repositoryFactory, IWriter writer)
      {
         Guard.NotNull(repositoryFactory, nameof(repositoryFactory));
         Guard.NotNull(writer, nameof(writer));

         _repositoryFactory = repositoryFactory;
         _writer = writer;
      }

      public void Search(string text)
      {
         if (ToXml)
            throw new NotImplementedException("Feature not available");
         
         var anyFound = false;
         
         foreach (var repo in _repositoryFactory.GetEnabledRepos(true, !LocalRepoOnly))
         {
            _writer.Empty().Text($"Searching repository '{repo.Name}' [{repo.RootPath}]...").Empty();
            
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

         if (!anyFound)
            _writer.Error($"No packages found that contain '{text}'");
      }
   }
}
