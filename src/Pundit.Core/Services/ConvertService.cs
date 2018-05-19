using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Framework;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Serializers;
using EBerzosa.Pundit.Core.Versioning;
using EBerzosa.Utils;
using Mapster;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Services
{
   public class ConvertService
   {
      private readonly PackService _packService;
      private readonly PackageSerializerFactory _packageSerializerFactory;
      private readonly IWriter _writer;

      public string SourcePath { get; set; }

      public string DestinationFolder { get; set; }

      public string Framework { get; set; }


      public ConvertService(PackService packService, PackageSerializerFactory packageSerializerFactory, IWriter writer)
      {
         _packService = packService;
         _packageSerializerFactory = packageSerializerFactory;
         _writer = writer;
      }


      public void NuGetToPundit()
      {
         Guard.NotEmpty(SourcePath, nameof(SourcePath));
         
         if (Path.GetExtension(SourcePath) != ".nupkg")
            throw new InvalidOperationException("The given file is not a NuGet package");

         if (!File.Exists(SourcePath))
            throw new FileNotFoundException("The given file does not exist", SourcePath);

         if (string.IsNullOrEmpty(DestinationFolder))
            DestinationFolder = Path.GetDirectoryName(SourcePath);
         else
            DestinationFolder = Path.GetFullPath(DestinationFolder);
         
         var punditSpec = new PackageSpec();

         using (var stream = File.Open(SourcePath, FileMode.Open, FileAccess.Read, FileShare.Read))
         {
            var reader = new PackageArchiveReader(stream);

            punditSpec.PackageId = reader.NuspecReader.GetIdentity().Id;
            punditSpec.Version = reader.NuspecReader.GetVersion().Adapt<PunditVersion>();
            punditSpec.Author = reader.NuspecReader.GetAuthors();
            punditSpec.Description = reader.NuspecReader.GetDescription();
            punditSpec.License = reader.NuspecReader.GetLicenseUrl();
            punditSpec.ProjectUrl = reader.NuspecReader.GetProjectUrl();
            punditSpec.ReleaseNotes = reader.NuspecReader.GetReleaseNotes();

            // TODO: Only bin supported for now
            foreach (var framework in reader.GetSupportedFrameworks())
            {
               if (!framework.IsDesktop())
               {
                  _writer.Empty().Warning($"Framework '{framework}' is not supported, skipping...");
                  continue;
               }

               punditSpec.Dependencies = new List<PackageDependency>();
               punditSpec.Framework = framework.Adapt<PunditFramework>();
               punditSpec.Files = new List<SourceFiles>();

               var dependencies = reader.GetPackageDependencies().FirstOrDefault(d => d.TargetFramework == framework);

               //TODO: Finish this
               throw new NotImplementedException();
               //if (dependencies != null)
                  //foreach (var dependency in dependencies.Packages)
                  //   punditSpec.Dependencies.Add(new PackageDependency(dependency.Id, dependency.VersionRange));

               var libs = reader.GetLibItems().FirstOrDefault(d => d.TargetFramework == framework);

               if (libs == null)
               {
                  _writer.Empty().Info($"No libraries found for framework '${framework}', skipping...");
                  continue;
               }

               var tempOut = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

               try
               {
                  if (Directory.Exists(tempOut))
                     Directory.Delete(tempOut, true);

                  Directory.CreateDirectory(tempOut);

                  foreach (var lib in libs.Items)
                  {
                     punditSpec.Files.Add(new SourceFiles(lib));

                     reader.ExtractFile(lib, Path.Combine(tempOut, lib), new NullLogger());
                  }

                  var punditSpecFile = Path.Combine(tempOut, PackageManifest.DefaultManifestFileName);
                  using (var outFileStream = File.Create(punditSpecFile))
                     _packageSerializerFactory.GetPundit().SerializePackageSpec(punditSpec, outFileStream);

                  _packService.ManifestFileOrPath = punditSpecFile;
                  _packService.Pack();

                  var destinationFile = Path.Combine(DestinationFolder, Path.GetFileName(_packService.DestinationFile));
                  if (File.Exists(destinationFile))
                     File.Delete(destinationFile);

                  File.Move(_packService.DestinationFile, destinationFile);
               }
               finally
               {
                  if (Directory.Exists(tempOut))
                     Directory.Delete(tempOut, true);
               }
            }
         }
      }
   }
}
