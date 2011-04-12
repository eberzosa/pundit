﻿using System;
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

      /// <summary>
      /// Dependency platform. If ommitted "noarch" assumed. If noarch is not found, no automatic
      /// guess is performed
      /// </summary>
      [XmlAttribute("platform")]
      public string Platform { get; set; }

      /// <summary>
      /// Set to true if the dependency must exist at dev time only.
      /// Use it if and only if your package doesn't require the dependent package in the runtime.
      /// This dependency won't be included in the compiled package manifest.
      /// 
      /// Real World Examples:
      /// 
      /// 1) You reference the boost.org library, however your project output is a pure so/dll.
      /// In this case your package's consumers won't need the boost libraries as they are linked in
      /// statically.
      /// 
      /// 2) You reference a "resource dll" (images, strings etc) which is embedded as a resource
      /// in your package output, i.e. files are not read from the disk, but from a so/dll instead.
      /// 
      /// 3) You write a .NET library, however you decide to merge-in the dependencies inside you
      /// library (using ILMerge or custom AppDomain loader)
      /// 
      /// ... (think carefully)
      /// 
      /// </summary>
      [XmlAttribute("devtime")]
      public bool DevTimeOnly { get; set; }
   }
}
