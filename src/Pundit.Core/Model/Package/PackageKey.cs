using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using EBerzosa.Utils;
using NuGet.Versioning;
using Pundit.Core.Utils;

namespace Pundit.Core.Model
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

      public PackageKey(string packageId, NuGetVersion version, string platform)
      {
         Guard.NotNull(packageId, nameof(packageId));

         PackageId = packageId;
         Version = version;
         Platform = PackageUtils.TrimPlatformName(platform);
      }

      [XmlAttribute("id")]
      [DataMember]
      public string PackageId { get; set; }

      //[XmlAttribute("version")]
      [XmlIgnore]
      [DataMember]
      public NuGetVersion Version { get; set; }

      [XmlAttribute("version")]
      [DataMember]
      public string VersionString
      {
         get => Version.ToString();
         set => Version = new NuGetVersion(value);
      }

      [XmlAttribute("platform")]
      [DataMember]
      public string Platform { get; set; }
      
      public override bool Equals(object obj)
      {
         if (obj is PackageKey packageKey)
            return 
               PackageId == packageKey.PackageId && 
               Platform == packageKey.Platform && 
               Version == packageKey.Version;

         return false;
      }

      public bool LooseEquals(PackageKey key)
      {
         return 
            PackageId == key.PackageId && 
            Platform == key.Platform;
      }

      public override int GetHashCode()
      {
         return PackageId.GetHashCode() * Version.GetHashCode() * Platform.GetHashCode();
      }

      public object Clone()
      {
         return new PackageKey(PackageId, Version, Platform);
      }

      public override string ToString()
      {
         return $"{PackageId} v{Version} ({Platform})";
      }
   }
}
