using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Serializers;
using Pundit.Core.Model;
using XmlSerializer = System.Xml.Serialization.XmlSerializer;

namespace Pundit.Core.Application
{
   public class InstalledPackagesManager
   {
      private const string CacheFileName = ".pundit-install-index";

      private readonly InstalledPackagesIndexSerializer _installedPackagesIndexSerializer;

      public InstalledPackagesManager(InstalledPackagesIndexSerializer installedPackagesIndexSerializer)
      {
         _installedPackagesIndexSerializer = installedPackagesIndexSerializer;
      }

      public InstalledPackagesIndex GetNew(string rootFolder, BuildConfiguration configuration)
      {
         return new InstalledPackagesIndex(rootFolder) {Configuration = configuration};
      }

      public InstalledPackagesIndex ReadFromFolder(string rootFolder)
      {
         var fullPath = Path.Combine(rootFolder, CacheFileName);

         if (!File.Exists(fullPath))
            return new InstalledPackagesIndex(fullPath);

         using (Stream s = File.OpenRead(fullPath))
            return _installedPackagesIndexSerializer.DeserializeInstalledPackagesIndex(s);
      }

      public void Save(InstalledPackagesIndex installedPackagesIndex)
      {
         using (Stream stream = File.Create(installedPackagesIndex.Location))
            _installedPackagesIndexSerializer.SerializePackageManifest(installedPackagesIndex, stream);
         
         new FileInfo(installedPackagesIndex.Location).Attributes |= FileAttributes.Hidden;
      }
   }

   public class InstalledPackagesIndex
   {
      private HashSet<PackageKey> _installed = new HashSet<PackageKey>();

      private bool _dirty;

      public string Location { get; }

      public IEnumerable<PackageKey> InstalledPackages
      {
         get => _installed;
         set => _installed = new HashSet<PackageKey>(value);
      }

      public BuildConfiguration Configuration { get; set; }

      public int TotalPackagesCount => _installed.Count;


      public InstalledPackagesIndex(string location)
      {
         Location = location;
      }

      public bool IsEmpty() => _installed == null || !_installed.Any();

      public bool IsInstalled(PackageKey pck) => _installed.Contains(pck);

      public PackageKey GetLoose(PackageKey packageKey) => _installed.FirstOrDefault(p => p.LooseEquals(packageKey));

      public IEnumerable<PackageKey> GetLooseNotIn(IEnumerable<PackageKey> packageKeys)
      {
         return _installed.Where(i => packageKeys.FirstOrDefault(p => p.LooseEquals(i)) == null);
      }

      public void Install(PackageKey pck)
      {
         PackageKey installedPackage = _installed.FirstOrDefault(k => k.LooseEquals(pck));

         if (installedPackage != null)
            _installed.Remove(installedPackage);
      }

      public void Uninstall(PackageKey pck)
      {
         if (_installed.Contains(pck))
            _installed.Remove(pck);
      }
   }
}