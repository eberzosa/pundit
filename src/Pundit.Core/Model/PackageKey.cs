using System;
using System.Xml.Serialization;
using Pundit.Core.Utils;

namespace Pundit.Core.Model
{
   /// <summary>
   /// Unique identifier for a package
   /// </summary>
   public class PackageKey : ICloneable
   {
      private string _packageId;
      private Version _version;
      private string _platform;

      public PackageKey()
      {
         
      }

      public PackageKey(string packageId, Version version, string platform)
      {
         if (packageId == null) throw new ArgumentNullException("packageId");

         PackageId = packageId;
         Version = version;
         Platform = PackageUtils.TrimPlatformName(platform);
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
         set { _platform = value; }
      }


      public override bool Equals(object obj)
      {
         if(obj is PackageKey)
         {
            var that = (PackageKey) obj;

            return this.PackageId == that.PackageId &&
                   this.Platform == that.Platform &&
                   this.Version == that.Version;
         }

         return false;
      }

      public override int GetHashCode()
      {
         return PackageId.GetHashCode()*Version.GetHashCode()*Platform.GetHashCode();
      }

      public object Clone()
      {
         return new PackageKey(PackageId, Version, Platform);
      }

      public override string ToString()
      {
         return string.Format("package id: [{0}], platform: [{1}], exact version: [{2}]",
                              PackageId, Platform, Version);
      }
   }
}
