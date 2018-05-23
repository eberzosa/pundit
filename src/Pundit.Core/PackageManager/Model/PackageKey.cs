using System;
using System.Diagnostics;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.Core.Model.Package
{
   /// <summary>
   /// Unique identifier for a package
   /// </summary>
   [DebuggerDisplay("{PackageId} {Version} [{Framework}]")]
   public class PackageKey : ICloneable
   {
      public string PackageId { get; set; }

      public NuGet.Versioning.NuGetVersion Version { get; set; }

      [Obsolete("Used in Pundit packages only.")]
      public string Framework { get; set; }
      

      public PackageKey(string packageId, NuGet.Versioning.NuGetVersion version, string framework)
      {
         Guard.NotNull(packageId, nameof(packageId));

         PackageId = packageId;
         Version = version;
         Framework = framework;
      }
      
      public override bool Equals(object obj)
      {
         if (obj is PackageKey packageKey)
            return PackageId == packageKey.PackageId && Framework == packageKey.Framework && Version == packageKey.Version;

         return false;
      }

      public bool LooseEquals(PackageKey key)
      {
         return PackageId == key.PackageId && Framework == key.Framework;
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
