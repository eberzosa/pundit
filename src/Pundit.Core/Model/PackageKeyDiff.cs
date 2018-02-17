using System;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Enums;
using NuGet.Versioning;

namespace Pundit.Core.Model
{
   public class PackageKeyDiff : PackageKey
   {
      public DiffType DiffType { get; set; }

      /// <summary>
      /// In case of upgraded (or downgraded) package contains a reference to the package it
      /// was updated from
      /// </summary>
      public PackageKey OldPackageKey { get; set; }

      public PackageKeyDiff()
      {
         
      }

      public PackageKeyDiff(DiffType diffType, string packageId, NuGetVersion version, string platform, bool isDeveloper) 
         : base(packageId, version, platform)
      {
         DiffType = diffType;
      }

      public PackageKeyDiff(DiffType diffType, PackageKey key) 
         : base(key.PackageId, key.Version, key.Platform)
      {
         DiffType = diffType;
      }

      public PackageKeyDiff(DiffType diffType, PackageKey newPackageKey, PackageKey oldPackageKey) 
         : base(newPackageKey.PackageId, newPackageKey.Version, newPackageKey.Platform)
      {
         DiffType = diffType;
         OldPackageKey = oldPackageKey;
      }
   }
}
