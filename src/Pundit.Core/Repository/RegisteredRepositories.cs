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

      public override string ToString()
      {
         return Name;
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

         return xs.Deserialize(File.OpenRead(filePath)) as RegisteredRepositories;
      }

      public void SaveTo(Stream s)
      {
         var xs = new XmlSerializer(typeof (RegisteredRepositories));

         xs.Serialize(s, this);
      }

      /// <summary>
      /// All names
      /// </summary>
      [XmlIgnore]
      public string[] Names
      {
         get { return _repos.Keys.ToArray(); }
      }

      [XmlIgnore]
      public string[] PublishingNames
      {
         get { return _repos.Where(r => r.Value.UseForPublishing).Select(r => r.Key).ToArray(); }
      }

      public bool ContainsRepository(string name)
      {
         return _repos.ContainsKey(name);
      }

      [XmlIgnore]
      public string this[string name]
      {
         get { return _repos[name].Uri; }
      }

      [XmlIgnore]
      public string this[int index]
      {
         get { return _reposList[index].Name; }
      }

      [XmlIgnore]
      public int TotalCount
      {
         get { return _reposList.Count; }
      }
   }
}
