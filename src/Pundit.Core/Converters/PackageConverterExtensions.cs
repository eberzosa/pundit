﻿using System;
using System.Collections.Generic;
using System.Linq;
using EBerzosa.Pundit.Core.Model.Package;
using NuGet.Frameworks;
using NuGet.Packaging;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Converters
{
   internal static class PackageConverterExtensions
   {
      public static PackageKey GetPackageKeyFromFileName(string fileName)
      {
         var chunks = fileName.Split(new[] { '-' }, 4);
         return new PackageKey(chunks[0], NuGet.Versioning.NuGetVersion.Parse(chunks[1] + "." + chunks[2]), chunks[3]);
      }

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
            new PackageDependencyGroup(
               NuGetFramework.AnyFramework,
               packageDependency.Select(p => p.ToNuGetPackageDependency()))
         };

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
   }
}
