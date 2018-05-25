using System;
using System.Collections.Generic;
using System.Linq;
using EBerzosa.Pundit.Core.Model.Package;
using Mapster;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Converters
{
   internal static class PackageConverterExtensions
   {
      static PackageConverterExtensions()
      {
         RegisterMappings();
      }

      public static PackageSpec ToPackageSpec(this PackageManifest manifest) //, NuGet.Frameworks.NuGetFramework framework)
      {
         var spec = manifest.Adapt<PackageSpec>();
         //spec.Framework = framework;

         return spec;
      }

      public static NuGet.Packaging.ManifestMetadata ToNuGetManifestMetadata(this PackageSpec packageSpec, NuGet.Frameworks.NuGetFramework framework)
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

            DependencyGroups = packageSpec.Dependencies.ToNuGetPackageDependencyGroup(framework),
         };

         metadata.SetLicenseUrl("http://localhost/" + packageSpec.License);
         metadata.SetProjectUrl(packageSpec.ProjectUrl);

         return metadata;
      }

      private static IEnumerable<NuGet.Packaging.PackageDependencyGroup> ToNuGetPackageDependencyGroup(
         this IEnumerable<PackageDependency> packageDependency, NuGet.Frameworks.NuGetFramework framework)
      {
         return new[] {new NuGet.Packaging.PackageDependencyGroup(framework, packageDependency.Select(p => p.ToNuGetPackageDependency()))};

         //return packageDependency
         //   .GroupBy(dependency => dependency.Framework, dependency => dependency)
         //   .Select(grupedDepenencies => new NuGet.Packaging.PackageDependencyGroup(
         //      grupedDepenencies.Key, 
         //      grupedDepenencies.Select(dependency => dependency.ToNuGetPackageDependency())));
      }

      private static NuGet.Packaging.Core.PackageDependency ToNuGetPackageDependency(this PackageDependency packageDependency)
      {
         return new NuGet.Packaging.Core.PackageDependency(packageDependency.PackageId, packageDependency.AllowedVersions);
      }



      private static void RegisterMappings()
      {
         TypeAdapterConfig<PackageManifest, PackageSpec>.NewConfig();
         TypeAdapterConfig<PackageDependency, PackageDependency>.NewConfig()
            .ConstructUsing(src => new PackageDependency(src.PackageId, src.AllowedVersions));
      }
   }
}
