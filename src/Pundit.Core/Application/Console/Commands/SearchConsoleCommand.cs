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
         string text = GetParameter("t:|text:", 0);
         bool inXml = GetBoolParameter(false, "x|xml");

         console.WriteLine("searching '{0}'...", text);
         bool found = false;
         foreach(PackageKey key in LocalConfiguration.RepositoryManager.LocalRepository.Search(text))
         {
            found = true;
            if (inXml)
            {
               console.WriteLine("<package id=\"{0}\" version=\"{1}\" platform=\"{2}\"/>",
                                 key.PackageId, key.Version, key.Platform);
            }
            else
            {
               console.WriteLine("id: [{0}], platform: [{1}], version: {2}",
                                 key.PackageId, key.Platform, key.Version);
            }
         }

         //display results
         if(!found)
         {
            console.WriteLine(ConsoleColor.Red, "nothing found");
         }
      }
   }
}
