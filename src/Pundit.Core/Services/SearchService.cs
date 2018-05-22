using System;
using System.Collections.Generic;
using System.Linq;
using EBerzosa.Pundit.Core.Model.Package;
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

         foreach (var repo in _repositoryFactory.TryGetEnabledRepos(true, !LocalRepoOnly))
         {
            _writer.Empty().Text($"Searching repository '{repo.Name}' [{repo.RootPath}]...").Empty();

            var packageKeys = repo.Search(text).ToArray();

            if (packageKeys.Length > 0)
               anyFound = true;

            _writer.Title("Search results")
               .BeginColumns(new int?[]
               {
                  packageKeys.Max(p => p.PackageId.Length + 2),
                  packageKeys.Max(p => p.Version.ToString().Length + 2),
                  //packageKeys.Max(p => p.Framework.GetShortFolderName().Length + 2),
                  null
               });

            _writer.Header("PackageId").Header("Version").Header("Fmwk");

            foreach (var packageKey in packageKeys)
            {
               _writer
                  .Text(packageKey.PackageId)
                  .Text(packageKey.Version.ToString())
                  .Text(packageKey.Framework?.GetShortFolderName());

               //var outputText = packageKey.ToString();
               
               //var pos = outputText.IndexOf(text, StringComparison.OrdinalIgnoreCase);
               //if (pos < 0)
               //{
               //   _writer.Text(outputText);
               //   continue;
               //}

               //   .Text(outputText.Substring(0, pos))
               //   .HightLight(outputText.Substring(pos, text.Length))
               //   .Text(outputText.Substring(pos + text.Length))
               //   .EndWrite();
            }

            _writer.EndColumns();
         }

         _writer.Empty();

         if (!anyFound)
            _writer.Error($"No packages found that contain '{text}'");
      }
   }
}
