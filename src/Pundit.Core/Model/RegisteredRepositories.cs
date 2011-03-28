using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   public class RegisteredRepository
   {
      [XmlAttribute("name")]
      public string Name { get; set; }

      [XmlAttribute("uri")]
      public string Uri { get; set; }
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
