using System.Collections.Generic;
using EBerzosa.Pundit.Core.Repository;
using NuGet.Versioning;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Resolvers
{
   public interface IDependencyResolver
   {
      IEnumerable<NuGetVersion> GetVersions(IRepository repo, DependencyNode node);

      PackageManifest GetManifest(IRepository repo, DependencyNode node);
   }
}
