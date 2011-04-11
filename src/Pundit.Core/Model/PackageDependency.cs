using System;
using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   [XmlRoot("package")]
   public class PackageDependency
   {
      public PackageDependency()
      {
         
      }

      public PackageDependency(string packageId, string versionPattern)
      {
         if (packageId == null) throw new ArgumentNullException("packageId");
         if (versionPattern == null) throw new ArgumentNullException("versionPattern");

         PackageId = packageId;
         VersionPattern = versionPattern;
      }

      [XmlAttribute("id")]
      public string PackageId { get; set; }

      [XmlAttribute("version")]
      public string VersionPattern { get; set; }

      [XmlAttribute("platform")]
      public string Platform { get; set; }
   }
}
