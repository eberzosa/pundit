using System.Collections.Generic;
using System.Xml.Serialization;
using EBerzosa.Pundit.Core.PackageManager.Xml;
using Pundit.Core.Model;

namespace Pundit.Core.Application
{
   [XmlRoot("InstalledPackagesIndex")]
   public class XmlInstalledPackagesIndex
   {
      [XmlArray("installed")]
      [XmlArrayItem("package")]
      public List<XmlPackageKey> InstalledPackages { get; set; }

      [XmlAttribute("configuration")]
      public BuildConfiguration Configuration { get; set; }
   }
}