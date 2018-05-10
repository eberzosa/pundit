﻿using System;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Pundit.Core.Repository;
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

      public RepositoryType PackageType { get; set; }

      public PackageKeyDiff()
      {
         
      }

      public PackageKeyDiff(DiffType diffType, string packageId, NuGetVersion version, string platform, RepositoryType packageType, bool isDeveloper) 
         : base(packageId, version, platform)
      {
         DiffType = diffType;
         PackageType = packageType;
      }

      public PackageKeyDiff(DiffType diffType, PackageKey key, RepositoryType packageType)
         : base(key.PackageId, key.Version, key.Platform)
      {
         DiffType = diffType;
         PackageType = packageType;
      }

      public PackageKeyDiff(DiffType diffType, PackageKey newPackageKey, PackageKey oldPackageKey, RepositoryType packageType) 
         : base(newPackageKey.PackageId, newPackageKey.Version, newPackageKey.Platform)
      {
         DiffType = diffType;
         OldPackageKey = oldPackageKey;
         PackageType = packageType;
      }
   }
}
