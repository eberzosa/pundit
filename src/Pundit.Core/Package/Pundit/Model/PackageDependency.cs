using System;
using System.Diagnostics;
using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.Core.Model.Package
{
   ///<summary>
   /// Package dependency
   ///</summary>
   [DebuggerDisplay("{PackageId} [{AllowedVersions.ToString()}] [{Framework}] [{Scope}]")]
   public class PackageDependency
   {
      public PackageDependency(string packageId, NuGet.Versioning.VersionRange allowedVersions)
      {
         Guard.NotNull(packageId, nameof(packageId));
         Guard.NotNull(allowedVersions, nameof(allowedVersions));
         
         PackageId = packageId;
         AllowedVersions = allowedVersions;
      }

      public string PackageId { get; set; }

      public NuGet.Versioning.VersionRange AllowedVersions { get; set; }

      [Obsolete("This is kept for Punit packages. Do not used in NuGet")]
      public string Framework { get; set; }
      
      /// <summary>
      /// Set to true if the dependency must exist at dev time only.
      /// Use it if and only if your package doesn't require the dependent package in the runtime.
      /// This dependency won't be included in the compiled package manifest.
      /// 
      /// Real World Examples:
      /// 
      /// 1) You reference the boost.org library, however your project output is a pure so/dll.
      /// In this case your package's consumers won't need the boost libraries as they are linked in
      /// statically.
      /// 
      /// 2) You reference a "resource dll" (images, strings etc) which is embedded as a resource
      /// in your package output, i.e. files are not read from the disk, but from a so/dll instead.
      /// 
      /// 3) You write a .NET library, however you decide to merge-in the dependencies inside you
      /// library (using ILMerge or custom AppDomain loader)
      /// 
      /// ... (think carefully)
      /// 
      /// </summary>
      public DependencyScope Scope { get; set; }

      ///<summary>
      /// Applicable only when dependency is installed into local project. When true, a sub folder
      /// under LIB will be created so binaries are unpacked under LIB\Platform\*
      ///</summary>
      public bool CreatePlatformFolder { get; set; }
   }
}
