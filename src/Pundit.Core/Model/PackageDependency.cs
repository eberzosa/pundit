using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   ///<summary>
   /// Dependency on another package definition
   ///</summary>
   [XmlRoot("package")]
   [Serializable]
   public class PackageDependency
   {
      private string _plaftorm;

      /// <summary>
      /// 
      /// </summary>
      public PackageDependency()
      {
         Platform = Platform;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="packageId"></param>
      /// <param name="versionPattern"></param>
      public PackageDependency(string packageId, string versionPattern) : this()
      {
         if (packageId == null) throw new ArgumentNullException("packageId");
         if (versionPattern == null) throw new ArgumentNullException("versionPattern");

         PackageId = packageId;
         VersionPattern = versionPattern;
      }

      /// <summary>
      /// Package id
      /// </summary>
      [XmlAttribute("id")]
      public string PackageId { get; set; }

      /// <summary>
      /// Version pattern
      /// </summary>
      [XmlAttribute("version")]
      public string VersionPattern { get; set; }

      /// <summary>
      /// Dependency platform. If ommitted "noarch" assumed. If noarch is not found, no automatic
      /// guess is performed
      /// </summary>
      [XmlAttribute("platform")]
      public string Platform
      {
         get { return _plaftorm; }
         set { _plaftorm = string.IsNullOrEmpty(value) ? Package.NoArchPlatformName : value; }
      }

      /// <summary>
      /// Dependency scope
      /// </summary>
      [XmlAttribute("scope")]
      [DefaultValue(DependencyScope.Normal)]
      public DependencyScope Scope { get; set; }

      ///<summary>
      /// Applicable only when dependency is installed into local project. When true, a sub folder
      /// under LIB will be created so binaries are unpacked under LIB\TargetFolder
      ///</summary>
      [XmlAttribute("targetFolder")]
      [DefaultValue(false)]
      public string TargetFolder { get; set; }
   }
}
