using System;
using System.Collections.Generic;
using EBerzosa.Pundit.Core.Model.Package;

namespace EBerzosa.Pundit.Core.Resolvers
{
   internal class DependencyResolverCache
   {
      private readonly Dictionary<string, List<SatisfyingInfo>> _versionCache;
      private readonly Dictionary<PackageKey, PackageManifest> _manifestCache;

      private readonly Func<DependencyNode, IEnumerable<SatisfyingInfo>> _fallbackSatisfyingInfos;
      private readonly NuGet.Frameworks.NuGetFramework _framework;

      public DependencyResolverCache(Func<DependencyNode, IEnumerable<SatisfyingInfo>> fallbackSatisfyingInfos, NuGet.Frameworks.NuGetFramework framework)
      {
         _versionCache = new Dictionary<string, List<SatisfyingInfo>>();
         _manifestCache = new Dictionary<PackageKey, PackageManifest>();

         _fallbackSatisfyingInfos = fallbackSatisfyingInfos;
         _framework = framework;
      }

      public IEnumerable<SatisfyingInfo> GetSatisfyingInfos(DependencyNode node)
      {
         if (_versionCache.ContainsKey(node.PackageId))
            return _versionCache[node.PackageId];

         _versionCache.Add(node.PackageId, new List<SatisfyingInfo>());

         _versionCache[node.PackageId].AddRange(_fallbackSatisfyingInfos(node));

         return _versionCache[node.PackageId];
      }

      public PackageManifest GetManifest(DependencyNode node)
      {
         if (!_manifestCache.ContainsKey(node.ActiveVersionKey))
            _manifestCache[node.ActiveVersionKey] = node.ActiveRepository.GetManifest(node.ActiveVersionKey, _framework);

         return _manifestCache[node.ActiveVersionKey];
      }
   }
}