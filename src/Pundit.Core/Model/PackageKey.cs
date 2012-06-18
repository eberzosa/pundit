using System;
using System.Xml.Serialization;

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

      /// <summary>
      /// 
      /// </summary>
      public PackageKey()
      {
         Platform = Platform;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="packageId"></param>
      /// <param name="version"></param>
      /// <param name="platform"></param>
      /// <exception cref="ArgumentNullException"></exception>
      public PackageKey(string packageId, Version version, string platform)
      {
         if (packageId == null) throw new ArgumentNullException("packageId");
         if (version == null) throw new ArgumentNullException("version");

         PackageId = packageId;
         Version = version;
         Platform = platform;
      }

      /// <summary>
      /// Package ID
      /// </summary>
      [XmlAttribute("id")]
      public string PackageId
      {
         get { return _packageId; }
         set { _packageId = value; }
      }

      /// <summary>
      /// Exact package version
      /// </summary>
      [XmlIgnore]
      public Version Version
      {
         get { return _version; }
         set { _version = value; }
      }

      /// <summary>
      /// Backed string conversion for serialization
      /// </summary>
      [XmlAttribute("version")]
      public string VersionString
      {
         get { return _version.ToString(); }
         set { _version = new Version(value);}
      }

      /// <summary>
      /// 
      /// </summary>
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
