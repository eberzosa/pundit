using System.Collections.Generic;
using System.Xml.Serialization;
using EBerzosa.Pundit.Core.Model.Package;
using Pundit.Core.Model;

namespace Pundit.Core.Application
{
   public class XmlInstalledPackagesIndex
   {
      [XmlArray("installed")]
      [XmlArrayItem("package")]
      public List<PackageKey> InstalledPackages { get; set; }

      [XmlAttribute("configuration")]
      public BuildConfiguration Configuration { get; set; }
   }
}