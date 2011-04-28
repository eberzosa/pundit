using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDesk.Options;
using Pundit.Core;
using Pundit.Core.Model;

namespace Pundit.Console.Commands
{
   class SearchCommand : BaseCommand
   {
      public SearchCommand(string[] args) : base(args)
      {
      }

      public override void Execute()
      {
         int depth = GetDepth();
         string text = GetText();
         var result = new List<KeyValuePair<PackageKey, string>>();

         var names = LocalRepository.TakeFirstRegisteredNames(depth, true);

         //search
         foreach(string repoName in names)
         {
            GlamTerm.WriteLine("searching [{0}]...", repoName);

            IRepository repo = RepositoryFactory.CreateFromUri(LocalRepository.GetRepositoryUriFromName(repoName));

            result.AddRange(repo.Search(text)
               .Select(pk => new KeyValuePair<PackageKey, string>(pk, repoName)));

         }

         //display results
         if(result.Count == 0)
         {
            GlamTerm.WriteErrorLine("nothing found");
         }
         else
         {
            foreach(var r in result)
            {
               GlamTerm.WriteLine("id: [{0}], platform: [{1}], version: {2}, repository: [{3}]",
                  r.Key.PackageId, r.Key.Platform, r.Key.Version, r.Value);
            }
         }
      }
   }
}
