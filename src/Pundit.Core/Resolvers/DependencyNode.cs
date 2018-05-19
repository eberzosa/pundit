using System;
using System.Collections.Generic;
using System.Linq;
using EBerzosa.Pundit.Core.Framework;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Versioning;

namespace EBerzosa.Pundit.Core.Resolvers
{
   public class DependencyNode : ICloneable
   {
      private readonly DependencyNode _parentNode;

      private readonly string _useReleasePackages;

      private SatisfyingInfo[] _satisfyingData;
      private int _activeVersionIndex = -1;

      //node's dependencies
      private readonly List<DependencyNode> _children = new List<DependencyNode>();
      private readonly FloatRange _allowedVersions;

      public string PackageId { get; }

      public PunditFramework Framework { get; }

      public FloatRange AllowedVersions
      {
         get
         {
            if (_useReleasePackages == null)
               return _allowedVersions;

            FloatBehaviour behaviour;
            if (_allowedVersions.FloatBehaviour == FloatBehaviour.Major)
               behaviour = FloatBehaviour.MajorPrerelease;
            else if (_allowedVersions.FloatBehaviour == FloatBehaviour.Minor)
               behaviour = FloatBehaviour.MinorPrerelease;
            else if (_allowedVersions.FloatBehaviour == FloatBehaviour.Patch)
               behaviour = FloatBehaviour.PatchPrerelease;
            else if (_allowedVersions.FloatBehaviour == FloatBehaviour.Revision)
               behaviour = FloatBehaviour.RevisionPrerelease;
            else
               behaviour = _allowedVersions.FloatBehaviour;

            return new FloatRange(behaviour, _allowedVersions.MinVersion, _useReleasePackages);
         }
      }

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

      public UnresolvedPackage UnresolvedPackage => new UnresolvedPackage(PackageId, Framework, _allowedVersions);

      public bool CanDowngrade => _activeVersionIndex > 0;

      public bool HasVersions => _satisfyingData != null;

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

      public IEnumerable<PunditVersion> AllVersions => _satisfyingData.Select(v => v.Version);

      public SatisfyingInfo ActiveVersion => _activeVersionIndex == -1 ? null : _satisfyingData[_activeVersionIndex];

      /// <summary>
      /// Versions from lowest to latest active (<see cref="ActiveVersion"/>)
      /// </summary>
      public IEnumerable<SatisfyingInfo> ActiveSatisfayingData
      {
         get
         {
            var active = new List<SatisfyingInfo>();

            for (int i = 0; i <= _activeVersionIndex; i++)
               active.Add(_satisfyingData[i]);

            return active;
         }
      }

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


      public DependencyNode(DependencyNode parentNode,
         string packageId, PunditFramework framework, FloatRange allowedVersions, string useReleasePackages)
      {
         _parentNode = parentNode;
         PackageId = packageId;
         Framework = framework;
         _allowedVersions = allowedVersions;
         _useReleasePackages = useReleasePackages;


         //if (_useReleasePackages == null)
         //{
         //   AllowedVersions = allowedVersions;
         //   return;
         //}

         //FloatBehaviour behaviour;
         //if (allowedVersions.FloatBehaviour == FloatBehaviour.Major)
         //   behaviour = FloatBehaviour.MajorPrerelease;
         //else if (allowedVersions.FloatBehaviour == FloatBehaviour.Minor)
         //   behaviour = FloatBehaviour.MinorPrerelease;
         //else if (allowedVersions.FloatBehaviour == FloatBehaviour.Patch)
         //   behaviour = FloatBehaviour.PatchPrerelease;
         //else if (allowedVersions.FloatBehaviour == FloatBehaviour.Revision)
         //   behaviour = FloatBehaviour.RevisionPrerelease;
         //else
         //   behaviour = allowedVersions.FloatBehaviour;

         //AllowedVersions = new FloatRange(behaviour, allowedVersions.MinVersion, _useReleasePackages);
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

         _satisfyingData = versions.ToArray();
         Array.Sort(_satisfyingData);

         if(_satisfyingData.Length > 0)
            _activeVersionIndex = _satisfyingData.Length - 1;

         _children.Clear();
         HasManifest = _satisfyingData.Length == 0; //if there are no versions, no point to fetch manifest
      }

      public void RemoveActiveVersion()
      {
         if (_activeVersionIndex < 0)
            return;

         Array.Resize(ref _satisfyingData, _satisfyingData.Length - 1);
         _activeVersionIndex--;
      }

      public void SetManifest(PackageManifest thisManifest)
      {
         if (thisManifest == null)
            throw new ArgumentNullException(nameof(thisManifest));

         _children.Clear();

         foreach (PackageDependency pd in thisManifest.Dependencies)
            _children.Add(new DependencyNode(this, pd.PackageId, pd.Framework ?? Framework, pd.AllowedVersions, _useReleasePackages));

         HasManifest = true;
      }

      public object Clone()
      {
         var node = new DependencyNode(_parentNode, PackageId, Framework, _allowedVersions, _useReleasePackages);

         if (_satisfyingData != null)
         {
            node._satisfyingData = new SatisfyingInfo[_satisfyingData.Length];

            for (int i = 0; i < node._satisfyingData.Length; i++)
               node._satisfyingData[i] = new SatisfyingInfo(_satisfyingData[0].Version, _satisfyingData[0].Repo);
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

      public override string ToString() => PackageId + "[" + AllowedVersions + "]" + "[" + Framework + "]";
   }
}
