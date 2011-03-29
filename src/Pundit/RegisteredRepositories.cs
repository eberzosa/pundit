using System.IO;
using System.Xml.Serialization;

namespace Pundit.Console
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
   }

   [XmlRoot("repositories")]
   public class RegisteredRepositories
   {
      [XmlArray("list")]
      [XmlArrayItem("repository")]
      public RegisteredRepository[] Repositories { get; set; }

      public static RegisteredRepositories LoadFrom(string filePath)
      {
         var xs = new XmlSerializer(typeof(RegisteredRepositories));

         return xs.Deserialize(File.OpenRead(filePath)) as RegisteredRepositories;
      }
   }
}
