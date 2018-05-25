using System;
using EBerzosa.Pundit.Core.Model.Package;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Package
{
   public interface IPackageReader : IDisposable
   {
      PackageManifest ReadManifest();

      void InstallTo(string rootFolder, PackageDependency originalDependency, BuildConfiguration configuration);

      void ExtractTo(string rootFolder);
   }
}