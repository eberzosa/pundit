using System.IO;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Serializers;
using EBerzosa.Utils;
using ICSharpCode.SharpZipLib.Zip;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Package.Pundit
{
   public class PunditPackageReader : PackageReader
   {
      private readonly IPackageSerializer _packageSerializer;

      private ZipInputStream _zipStream;

      public PunditPackageReader(IPackageSerializer packageSerializer, Stream packageStream)
      {
         Guard.NotNull(packageSerializer, nameof(packageSerializer));
         Guard.NotNull(packageStream, nameof(packageStream));

         _packageSerializer = packageSerializer;
         _zipStream = new ZipInputStream(packageStream);
      }

      public override PackageManifest ReadManifest()
      {
         ZipEntry entry;

         while((entry = _zipStream.GetNextEntry()) != null)
         {
            if (entry.IsFile && entry.Name == PackageManifest.DefaultManifestFileName)
               return _packageSerializer.DeserializePackageManifest(_zipStream);
         }

         return null;
      }
      
      /// <summary>
      /// Installs the package to a specific location. If destination files exist
      /// they will be silently overwritten.
      /// </summary>
      /// <param name="rootFolder">Solution's root folder</param>
      /// <param name="originalDependency"></param>
      /// <param name="configuration">Desired configuration name</param>
      public override void InstallTo(string rootFolder, PackageDependency originalDependency, BuildConfiguration configuration)
      {
         PackageManifest pkg = ReadManifest();

         ZipEntry entry;
         while ((entry = _zipStream.GetNextEntry()) != null)
         {
            if (entry.IsFile)
               InstallToInternal(rootFolder, pkg.PackageId, configuration, entry.Name);
         }
      }

      public override void ExtractTo(string rootFolder)
      {
         ZipEntry entry;
         while ((entry = _zipStream.GetNextEntry()) != null)
            ExtractFileTo(rootFolder, entry.Name, entry.IsFile);
      }

      protected override void Dispose(bool disposing)
      {
         if (_zipStream != null)
         {
            _zipStream.Close();
            _zipStream.Dispose();
            _zipStream = null;
         }
      }

      protected override Stream GetSourceStream(string fileToInstall)
      {
         return _zipStream;
      }
   }
}
