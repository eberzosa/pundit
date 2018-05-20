using System;
using System.Collections.Generic;
using System.Linq;
using EBerzosa.Pundit.Core.Framework;
using EBerzosa.Pundit.Core.Versioning;
using Mapster;
using NuGet.Frameworks;
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

         Mapster.TypeAdapterConfig<PackageSpec, NuGetv3.Manifest>.NewConfig()
            .MapWith(spec => new NuGetv3.Manifest(CreateManifestMetadata(spec), spec.Files.Adapt<List<SourceFiles>, ICollection<NuGetv3.ManifestFile>>()));

         Mapster.TypeAdapterConfig<List<PackageDependency>, IEnumerable<NuGetv3.PackageDependencyGroup>>.NewConfig()
            .MapWith(dependencies => dependencies
               .GroupBy(d => d.Framework)
               .Select(packagesForPlatform => new NuGetv3.PackageDependencyGroup(
                  packagesForPlatform.Key.Adapt<NuGetFramework>(),
                  packagesForPlatform.Select(d =>
                     new NuGetv3.Core.PackageDependency(d.PackageId, d.AllowedVersions.Adapt<NuGet.Versioning.VersionRange>())))));

         Mapster.TypeAdapterConfig<SourceFiles, NuGetv3.ManifestFile>.NewConfig()
            .Map(file => file.Target, files => files.FileKind.Adapt<PackageFileKind, string>() + files.TargetDirectory);

         Mapster.TypeAdapterConfig<PackageFileKind, string>.NewConfig()
            .MapWith(kind => PackageFileKindToString(kind));
      }

      internal static NuGet.Versioning.VersionRange PunditStringVersionToVersionRange(string punditVersion)
      {
         var versionChunks = punditVersion.Split('.');

         var lastNumberPlusOne = int.Parse(versionChunks[versionChunks.Length - 1]) + 1;

         var minVersion = new PunditVersion(
            int.Parse(versionChunks[0]),
            int.Parse(versionChunks[1]),
            versionChunks.Length > 2 ? int.Parse(versionChunks[2]) : 0,
            versionChunks.Length > 3 ? int.Parse(versionChunks[3]) : 0);

         var maxVersion = new PunditVersion(
            int.Parse(versionChunks[0]),
            versionChunks.Length > 2 ? int.Parse(versionChunks[1]) : lastNumberPlusOne,
            versionChunks.Length > 3 ? int.Parse(versionChunks[2]) : versionChunks.Length > 2 ? lastNumberPlusOne : 0,
            versionChunks.Length > 4 ? int.Parse(versionChunks[3]) : versionChunks.Length > 3 ? lastNumberPlusOne : 0);

         return new NuGet.Versioning.VersionRange(minVersion, true, maxVersion, false);
      }
      

      private static string PackageFileKindToString(PackageFileKind kind)
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
      }
   }
}
