using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   [XmlRoot("snaphot")]
   public class RepositorySnapshot
   {
      public const string DefaultFileExtension = ".pundit-repo-snapshot";

      [XmlArray("packages")]
      [XmlArrayItem("package")]
      public PackageKey[] Packages { get; set; }

      public void SaveTo(string filePath)
      {
         var xs = new XmlSerializer(typeof (RepositorySnapshot));

         using(Stream s = File.Create(filePath))
         {
            xs.Serialize(s, this);
         }
      }

      public static RepositorySnapshot LoadFrom(string filePath)
      {
         var xs = new XmlSerializer(typeof (RepositorySnapshot));

         using(Stream s = File.OpenRead(filePath))
         {
            return (RepositorySnapshot) xs.Deserialize(s);
         }
      }
   }
}
