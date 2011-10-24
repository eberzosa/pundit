using System;
using System.Collections.Generic;
using System.Linq;
using NDesk.Options;
using Pundit.Core.Model;

namespace Pundit.Core.Application.Console.Commands
{
   class SearchConsoleCommand : BaseConsoleCommand
   {
      public SearchConsoleCommand(IConsoleOutput console, string currentDirectory, string[] args)
         : base(console, currentDirectory, args)
      {
      }

      public override void Execute()
      {
         int depth = GetDepth();
         string text = GetText();
         var result = new List<KeyValuePair<PackageKey, string>>();
         bool printXml = false;

         new OptionSet().Add("x|xml", x => printXml = x != null).Parse(GetCommandLine());

         var names = LocalRepository.TakeFirstRegisteredNames(depth, true);

         //search
         foreach(string repoName in names)
         {
            console.WriteLine("searching [{0}]...", repoName);

            IRepository repo = RepositoryFactory.CreateFromUri(LocalRepository.GetRepositoryUriFromName(repoName));

            result.AddRange(repo.Search(text)
               .Select(pk => new KeyValuePair<PackageKey, string>(pk, repoName)));

         }

         //display results
         if(result.Count == 0)
         {
            console.WriteLine(ConsoleColor.Red, "nothing found");
         }
         else
         {
            foreach(var r in result)
            {
               if (printXml)
               {
                  console.WriteLine("<package id=\"{0}\" version=\"{1}\" platform=\"{2}\"/>",
                                    r.Key.PackageId, r.Key.Version, r.Key.Platform);
               }
               else
               {
                  console.WriteLine("id: [{0}], platform: [{1}], version: {2}, repository: [{3}]",
                                    r.Key.PackageId, r.Key.Platform, r.Key.Version, r.Value);
               }
            }
         }
      }
   }
}
