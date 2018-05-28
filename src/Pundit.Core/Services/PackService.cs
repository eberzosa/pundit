using System;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Serializers;
using EBerzosa.Pundit.Core.Versioning;
using EBerzosa.Utils;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace EBerzosa.Pundit.Core.Services
{
   public class PackService
   {
      private readonly IPackageSerializer _packageSerializer;
      private readonly ManifestResolver _manifestResolver;
      private readonly IWriter _writer;

      private string _resolvedOutputPath;
      private string _packageSpecFile;

      public string ManifestFileOrPath { get; set; }

      public string OutputPath { get; set; }

      public string Version { get; set; }

      public string ReleaseLabel { get; set; }

      public PackType Type { get; set; }

      public string DestinationFile { get; private set; }



      public PackService(IPackageSerializer packageSerializer, ManifestResolver manifestResolver, IWriter writer)
      {
         Guard.NotNull(packageSerializer, nameof(packageSerializer));
         Guard.NotNull(manifestResolver, nameof(manifestResolver));
         Guard.NotNull(writer, nameof(writer));

         _packageSerializer = packageSerializer;
         _manifestResolver = manifestResolver;
         _writer = writer;
      }

      public void Pack()
      {
         _packageSpecFile = _manifestResolver.GetManifest(ManifestFileOrPath);

         var solutionDirectory = Path.GetDirectoryName(_packageSpecFile);

         _resolvedOutputPath =  OutputPath == null || !Path.IsPathRooted(OutputPath)
            ? Path.Combine(_manifestResolver.CurrentDirectory, OutputPath ?? solutionDirectory)
            : OutputPath;
         
         if (!Directory.Exists(_resolvedOutputPath))
            throw new DirectoryNotFoundException($"Destination directory '{_resolvedOutputPath}' does not exist");

         _writer.BeginColumns(new int?[] {15, null})
            .Text("Package Spec:").Text(_packageSpecFile)
            .Text("Solution Root:").Text(solutionDirectory)
            .Text("Output Dir:").Text(_resolvedOutputPath)
            .EndColumns().Empty();

         PackageSpec packageSpec;
         using (Stream packageSpecStream = File.OpenRead(_packageSpecFile))
         {
            packageSpec = _packageSerializer.DeserializePackageSpec(packageSpecStream);
            packageSpec.Validate();
         }

         if (Version != null)
         {
            _writer.Info($"Overriding package version '{packageSpec.Version}' from spec with '{Version}'");
            packageSpec.Version = NuGet.Versioning.NuGetVersion.Parse(Version);
         }

         if (ReleaseLabel != null)
         {
            packageSpec.Version = new NuGet.Versioning.NuGetVersion(packageSpec.Version.Major,
               packageSpec.Version.Minor, packageSpec.Version.Patch, ReleaseLabel + "." + packageSpec.Version.Revision, null);
         }

         if (!string.IsNullOrEmpty(packageSpec.Version.Release))
            packageSpec.Version = packageSpec.Version.RevisionToLabel();

         var packageName = Type == PackType.NuGet
            ? new NuGet.Packaging.VersionFolderPathResolver(null).GetPackageFileName(packageSpec.PackageId, packageSpec.Version)
            : packageSpec.GetNewManifestFileName();
         
         DestinationFile = Path.Combine(_resolvedOutputPath, packageName);

         if (File.Exists(DestinationFile))
            _writer.Warning($"Package '{packageName}' already exists, it will be overwritted");

         _writer.Text($"Creating package '{packageName}', {packageSpec}...");

         long bytesWritten;

         using (Stream writeStream = File.Create(DestinationFile))
         using (var packageWriter = GetWriter(solutionDirectory, packageSpec, writeStream))
         {
            packageWriter.OnBeginPackingFile = p => _writer.EndWrite().BeginWrite().Text("Packing ").Success(p.FileName).Text("... ");
            packageWriter.OnEndPackingFile = p => _writer.Success("ok").EndWrite();

            bytesWritten = packageWriter.WriteAll();
         }

         var packageSize = new FileInfo(DestinationFile).Length;

         var written = PathUtils.FileSizeToString(bytesWritten);
         var packed = PathUtils.FileSizeToString(packageSize);
         var ratio = bytesWritten == 0 ? 100 : packageSize * 100 / bytesWritten;
         _writer.Text($"Packed {written} to {packed} (ratio: {ratio:D2}%)");
      }

      private IPackageWriter GetWriter(string rootDirectory, PackageSpec packageSpec, Stream outputStream)
      {
         if (Type == PackType.Pundit)
            return new PunditPackageWriter(_packageSerializer, rootDirectory, packageSpec, outputStream);

         if (Type == PackType.NuGet)
            return new NuGetv3PackageWriter(rootDirectory, packageSpec, outputStream);

         throw new NotSupportedException();
      }
   }
}
