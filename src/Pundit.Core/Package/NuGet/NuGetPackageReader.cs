using System;
using System.IO;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Utils;
using Mapster;
using NuGet.Frameworks;
using NuGet.Packaging;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;
using Pundit.Core.Utils;
using PackageDependency = EBerzosa.Pundit.Core.Model.Package.PackageDependency;

namespace EBerzosa.Pundit.Core.Package
{
   public class NuGetPackageReader : PackageStreamer, IPackageReader
   {
      public event EventHandler<ResolvedFileEventArgs> InstallingResolvedFile;

      private readonly PackageArchiveReader _packageReader;

      public NuGetPackageReader(Stream packageStream)
      {
         Guard.NotNull(packageStream, nameof(packageStream));

         _packageReader = new PackageArchiveReader(packageStream);
      }
      
      public PackageManifest ReadManifest() => throw new NotSupportedException();


      public void InstallTo(string rootFolder, PackageDependency originalDependency, BuildConfiguration configuration)
      {
         throw new NotImplementedException();

         foreach (var frameworkSpecificGroup in _packageReader.GetReferenceItems())
         {
            //if (frameworkSpecificGroup.TargetFramework != originalDependency.Framework.Adapt<NuGetFramework>())
            //   continue;

            foreach (var item in frameworkSpecificGroup.Items)
               InstallLibrary(_packageReader.GetIdentity().Id, rootFolder, item, configuration);
         }
      }

      private void InstallLibrary(string packageId, string root, string name, BuildConfiguration targetConfig)
      {
         var fileName = name.Substring(name.LastIndexOf("/") + 1);

         InstallingResolvedFile?.Invoke(null, new ResolvedFileEventArgs(packageId, PackageFileKind.Binary, targetConfig, fileName));

         string targetPath = Path.Combine(root, "lib");
         if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);

         targetPath = Path.Combine(targetPath, fileName);
         
         if (File.Exists(targetPath))
            File.Delete(targetPath);

         using (Stream ts = File.Create(targetPath))
            _packageReader.GetStream(name).CopyTo(ts);
      }

      protected override void Dispose(bool disposing) => _packageReader?.Dispose();
   }
}
