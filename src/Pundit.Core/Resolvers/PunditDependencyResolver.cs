using System.Collections.Generic;
using EBerzosa.Pundit.Core.Model;
using NuGet.Versioning;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Resolvers
{
   internal class PunditDependencyResolver : IDependencyResolver
   {
      public IEnumerable<NuGetVersion> GetVersions(IRepository repo, DependencyNode node)
         => repo.GetVersions(node.UnresolvedPackage);

      public PackageManifest GetManifest(IRepository repo, DependencyNode node)
         => repo.GetManifest(node.ActiveVersionKey);
   }
}
