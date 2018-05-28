using System;
using System.Collections.Generic;
using System.Linq;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Versioning;

namespace EBerzosa.Pundit.Core.Resolvers
{
   public class DependencyNode : ICloneable
   {
      private readonly DependencyNode _parentNode;
      
      private List<SatisfyingInfo> _satisfyingData;
      
      //node's dependencies
      private readonly List<DependencyNode> _children = new List<DependencyNode>();

      public string PackageId { get; }

      [Obsolete("Used in Pundit packages only.")]
      public string Framework { get; private set; }

      public VersionRangeExtended AllowedVersions { get; }

      public IEnumerable<DependencyNode> Children => _children;

      // This is hardcoded as a temp hack
      public bool MustBeNuGet => _parentNode.ActiveRepository is NuGetFileSystemRepo;
         
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

      public UnresolvedPackage UnresolvedPackage => new UnresolvedPackage(PackageId, Framework, AllowedVersions);

      public bool CanDowngrade => _satisfyingData.Count > 0;

      public bool HasVersions => _satisfyingData != null;

      public bool HasManifest { get; private set; }

      public bool IsFull => HasVersions && HasManifest;

      public bool IsRecursivelyFull
      {
         get
         {
            foreach (DependencyNode child in _children)
               if (!child.IsRecursivelyFull)
                  return false;

            return IsFull;
         }
      }

      public IEnumerable<NuGet.Versioning.NuGetVersion> AllVersions => _satisfyingData.Select(v => v.Version);

      public SatisfyingInfo ActiveVersion { get; private set; }


      /// <summary>
      /// Versions from lowest to latest active (<see cref="ActiveVersion"/>)
      /// </summary>
      public IEnumerable<SatisfyingInfo> ActiveSatisfayingData => _satisfyingData;

      public PackageKey ActiveVersionKey
      {
         get
         {
            if (ActiveVersion == null)
               throw new ArgumentException("node has no active version");

            return new PackageKey(PackageId, ActiveVersion.Version, Framework);
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


      public DependencyNode(DependencyNode parentNode, string packageId, string framework, VersionRangeExtended allowedVersions)
      {
         _parentNode = parentNode;
         PackageId = packageId;
         Framework = framework;
         AllowedVersions = allowedVersions;
      }


      public void MarkAsRoot(PackageManifest rootManifest)
      {
         SetVersions(new[] {new SatisfyingInfo(rootManifest.Version, null)});
         SetManifest(rootManifest);
      }
      
      public void SetVersions(IEnumerable<SatisfyingInfo> versions)
      {
         if (versions == null)
            throw new ArgumentNullException(nameof(versions));

         _satisfyingData = versions.OrderByDescending(s => s.Version, AllowedVersions.Comparer).ToList();

         _children.Clear();
         HasManifest = _satisfyingData.Count == 0; //if there are no versions, no point to fetch manifest

         SetActiveVersion();
      }

      public void RemoveActiveVersion()
      {
         if (_satisfyingData.Count <= 0)
            return;

         _satisfyingData.Remove(ActiveVersion);

         SetActiveVersion();
      }

      public void SetManifest(PackageManifest thisManifest)
      {
         if (thisManifest == null)
            throw new ArgumentNullException(nameof(thisManifest));

         _children.Clear();

         foreach (PackageDependency pd in thisManifest.Dependencies)
            _children.Add(new DependencyNode(this, pd.PackageId, pd.Framework,
               new VersionRangeExtended(pd.AllowedVersions) {ReleaseLabel = AllowedVersions.ReleaseLabel}));

         HasManifest = true;

         // TODO: Hack to populate the Framework to Pundit packages which come from a dependency of a NuGet as NuGet does not have a FW defined in the Manifest
         if (Framework == null && thisManifest.LegacyFramework != null)
            Framework = thisManifest.LegacyFramework;
      }

      public object Clone()
      {
         var node = new DependencyNode(_parentNode, PackageId, Framework, AllowedVersions);

         if (_satisfyingData != null)
         {
            node._satisfyingData = new List<SatisfyingInfo>();

            for (int i = 0; i < node._satisfyingData.Count; i++)
               node._satisfyingData.Add(new SatisfyingInfo(_satisfyingData[0].Version, _satisfyingData[0].Repo));
         }

         node.ActiveVersion = new SatisfyingInfo(ActiveVersion.Version, ActiveVersion.Repo);

         //manifest

         node.HasManifest = HasManifest;

         foreach (DependencyNode child in _children)
            node._children.Add((DependencyNode)child.Clone());

         return node;
      }

      public override string ToString() => PackageId + "[" + AllowedVersions + "]" + "[" + Framework + "]";

      private void SetActiveVersion()
      {
         if (_satisfyingData.Count == 0)
         {
            ActiveVersion = null;
            return;
         }

         var version = AllowedVersions.FindBestMatch(_satisfyingData.Select(s => s.Version));
         if (version == null)
            throw new InvalidOperationException("No best version found");

         ActiveVersion = _satisfyingData.FirstOrDefault(s => s.Version == version);
      }
   }
}
