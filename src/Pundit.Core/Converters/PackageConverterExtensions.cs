using System;
using System.Collections.Generic;
using System.Linq;
using EBerzosa.Pundit.Core.Framework;
using EBerzosa.Pundit.Core.Model.Package;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Converters
{
   internal static class PackageConverterExtensions
   {
      public static NuGet.Packaging.ManifestMetadata ToNuGetManifestMetadata(this PackageSpec packageSpec)
      {
         var metadata = new NuGet.Packaging.ManifestMetadata
         {
            MinClientVersionString = "3.3", // Because of the ContentFiles

            Id = packageSpec.PackageId,
            Version = packageSpec.Version,
            Description = packageSpec.Description,
            Authors = new[] {packageSpec.Author},

            // Optional
            ReleaseNotes = packageSpec.ReleaseNotes,
            PackageTypes = new[] {new NuGet.Packaging.Core.PackageType("Pundit", new Version(2, 0))},

            DependencyGroups = packageSpec.Dependencies.ToNuGetPackageDependencyGroup(),
         };

         metadata.SetLicenseUrl("http://localhost/" + packageSpec.License);
         metadata.SetProjectUrl(packageSpec.ProjectUrl);

         return metadata;
      }

      private static IEnumerable<NuGet.Packaging.PackageDependencyGroup> ToNuGetPackageDependencyGroup(this IEnumerable<PackageDependency> packageDependency)
      {
         return new[]
         {
            new NuGet.Packaging.PackageDependencyGroup(
               PunditFramework.AnyFramework.NuGetFramework, 
               packageDependency.Select(d => d.ToNuGetPackageDependency()))
         };
      }

      private static NuGet.Packaging.Core.PackageDependency ToNuGetPackageDependency(this PackageDependency packageDependency)
      {
         return new NuGet.Packaging.Core.PackageDependency(packageDependency.PackageId, packageDependency.AllowedVersions.NuGetVersionRange);
      }
   }
}
