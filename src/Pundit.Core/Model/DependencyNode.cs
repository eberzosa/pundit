using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;
using NuGet.Versioning;

namespace Pundit.Core.Model
{
   public class DependencyNode : ICloneable
   {
      private readonly DependencyNode _parentNode;
      private readonly string _packageId;
      private readonly string _platform;

      private readonly VersionRange _versionPattern;
      private readonly bool _includeDeveloperPackages;
      private NuGetVersion[] _versions; //versions satisfying version pattern
      private int _activeVersionIndex = -1;  //active version index in _versions

      //node's dependencies
      private readonly List<DependencyNode> _children = new List<DependencyNode>();

      public DependencyNode(DependencyNode parentNode,
         string packageId, string platform, VersionRange versionPattern, bool includeDeveloperPackages)
      {
         _parentNode = parentNode;
         _packageId = packageId;
         _platform = platform;
         _versionPattern = versionPattern;
         _includeDeveloperPackages = includeDeveloperPackages;
      }

      public string PackageId { get { return _packageId; } }

      public string Platform { get { return _platform; } }
      
      public VersionRange VersionPattern { get { return _versionPattern; } }

      public IEnumerable<DependencyNode> Children { get { return _children; } }

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

      public void MarkAsRoot(PackageManifest rootManifest)
      {
         SetVersions(new[] {rootManifest.Version});
         SetManifest(rootManifest);
      }

      public void SetVersions(NuGetVersion[] versions)
      {
         _versions = versions ?? throw new ArgumentNullException(nameof(versions));

         Array.Sort(_versions);

         if(_versions.Length > 0) _activeVersionIndex = _versions.Length - 1;

         HasVersions = true;

         _children.Clear();
         HasManifest = (versions.Length == 0); //if there are no versions, no point to fetch manifest
      }

      public void SetManifest(PackageManifest thisManifest)
      {
         if (thisManifest == null)
            throw new ArgumentNullException(nameof(thisManifest));

         _children.Clear();

         foreach(PackageDependency pd in thisManifest.Dependencies)
         {
            _children.Add(new DependencyNode(this, pd.PackageId, pd.Platform, pd.VersionPattern, _includeDeveloperPackages));
         }

         HasManifest = true;
      }

      public bool HasVersions { get; private set; }

      public bool HasManifest { get; private set; }

      public bool IsFull => HasVersions && HasManifest;

      public bool IsRecursivelyFull
      {
         get
         {
            foreach(DependencyNode child in _children)
            {
               if(!child.IsRecursivelyFull)
               {
                  return false;
               }
            }

            return IsFull;
         }
      }

      public NuGetVersion ActiveVersion
      {
         get
         {
            if (_activeVersionIndex == -1) return null;

            return _versions[_activeVersionIndex];
         }
      }

      /// <summary>
      /// Versions from lowest to latest active (<see cref="ActiveVersion"/>)
      /// </summary>
      public IEnumerable<NuGetVersion> ActiveVersions
      {
         get
         {
            var active = new List<NuGetVersion>();

            for (int i = 0; i <= _activeVersionIndex; i++ )
            {
               active.Add(_versions[i]);   
            }

            return active;
         }
      }

      public IEnumerable<NuGetVersion> AllVersions => _versions;

      public PackageKey ActiveVersionKey
      {
         get
         {
            if(ActiveVersion == null)
               throw new ArgumentException("node has no active version");

            return new PackageKey(_packageId, ActiveVersion, _platform);
         }
      }

      public UnresolvedPackage UnresolvedPackage => new UnresolvedPackage(_packageId, _platform, VersionPattern, _includeDeveloperPackages);
      
      public bool CanDowngrade => _activeVersionIndex > 0;


      public object Clone()
      {
         var node = new DependencyNode(_parentNode, _packageId, _platform, _versionPattern, _includeDeveloperPackages)
         {
            HasVersions = HasVersions
         };

         if(_versions != null)
         {
            node._versions = new NuGetVersion[_versions.Length];

            if(_versions.Length > 0)
            {
               Array.Copy(_versions, node._versions, _versions.Length);
            }
         }

         node._activeVersionIndex = _activeVersionIndex;

         //manifest

         node.HasManifest = HasManifest;

         foreach(DependencyNode child in _children)
         {
            node._children.Add((DependencyNode)child.Clone());
         }

         return node;
      }
   }
}
