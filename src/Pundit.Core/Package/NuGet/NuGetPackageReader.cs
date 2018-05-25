using System;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Utils;
using NuGet.Packaging;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Package
{
   public class NuGetPackageReader : PackageReader
   {
      private readonly PackageArchiveReader _packageReader;

      public NuGetPackageReader(Stream packageStream)
      {
         Guard.NotNull(packageStream, nameof(packageStream));

         _packageReader = new PackageArchiveReader(packageStream);
      }
      
      public override PackageManifest ReadManifest() => throw new NotSupportedException();


      public override void InstallTo(string rootFolder, PackageDependency originalDependency, BuildConfiguration configuration)
      {
         var packageId = _packageReader.GetIdentity().Id;

         foreach (var file in _packageReader.GetFiles().Where(ShouldInclude))
            InstallToInternal(rootFolder, packageId, configuration, file);
      }

      public override void ExtractTo(string rootFolder)
      {
         foreach (var file in _packageReader.GetFiles().Where(ShouldInclude))
            ExtractFileTo(rootFolder, file, true);
      }


      protected override Stream GetSourceStream(string fileToInstall) => _packageReader.GetStream(fileToInstall);
      
      protected override void Dispose(bool disposing) => _packageReader?.Dispose();

      private static bool ShouldInclude(string fullName)
      {
         // Not all the files from a zip file are needed
         // So, files such as '.rels' and '[Content_Types].xml' are not extracted
         var fileName = Path.GetFileName(fullName);
         if (fileName != null)
         {
            if (fileName == ".rels")
               return false;

            if (fileName == "[Content_Types].xml")
               return false;
         }

         var extension = Path.GetExtension(fullName);
         if (extension == ".psmdcp")
            return false;

         //if (string.Equals(fullName, hashFileName, StringComparison.OrdinalIgnoreCase))
         //   return false;

         // Skip nupkgs and nuspec files found in the root, the valid ones are already extracted
         if (PackageHelper.IsRoot(fullName) &&
             (PackageHelper.IsNuspec(fullName) || fullName.EndsWith(".nupkg", StringComparison.OrdinalIgnoreCase)))
         {
            return false;
         }

         return true;
      }
   }
}
