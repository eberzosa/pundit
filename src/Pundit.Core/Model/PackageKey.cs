using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Pundit.Core.Utils;

namespace Pundit.Core.Model
{
   /// <summary>
   /// Unique identifier for a package
   /// </summary>
   public class PackageKey : IEquatable<PackageKey>, ICloneable
   {
      private string _packageId;
      private Version _version;
      private string _platform;

      public PackageKey()
      {
         Platform = Platform;
      }

      public PackageKey(string packageId, Version version, string platform) : this()
      {
         if (packageId == null) throw new ArgumentNullException("packageId");

         PackageId = packageId;
         Version = version;
         Platform = platform;
      }

      [XmlAttribute("id")]
      public string PackageId
      {
         get { return _packageId; }
         set { _packageId = value; }
      }

      //[XmlAttribute("version")]
      [XmlIgnore]
      public Version Version
      {
         get { return _version; }
         set { _version = value; }
      }

      [XmlAttribute("version")]
      public string VersionString
      {
         get { return _version.ToString(); }
         set { _version = new Version(value);}
      }

      [XmlAttribute("platform")]
      public string Platform
      {
         get { return _platform; }
         set { _platform = string.IsNullOrEmpty(value) ? Package.NoArchPlatformName : value; }
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(obj, null)) return false;
         if (ReferenceEquals(obj, this)) return true;
         if (obj.GetType() != GetType()) return false;
         return Equals((PackageKey) obj);
      }

      public bool LooseEquals(PackageKey key)
      {
         return this.PackageId == key.PackageId &&
                this.Platform == key.Platform;
      }

      public override int GetHashCode()
      {
         return PackageId.GetHashCode()*Version.GetHashCode()*Platform.GetHashCode();
      }

      public object Clone()
      {
         return new PackageKey(PackageId, Version, Platform);
      }

      public bool Equals(PackageKey that)
      {
         if (ReferenceEquals(that, null)) return false;

         return this.PackageId == that.PackageId &&
                this.Platform == that.Platform &&
                this.Version == that.Version;
      }

      public override string ToString()
      {
         return string.Format("package id: [{0}], platform: [{1}], exact version: [{2}]",
                              PackageId, Platform, Version);
      }
   }
}
