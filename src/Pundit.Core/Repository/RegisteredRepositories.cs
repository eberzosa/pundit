using System;
using System.Collections.Generic;
using System.Linq;

namespace EBerzosa.Pundit.Core.Repository
{
   public class RegisteredRepositories
   {
      private readonly Dictionary<string, RegisteredRepository> _repos = new Dictionary<string, RegisteredRepository>();
      private readonly List<RegisteredRepository> _reposList = new List<RegisteredRepository>();
      
      public bool AllowAutoUpdate { get; set; }

      public long LastUpdated { get; set; }

      public RegisteredRepository[] RepositoriesArray
      {
         get => _repos.Values.ToArray();
         set
         {
            _repos.Clear();
            _reposList.Clear();

            if (value != null)
            {
               foreach (RegisteredRepository rr in value)
               {
                  if (RepositoryFactory.NuGetCacheRepoName.Equals(rr.Name, StringComparison.OrdinalIgnoreCase) ||
                      RepositoryFactory.PunditCacheRepoName.Equals(rr.Name, StringComparison.OrdinalIgnoreCase))
                  {
                     throw new InvalidOperationException($"'{rr.Name}' repo uses a reserved name, please change it");
                  }

                  _repos[rr.Name] = rr;
                  _reposList.Add(rr);
               }
            }
         }
      }
      
      public string[] Names => _repos.Keys.ToArray();

      public string[] PublishingNames => _repos.Where(r => r.Value.UseForPublishing).Select(r => r.Key).ToArray();

      public bool ContainsRepository(string name) => _repos.ContainsKey(name);

      public string this[string name] => _repos[name].Uri;

      public string this[int index] => _reposList[index].Name;

      public int TotalCount => _reposList.Count;
   }
}
