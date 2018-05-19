using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using EBerzosa.Pundit.Core.Framework;
using EBerzosa.Pundit.Core.Versioning;
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

      public PackageKey(string packageId, PunditVersion version, PunditFramework framework)
      {
         Guard.NotNull(packageId, nameof(packageId));

         PackageId = packageId;
         Version = version;
         Framework = framework;
      }

      private PackageKey(string packageId, PunditVersion version, string framework)
         : this(packageId, version, PunditFramework.Parse(framework))
      {
      }

      [XmlAttribute("id")]
      [DataMember]
      public string PackageId { get; set; }

      //[XmlAttribute("version")]
      [XmlIgnore]
      [DataMember]
      public PunditVersion Version { get; set; }

      [XmlAttribute("version")]
      [DataMember]
      public string VersionString
      {
         get => Version.ToString();
         set => Version = PunditVersion.Parse(value);
      }

      [XmlAttribute("platform")]
      [DataMember]
      public PunditFramework Framework { get; set; }
      
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
