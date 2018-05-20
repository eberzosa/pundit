using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Pundit.Core.Model.Xml;
using ExpressMapper;
using ExpressMapper.Extensions;
using Pundit.Core.Model;
using PackageDependency = EBerzosa.Pundit.Core.Model.Package.PackageDependency;

namespace EBerzosa.Pundit.Core.Mappings
{
   internal class NugetPackageMappings
   {
      private static bool _registered;

      public static void XmlMappings()
      {
         if (_registered)
            return;

         _registered = true;

         Mapper.RegisterCustom<PackageSpec, NuGet.Manifest>(spec => new NuGet.Manifest
         {
            Metadata = new NuGet.ManifestMetadata
            {
               MinClientVersionString = "3.3", // Because of the ContentFiles

               Id = spec.PackageId,
               Version = spec.Version.ToString(),
               Description = spec.Description,
               Authors = spec.Author,

               // Optional
               ReleaseNotes = spec.ReleaseNotes,
               LicenseUrl = spec.License,
               ProjectUrl = spec.ProjectUrl,
               //FrameworkAssemblies = new List<NuGet.ManifestFrameworkAssembly> {new NuGet.ManifestFrameworkAssembly {TargetFramework = spec.Platform}},

               DependencySets = spec.Dependencies.Map<List<PackageDependency>, List<NuGet.ManifestDependencySet>>(),
               ContentFiles = spec.Files.Map<List<SourceFiles>, List<NuGet.ManifestContentFiles>>(),

            }
         });

         Mapper.RegisterCustom<PackageDependency, NuGet.ManifestDependencySet>(dependency => new NuGet.ManifestDependencySet
         {
            Dependencies = new List<NuGet.ManifestDependency>
            {
               new NuGet.ManifestDependency
               {
                  Id = dependency.PackageId,
                  Version = dependency.VersionPattern
               }
            },
            TargetFramework = dependency.Platform
         });

         Mapper.RegisterCustom<SourceFiles, NuGet.ManifestContentFiles>(files => new NuGet.ManifestContentFiles
         {
            Include = files.Include,
            Exclude = files.Exclude,
            BuildAction = files
         });

         Mapper.Register<XmlPackageDependency, PackageDependency>()

            .After((src, dst) =>
            {
               if (src.DevTimeOnly)
               {
                  if (src.Scope == XmlDependencyScope.Normal)
                     dst.Scope = DependencyScope.Build;
               }
               else
               {
                  if (src.Scope != XmlDependencyScope.Normal)
                     dst.Scope = DependencyScope.Normal;
               }
            });


         Mapper.Compile();
      }
   }
}
