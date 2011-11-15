using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   ///<summary>
   /// Package dependency
   ///</summary>
   [XmlRoot("package")]
   [Serializable]
   [DataContract]
   public class PackageDependency
   {
      private string _plaftorm;

      public PackageDependency()
      {
         Platform = Platform;
      }

      public PackageDependency(string packageId, string versionPattern) : this()
      {
         if (packageId == null) throw new ArgumentNullException("packageId");
         if (versionPattern == null) throw new ArgumentNullException("versionPattern");

         PackageId = packageId;
         VersionPattern = versionPattern;
      }

      [XmlAttribute("id")]
      [DataMember(Name = "id")]
      public string PackageId { get; set; }

      [XmlAttribute("version")]
      [DataMember(Name = "versionPattern")]
      public string VersionPattern { get; set; }

      /// <summary>
      /// Dependency platform. If ommitted "noarch" assumed. If noarch is not found, no automatic
      /// guess is performed
      /// </summary>
      [XmlAttribute("platform")]
      [DataMember(Name = "platform")]
      public string Platform
      {
         get { return _plaftorm; }
         set { _plaftorm = string.IsNullOrEmpty(value) ? Package.NoArchPlatformName : value; }
      }

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
      /// THIS ATTRIBUTE IS DEPRECATED, USE SCOPE PROPERTY INSTEAD
      /// 
      /// </summary>
      [XmlAttribute("devtime")]
      [DefaultValue(false)]
      public bool DevTimeOnly
      {
         get { return Scope != DependencyScope.Normal; }
         set
         {
            if(value)
            {
               if (Scope == DependencyScope.Normal)
                  Scope = DependencyScope.Build;
            }
            else
            {
               if (Scope != DependencyScope.Normal)
                  Scope = DependencyScope.Normal;
            }
         }
      }

      [XmlAttribute("scope")]
      [DefaultValue(DependencyScope.Normal)]
      [DataMember(Name = "scope")]
      public DependencyScope Scope { get; set; }

      ///<summary>
      /// Applicable only when dependency is installed into local project. When true, a sub folder
      /// under LIB will be created so binaries are unpacked under LIB\Platform\*
      ///</summary>
      [XmlAttribute("createPlatformFolder")]
      [DefaultValue(false)]
      [DataMember(Name = "createPlatformFolder")]
      public bool CreatePlatformFolder { get; set; }
   }
}
