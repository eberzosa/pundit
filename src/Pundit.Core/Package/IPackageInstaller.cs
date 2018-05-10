using System;
using System.Collections.Generic;
using EBerzosa.Pundit.Core.Resolvers;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;

namespace EBerzosa.Pundit.Core.Package
{
   public interface IPackageInstaller : IDisposable
   {
      event EventHandler<PackageKeyDiffEventArgs> BeginInstallPackage;
      event EventHandler<PackageKeyDiffEventArgs> FinishInstallPackage;
      
      IEnumerable<PackageKeyDiff> GetDiffWithCurrent(IEnumerable<SatisfyingInfoExtended> resolutionResult);
      void Reinstall(BuildConfiguration configuration);
      void Upgrade(BuildConfiguration configuration, IEnumerable<PackageKeyDiff> diffs);
   }
}