using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.Core.Model.Package
{
   /// <summary>
   /// Unique identifier for a package
   /// </summary>
   [DataContract]
   public class PackageKey : ICloneable
   {
      public PackageKey()
      {
      }

      public PackageKey(string packageId, NuGet.Versioning.NuGetVersion version, NuGet.Frameworks.NuGetFramework framework)
      {
         Guard.NotNull(packageId, nameof(packageId));

         PackageId = packageId;
         Version = version;
         Framework = framework;
      }

      private PackageKey(string packageId, NuGet.Versioning.NuGetVersion version, string framework)
         : this(packageId, version, NuGet.Frameworks.NuGetFramework.Parse(framework))
      {
      }

      [XmlAttribute("id")]
      [DataMember]
      public string PackageId { get; set; }

      //[XmlAttribute("version")]
      [XmlIgnore]
      [DataMember]
      public NuGet.Versioning.NuGetVersion Version { get; set; }

      [XmlAttribute("version")]
      [DataMember]
      public string VersionString
      {
         get => Version.ToString();
         set => Version = NuGet.Versioning.NuGetVersion.Parse(value);
      }

      [XmlAttribute("platform")]
      [DataMember]
      public NuGet.Frameworks.NuGetFramework Framework { get; set; }
      
      public override bool Equals(object obj)
      {
         if (obj is PackageKey packageKey)
            return 
               PackageId == packageKey.PackageId && 
               Framework == packageKey.Framework && 
               Version == packageKey.Version;

         return false;
      }

      public bool LooseEquals(PackageKey key)
      {
         return 
            PackageId == key.PackageId && 
            Framework == key.Framework;
      }

      public override int GetHashCode()
      {
         return PackageId.GetHashCode() * Version.GetHashCode() * Framework.GetHashCode();
      }

      public object Clone()
      {
         return new PackageKey(PackageId, Version, Framework);
      }

      public override string ToString()
      {
         return $"{PackageId} v{Version} ({Framework})";
      }
   }
}
