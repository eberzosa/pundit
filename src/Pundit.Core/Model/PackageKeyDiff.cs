using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pundit.Core.Model
{
   /// <summary>
   /// 
   /// </summary>
   [DataContract]
   public class PackageKeyDiff : PackageKey
   {
      /// <summary>
      /// 
      /// </summary>
      [DataMember]
      public DiffType DiffType { get; set; }

      /// <summary>
      /// In case of upgraded (or downgraded) package contains a reference to the package it
      /// was updated from
      /// </summary>
      [DataMember]
      public PackageKey OldPackageKey { get; set; }

      /// <summary>
      /// The size of the binary package corresponding to the manifest
      /// </summary>
      [DataMember]
      public long BinarySize { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public PackageKeyDiff()
      {
         
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="diffType"></param>
      /// <param name="packageId"></param>
      /// <param name="version"></param>
      /// <param name="platform"></param>
      public PackageKeyDiff(DiffType diffType, string packageId, Version version, string platform) :
         base(packageId, version, platform)
      {
         DiffType = diffType;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="diffType"></param>
      /// <param name="key"></param>
      public PackageKeyDiff(DiffType diffType, PackageKey key) :
         base(key.PackageId, key.Version, key.Platform)
      {
         DiffType = diffType;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="diffType"></param>
      /// <param name="newPackageKey"></param>
      /// <param name="oldPackageKey"></param>
      public PackageKeyDiff(DiffType diffType, PackageKey newPackageKey, PackageKey oldPackageKey) :
         base(newPackageKey.PackageId, newPackageKey.Version, newPackageKey.Platform)
      {
         DiffType = diffType;
         OldPackageKey = oldPackageKey;
      }
   }
}
