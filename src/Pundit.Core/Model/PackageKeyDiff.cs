using EBerzosa.Pundit.Core.Framework;
using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Versioning;

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

      public PackageKeyDiff(DiffType diffType, string packageId, NuGet.Versioning.NuGetVersion version, PunditFramework framework, RepositoryType packageType) 
         : base(packageId, version, framework)
      {
         DiffType = diffType;
         PackageType = packageType;
      }

      public PackageKeyDiff(DiffType diffType, PackageKey key, RepositoryType packageType)
         : base(key.PackageId, key.Version, key.Framework)
      {
         DiffType = diffType;
         PackageType = packageType;
      }

      public PackageKeyDiff(DiffType diffType, PackageKey newPackageKey, PackageKey oldPackageKey, RepositoryType packageType) 
         : base(newPackageKey.PackageId, newPackageKey.Version, newPackageKey.Framework)
      {
         DiffType = diffType;
         OldPackageKey = oldPackageKey;
         PackageType = packageType;
      }
   }
}
