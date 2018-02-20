using System;
using System.Collections.Generic;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;

namespace EBerzosa.Pundit.Core.Package
{
   public interface IPackageInstaller : IDisposable
   {
      event EventHandler<PackageKeyDiffEventArgs> BeginInstallPackage;
      event EventHandler<PackageKeyDiffEventArgs> FinishInstallPackage;
      
      IEnumerable<PackageKeyDiff> GetDiffWithCurrent(IEnumerable<PackageKey> resolutionResult);
      void Reinstall(BuildConfiguration configuration);
      void Upgrade(BuildConfiguration configuration, IEnumerable<PackageKeyDiff> diffs);
   }
}