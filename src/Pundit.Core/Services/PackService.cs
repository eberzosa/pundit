using System;
using System.IO;
using EBerzosa.Pundit.Core.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Serializers;
using EBerzosa.Pundit.Core.Versioning;
using EBerzosa.Utils;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;
using Pundit.Core.Utils;

namespace EBerzosa.Pundit.Core.Services
{
   public class PackService
   {
      private readonly PackageSerializerFactory _packageSerializerFactory;
      private readonly ManifestResolver _manifestResolver;
      private readonly IWriter _writer;

      private string _resolvedOutputPath;
      private string _resolvedManifestPath;

      public string ManifestFileOrPath { get; set; }

      public string OutputPath { get; set; }

      public string Version { get; set; }

      public bool IsDeveloperPackage { get; set; }

      public string DestinationFile { get; private set; }


      public PackService(PackageSerializerFactory packageSerializerFactory, ManifestResolver manifestResolver, IWriter writer)
      {
         Guard.NotNull(packageSerializerFactory, nameof(packageSerializerFactory));
         Guard.NotNull(manifestResolver, nameof(manifestResolver));
         Guard.NotNull(writer, nameof(writer));

         _packageSerializerFactory = packageSerializerFactory;
         _manifestResolver = manifestResolver;
         _writer = writer;
      }

      public void Pack()
      {
         _resolvedManifestPath = _manifestResolver.GetManifest(ManifestFileOrPath);

         var solutionDirectory = Path.GetDirectoryName(_resolvedManifestPath);

         _resolvedOutputPath = Path.Combine(_manifestResolver.CurrentDirectory, OutputPath ?? solutionDirectory);
         
         if (!Directory.Exists(_resolvedOutputPath))
            throw new DirectoryNotFoundException($"Destination directory '{_resolvedOutputPath}' does not exist");

         _writer.BeginColumns(new int?[] {15, null})
            .Text("Package Spec:").Text(_resolvedManifestPath)
            .Text("Solution Root:").Text(solutionDirectory)
            .Text("Output Dir:").Text(_resolvedOutputPath)
            .EndColumns().Empty();

         PackageSpec packageSpec;
         using (Stream devPackStream = File.OpenRead(_resolvedManifestPath))
         {
            packageSpec = _packageSerializerFactory.GetPundit().DeserializePackageSpec(devPackStream);
         }

         if (Version != null)
         {
            _writer.Info($"Overriding package version '{packageSpec.Version}' from spec with '{Version}'");

            if (!IsDeveloperPackage)
               packageSpec.Version = PunditVersion.Parse(Version);
            else
            {
               if (Version.Split('.').Length > 4)
                  throw new ApplicationException($"Version format for '{Version}' not supported");

               var versionChunks = Version.Split('.');
               packageSpec.Version = new PunditVersion(int.Parse(versionChunks[0]), 
                  int.Parse(versionChunks[1]), int.Parse(versionChunks[2]), int.Parse(versionChunks[3]), VersionUtils.DevMarker, null);
            }
         }
         
         var packageName = new PackageFileName(packageSpec).FileName;

         DestinationFile = Path.Combine(_resolvedOutputPath, packageName);

         if (File.Exists(DestinationFile))
            _writer.Warning($"Package '{packageName}' already exists, it will be overwritted");

         _writer.Text($"Creating package '{packageName}'...");

         long bytesWritten;

         using (Stream writeStream = File.Create(DestinationFile))
         using (var packageWriter = new PackageWriter(_packageSerializerFactory, solutionDirectory, packageSpec, writeStream))
         {
            packageWriter.BeginPackingFile += PackageWriterOnBeginPackingFile;
            packageWriter.EndPackingFile += PackageWriterOnEndPackingFile;
            bytesWritten = packageWriter.WriteAll();
         }

         var packageSize = new FileInfo(DestinationFile).Length;

         _writer.Text($"Packed {PathUtils.FileSizeToString(bytesWritten)} to {PathUtils.FileSizeToString(packageSize)} (ratio: {packageSize * 100 / bytesWritten:D2}%)");
      }

      private void PackageWriterOnBeginPackingFile(object sender, PackageFileEventArgs e)
      {
         _writer.EndWrite().BeginWrite()
            .Text("Packing ").Success(e.FileName).Text("... ");
      }

      private void PackageWriterOnEndPackingFile(object sender, PackageFileEventArgs e)
      {
         _writer.Success("ok").EndWrite();
      }
   }
}
