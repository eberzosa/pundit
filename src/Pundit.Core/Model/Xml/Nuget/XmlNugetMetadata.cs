using System.Collections.Generic;
using System.Xml.Serialization;

namespace EBerzosa.Pundit.Core.Model.Xml.Nuget
{
   [XmlRoot("metadata")]
   public class XmlNugetMetadata
   {
      /* Required */

      [XmlElement("id")]
      public string PackageId { get; set; }

      [XmlElement("version")]
      public string Version { get; set; }

      [XmlElement("description")]
      public string Description { get; set; }

      [XmlElement("authors")]
      public string Author { get; set; }

      /* Optional */

      [XmlElement("title")]
      public string Title { get; set; }

      [XmlElement("owners")]
      public string Owners { get; set; }

      [XmlElement("projectUrl")]
      public string ProjectUrl { get; set; }

      [XmlElement("licenseUrl")]
      public string License { get; set; }

      [XmlElement("iconUrl")]
      public string Icon { get; set; }

      [XmlElement("requireLicenseAcceptance")]
      public bool RequireLicenseAcceptance { get; set; }

      [XmlElement("developmentDependency")]
      public bool DevelopmentDependency { get; set; }
      
      [XmlElement("summary")]
      public string Summary { get; set; }
      
      [XmlElement("releaseNotes")]
      public string ReleaseNotes { get; set; }

      [XmlElement("copyright")]
      public string Copyright { get; set; }

      [XmlElement("language")]
      public string Language { get; set; }

      [XmlElement("tags")]
      public string Tags { get; set; }
      
      [XmlElement("minClientVersion")]
      public string MinimumClientVersion { get; set; }

      [XmlArray("dependencies")]
      [XmlArrayItem("dependency")]
      public List<XmlPackageDependency> Dependencies { get; set; }
   }
}
