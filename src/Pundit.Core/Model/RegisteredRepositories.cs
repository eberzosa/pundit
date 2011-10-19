using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

namespace Pundit.Core.Model
{
   public class RegisteredRepository
   {
      [XmlAttribute("name")]
      public string Name { get; set; }

      [XmlAttribute("uri")]
      public string Uri { get; set; }

      /// <summary>
      /// If true, repository will be used for publishing.
      /// Configuration may have more than one repository with this flag set to true,
      /// in which case publishing will be performed to every of them.
      /// </summary>
      [XmlAttribute("publish")]
      public bool UseForPublishing { get; set; }

      /// <summary>
      /// If repository is disabled resolution process shouldn't take it into account
      /// </summary>
      [XmlAttribute("enabled")]
      public bool IsEnabled { get; set; }

      public RegisteredRepository()
      {
         IsEnabled = true;
      }

      public RegisteredRepository(string name) : this()
      {
         this.Name = name;
      }

      public override string ToString()
      {
         return Name + (IsEnabled ? "" : " (disabled)");
      }
   }

   [XmlRoot("repositories")]
   public class RegisteredRepositories
   {
      public const string LocalRepositoryName = "local";

      private Dictionary<string, RegisteredRepository> _repos = new Dictionary<string, RegisteredRepository>();
      private List<RegisteredRepository> _reposList = new List<RegisteredRepository>();

      [XmlAttribute("auto-update")]
      public bool AllowAutoUpdate { get; set; }

      [XmlAttribute("last-updated")]
      public long LastUpdated { get; set; }

      [XmlArray("list")]
      [XmlArrayItem("repository")]
      public RegisteredRepository[] RepositoriesArray
      {
         get { return _repos.Values.ToArray(); }
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

      public static RegisteredRepositories LoadFrom(string filePath)
      {
         var xs = new XmlSerializer(typeof(RegisteredRepositories));

         using (Stream s = File.OpenRead(filePath))
         {
            return xs.Deserialize(s) as RegisteredRepositories;
         }
      }

      public void SaveTo(Stream s)
      {
         var xs = new XmlSerializer(typeof (RegisteredRepositories));

         xs.Serialize(s, this);
      }

      /// <summary>
      /// Active repositories
      /// </summary>
      [XmlIgnore]
      public string[] ActiveNames
      {
         get { return _repos.Where(r => r.Value.IsEnabled).Select(r => r.Key).ToArray(); }
      }

      [XmlIgnore]
      public string[] PublishingNames
      {
         get { return _repos.Where(r => r.Value.UseForPublishing && r.Value.IsEnabled).Select(r => r.Key).ToArray(); }
      }

      public bool ContainsRepository(string name)
      {
         return _repos.ContainsKey(name);
      }

      [XmlIgnore]
      public IEnumerable<RegisteredRepository> All
      {
         get { return new List<RegisteredRepository>(_repos.Values); }
      }

      [XmlIgnore]
      public string this[string name]
      {
         get { return _repos[name].Uri; }
      }
   }
}
