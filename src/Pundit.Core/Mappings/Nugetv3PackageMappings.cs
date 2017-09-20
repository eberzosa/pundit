using System;
using System.Collections.Generic;
using System.Linq;
using ExpressMapper;
using ExpressMapper.Extensions;
using NuGet.Frameworks;
using NuGet.Versioning;
using Pundit.Core.Model;
using PackageDependency = EBerzosa.Pundit.Core.Model.Package.PackageDependency;
using NuGetv3 = NuGet.Packaging;

namespace EBerzosa.Pundit.Core.Mappings
{
   internal class NuGetv3PackageMappings
   {
      private static bool _registered;

      public static void Initialise()
      {
         if (_registered)
            return;

         _registered = true;
         
         Mapper.RegisterCustom<PackageSpec, NuGetv3.Manifest>(spec =>
         {
            var metadata = new NuGetv3.ManifestMetadata
            {
               MinClientVersionString = "3.3", // Because of the ContentFiles

               Id = spec.PackageId,
               Version = new NuGetVersion(spec.Version.Major, spec.Version.Minor, spec.Version.Build, spec.Version.Revision),
               Description = spec.Description,
               Authors = new[] {spec.Author},

               // Optional
               ReleaseNotes = spec.ReleaseNotes,
               //LicenseUrl = spec.License,
               //ProjectUrl = spec.ProjectUrl,
               //FrameworkAssemblies = new List<NuGet.ManifestFrameworkAssembly> {new NuGet.ManifestFrameworkAssembly {TargetFramework = spec.Platform}},

               DependencyGroups = spec.Dependencies.Map<List<PackageDependency>, IEnumerable<NuGetv3.PackageDependencyGroup>>(),
            };

            metadata.SetLicenseUrl("http://localhost/" + spec.License);
            metadata.SetProjectUrl(spec.ProjectUrl);
            
            return new NuGetv3.Manifest(metadata, spec.Files.Map<List<SourceFiles>, ICollection<NuGetv3.ManifestFile>>());
         });

         Mapper.RegisterCustom<List<PackageDependency>, IEnumerable<NuGetv3.PackageDependencyGroup>>(dependencies =>
         {
            return dependencies
               .GroupBy(d => d.Platform)
               .Select(packagesForPlatform => new NuGetv3.PackageDependencyGroup(
                  NuGetFramework.Parse(packagesForPlatform.Key),
                  packagesForPlatform.Select(d =>
                     new NuGetv3.Core.PackageDependency(d.PackageId, PunditStringVersionToNugetVersionRange(d.VersionPattern)))));
         });
         
         Mapper.RegisterCustom<SourceFiles, NuGetv3.ManifestFile>(file => new NuGetv3.ManifestFile
         {
            Source = file.Include,
            Exclude = file.Exclude,
            Target = file.FileKind.Map<PackageFileKind, string>() + file.TargetDirectory
         });

         Mapper.RegisterCustom<PackageFileKind, string>(kind =>
         {
            switch (kind)
            {
               case PackageFileKind.Binary:
                  return "lib/";

               case PackageFileKind.Include:
                  return "content/includes/";

               case PackageFileKind.Tools:
                  return "tools/";

               case PackageFileKind.Other:
               case PackageFileKind.Unknown:
                  return "content/";
                  
               default:
                  throw new NotSupportedException($"PackageFileKind '{kind}' not supported");
            }
         });
      }

      internal static VersionRange PunditStringVersionToNugetVersionRange(string punditVersion)
      {
         var versionChunks = punditVersion.Split('.');

         var lastNumberPlusOne = int.Parse(versionChunks[versionChunks.Length - 1]) + 1;

         var minVersion = new NuGetVersion(
            int.Parse(versionChunks[0]), 
            int.Parse(versionChunks[1]),
            versionChunks.Length > 2 ? int.Parse(versionChunks[2]) : 0,
            versionChunks.Length > 3 ? int.Parse(versionChunks[3]) : 0);

         var maxVersion = new NuGetVersion(
            int.Parse(versionChunks[0]),
            versionChunks.Length > 2 ? int.Parse(versionChunks[1]) : lastNumberPlusOne,
            versionChunks.Length > 3 ? int.Parse(versionChunks[2]) : versionChunks.Length > 2 ? lastNumberPlusOne : 0,
            versionChunks.Length > 4 ? int.Parse(versionChunks[3]) : versionChunks.Length > 3 ? lastNumberPlusOne : 0);

         return new VersionRange(minVersion, true, maxVersion, false);
      }
   }
}
