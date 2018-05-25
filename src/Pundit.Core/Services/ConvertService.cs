using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Converters;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Package;
using EBerzosa.Pundit.Core.Package.Pundit;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Serializers;
using EBerzosa.Utils;
using ICSharpCode.SharpZipLib.Zip;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Services
{
   public class ConvertService
   {
      private readonly PackService _packService;
      private readonly PackageReaderFactory _packageReaderFactory;
      private readonly IPackageSerializer _packageSerializer;
      private readonly IWriter _writer;

      public string SourcePath { get; set; }

      public string DestinationFolder { get; set; }

      public string Framework { get; set; }


      public ConvertService(PackService packService, PackageReaderFactory packageReaderFactory, IPackageSerializer packageSerializer, IWriter writer)
      {
         _packService = packService;
         _packageReaderFactory = packageReaderFactory;
         _packageSerializer = packageSerializer;
         _writer = writer;
      }

      public void PunditToNuGet()
      {
         Guard.NotEmpty(SourcePath, nameof(SourcePath));

         if (!Directory.Exists(SourcePath))
         {
            PunditToNuGetInternal();
            return;
         }

         var sourceDirectory = SourcePath;
         foreach (var file in Directory.GetFiles(sourceDirectory, "*.pundit", SearchOption.TopDirectoryOnly))
         {
            SourcePath = file;
            PunditToNuGetInternal();
         }
      }

      private void PunditToNuGetInternal()
      {
         if (!File.Exists(SourcePath))
            throw new FileNotFoundException("The given file does not exist", SourcePath);

         if (!PackageManifest.PunditPackageExtension.Equals(Path.GetExtension(SourcePath), StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("The given file is not a Pundit package");

         _writer.Empty().Text($"Processing '{SourcePath}'...");

         DestinationFolder = string.IsNullOrEmpty(DestinationFolder) ? Path.GetDirectoryName(SourcePath) : Path.GetFullPath(DestinationFolder);

         string tmpFolder;
         string manifestPath;

         using (var stream = File.Open(SourcePath, FileMode.Open, FileAccess.Read))
         {
            var reader = _packageReaderFactory.Get(RepositoryType.Pundit, stream);

            tmpFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            Directory.CreateDirectory(tmpFolder);

            var packageSpec = reader.ReadManifest().ToPackageSpec(); 
            //var packageSpec =  manifest.ToPackageSpec(PackageExtensions.GetFramework(manifest.LegacyFramework));

            FillMissingRequiredNuGetProperties(packageSpec);

            _writer.BeginWrite().Text($" Extracting {packageSpec.PackageId} [{packageSpec.Version}]... ");
            
            reader.ExtractTo(tmpFolder);

            foreach (var file in Directory.GetFiles(tmpFolder, "*", SearchOption.AllDirectories))
            {
               var path = file.Substring(tmpFolder.Length + 1).Replace('\\', '/');

               var sourceFile = new SourceFiles
               {
                  BaseDirectory = tmpFolder,
                  Include = file,
                  FileKind = PackageReader.GetKind(path),
               };

               if (sourceFile.FileKind == PackageFileKind.Binary)
               {
                  var config = path.Split('/')[1];
                  sourceFile.Configuration = (BuildConfiguration)Enum.Parse(typeof(BuildConfiguration), config, true);
                  sourceFile.BaseDirectory += "\\bin\\" + sourceFile.Configuration.ToString().ToLower();
               }
               else if (sourceFile.FileKind == PackageFileKind.Include)
                  sourceFile.BaseDirectory += "\\include";
               else if (sourceFile.FileKind == PackageFileKind.Tools)
                  sourceFile.BaseDirectory += "\\tools";
               else if (sourceFile.FileKind == PackageFileKind.Other)
                  sourceFile.BaseDirectory += "\\other";

               packageSpec.Files.Add(sourceFile);
            }

            if (packageSpec.Files.Count == 0)
               packageSpec.Files.Add(new SourceFiles{BaseDirectory =  tmpFolder, Include = "bin\\*"});

            manifestPath = Path.Combine(tmpFolder, "spec");
            using (var specStream = File.Open(manifestPath, FileMode.Create, FileAccess.Write))
               _packageSerializer.SerializePackageSpec(packageSpec, specStream);

            _writer.Success("ok").EndWrite();
         }

         _packService.Type = PackType.NuGet;
         _packService.OutputPath = DestinationFolder;
         _packService.ManifestFileOrPath = manifestPath;

         _writer.BeginWrite().Text(" RePacking... ");
         _packService.Pack();
         _writer.Success("ok").EndWrite();

         Directory.Delete(tmpFolder, true);
      }

      public void NuGetToPundit()
      {
         Guard.NotEmpty(SourcePath, nameof(SourcePath));

         if (!File.Exists(SourcePath))
            throw new FileNotFoundException("The given file does not exist", SourcePath);

         if (Path.GetExtension(SourcePath) != ".nupkg")
            throw new InvalidOperationException("The given file is not a NuGet package");

         if (string.IsNullOrEmpty(DestinationFolder))
            DestinationFolder = Path.GetDirectoryName(SourcePath);
         else
            DestinationFolder = Path.GetFullPath(DestinationFolder);
         
         var punditSpec = new PackageSpec();

         using (var stream = File.Open(SourcePath, FileMode.Open, FileAccess.Read, FileShare.Read))
         {
            var reader = new NuGet.Packaging.PackageArchiveReader(stream);

            punditSpec.PackageId = reader.NuspecReader.GetIdentity().Id;
            punditSpec.Version = reader.NuspecReader.GetVersion();
            punditSpec.Author = reader.NuspecReader.GetAuthors();
            punditSpec.Description = reader.NuspecReader.GetDescription();
            punditSpec.License = reader.NuspecReader.GetLicenseUrl();
            punditSpec.ProjectUrl = reader.NuspecReader.GetProjectUrl();
            punditSpec.ReleaseNotes = reader.NuspecReader.GetReleaseNotes();

            // TODO: Only bin supported for now
            foreach (var framework in reader.GetSupportedFrameworks())
            {
               if (!NuGet.Frameworks.NuGetFrameworkExtensions.IsDesktop(framework))
               {
                  _writer.Empty().Warning($"Framework '{framework}' is not supported, skipping...");
                  continue;
               }

               punditSpec.Dependencies = new List<PackageDependency>();
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

                     reader.ExtractFile(lib, Path.Combine(tempOut, lib), NuGet.Common.NullLogger.Instance);
                  }

                  var punditSpecFile = Path.Combine(tempOut, PackageManifest.DefaultManifestFileName);
                  //using (var outFileStream = File.Create(punditSpecFile))
                  //   new PunditPackageWriter(new XmlSerializer(), )
                  //   _packageSerializerFactory.GetPundit().SerializePackageSpec(punditSpec, outFileStream);

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

      private void FillMissingRequiredNuGetProperties(PackageSpec packageSpec)
      {
         // Fill missing fields from Pundit (happens quite often)
         if (string.IsNullOrEmpty(packageSpec.Description))
            packageSpec.Description = packageSpec.PackageId;

         if (string.IsNullOrEmpty(packageSpec.Author))
            packageSpec.Author = "Unknown";

         if (string.IsNullOrEmpty(packageSpec.ProjectUrl))
            packageSpec.ProjectUrl = "http://localhost/";
      }
   }
}
