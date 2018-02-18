using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace EBerzosa.Pundit.Core.Repository
{
   public class RegisteredRepositories
   {
      public const string LocalRepositoryName = "local";

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
