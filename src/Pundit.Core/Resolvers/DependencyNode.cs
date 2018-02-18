using System;
using System.Collections.Generic;
using System.Linq;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Repository;
using NuGet.Versioning;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Resolvers
{
   public class DependencyNode : ICloneable
   {
      private readonly DependencyNode _parentNode;

      private readonly bool _includeDeveloperPackages;

      private LocationInfo[] _versions; //versions satisfying version pattern
      private int _activeVersionIndex = -1;  //active version index in _versions

      //node's dependencies
      private readonly List<DependencyNode> _children = new List<DependencyNode>();

      public string PackageId { get; }

      public string Platform { get; }

      public VersionRange VersionPattern { get; }

      public IEnumerable<DependencyNode> Children => _children;

      public string Path
      {
         get
         {
            var pp = new List<string>();

            var current = this;

            do
            {
               pp.Add(current.PackageId);
               current = current._parentNode;
            } while (current != null);

            pp.Reverse();
            return "/" + string.Join("/", pp);
         }
      }

      public UnresolvedPackage UnresolvedPackage => new UnresolvedPackage(PackageId, Platform, VersionPattern, _includeDeveloperPackages);

      public bool CanDowngrade => _activeVersionIndex > 0;

      public bool HasVersions => _versions != null;

      public bool HasManifest { get; private set; }

      public bool IsFull => HasVersions && HasManifest;

      public bool IsRecursivelyFull
      {
         get
         {
            foreach (DependencyNode child in _children)
            {
               if (!child.IsRecursivelyFull)
               {
                  return false;
               }
            }

            return IsFull;
         }
      }

      public IEnumerable<NuGetVersion> AllVersions => _versions.Select(v => v.Version);

      public LocationInfo ActiveVersion => _activeVersionIndex == -1 ? null : _versions[_activeVersionIndex];

      /// <summary>
      /// Versions from lowest to latest active (<see cref="ActiveVersion"/>)
      /// </summary>
      public IEnumerable<NuGetVersion> ActiveVersions
      {
         get
         {
            var active = new List<NuGetVersion>();

            for (int i = 0; i <= _activeVersionIndex; i++)
            {
               active.Add(_versions[i].Version);
            }

            return active;
         }
      }

      public PackageKey ActiveVersionKey
      {
         get
         {
            if (ActiveVersion == null)
               throw new ArgumentException("node has no active version");

            return new PackageKey(PackageId, ActiveVersion.Version, Platform);
         }
      }

      public IRepository ActiveRepository
      {
         get
         {
            if (ActiveVersion == null)
               throw new ArgumentException("node has no active version");

            return ActiveVersion.Repo;
         }
      }

      public IDependencyResolver ActiveResolver
      {
         get
         {
            if (ActiveVersion == null)
               throw new ArgumentException("node has no active version");

            return ActiveVersion.Resolver;
         }
      }


      public DependencyNode(DependencyNode parentNode,
         string packageId, string platform, VersionRange versionPattern, bool includeDeveloperPackages)
      {
         _parentNode = parentNode;
         PackageId = packageId;
         Platform = platform;
         VersionPattern = versionPattern;
         _includeDeveloperPackages = includeDeveloperPackages;
      }


      public void MarkAsRoot(PackageManifest rootManifest)
      {
         SetVersions(new[] {new LocationInfo(rootManifest.Version, null, null)});
         SetManifest(rootManifest);
      }
      
      public void SetVersions(IEnumerable<LocationInfo> versions)
      {
         if (versions == null)
            throw new ArgumentNullException(nameof(versions));

         _versions = versions.ToArray();
         Array.Sort(_versions);

         if(_versions.Length > 0)
            _activeVersionIndex = _versions.Length - 1;

         _children.Clear();
         HasManifest = _versions.Length == 0; //if there are no versions, no point to fetch manifest
      }

      public void RemoveActiveVersion()
      {
         if (_activeVersionIndex < 0)
            return;

         Array.Resize(ref _versions, _versions.Length - 1);
         _activeVersionIndex--;
      }

      public void SetManifest(PackageManifest thisManifest)
      {
         if (thisManifest == null)
            throw new ArgumentNullException(nameof(thisManifest));

         _children.Clear();

         foreach (PackageDependency pd in thisManifest.Dependencies)
            _children.Add(new DependencyNode(this, pd.PackageId, pd.Platform ?? Platform, pd.VersionPattern, _includeDeveloperPackages));

         HasManifest = true;
      }

      public object Clone()
      {
         var node = new DependencyNode(_parentNode, PackageId, Platform, VersionPattern, _includeDeveloperPackages);

         if (_versions != null)
         {
            node._versions = new LocationInfo[_versions.Length];

            for (int i = 0; i < node._versions.Length; i++)
               node._versions[i] = new LocationInfo(_versions[0].Version, _versions[0].Repo, _versions[0].Resolver);
         }

         node._activeVersionIndex = _activeVersionIndex;

         //manifest

         node.HasManifest = HasManifest;

         foreach (DependencyNode child in _children)
         {
            node._children.Add((DependencyNode)child.Clone());
         }

         return node;
      }
   }
}
