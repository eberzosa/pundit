using System.Collections.Generic;
using System.Linq;
using EBerzosa.Pundit.Core.Model.Package;
using Pundit.Core.Model;

namespace Pundit.Core.Application
{
   public class InstalledPackagesIndex
   {
      public HashSet<PackageKey> InstalledPackages { get; set; }

      public string Location { get; }
      
      public BuildConfiguration Configuration { get; set; }

      public int TotalPackagesCount => InstalledPackages.Count;


      public InstalledPackagesIndex(string location)
      {
         InstalledPackages = new HashSet<PackageKey>();
         Location = location;
      }

      public bool IsEmpty() => InstalledPackages == null || !InstalledPackages.Any();

      public bool IsInstalled(PackageKey pck) => InstalledPackages.Contains(pck);

      public PackageKey GetLoose(PackageKey packageKey) => InstalledPackages.FirstOrDefault(p => p.LooseEquals(packageKey));

      public IEnumerable<PackageKey> GetLooseNotIn(IEnumerable<PackageKey> packageKeys)
      {
         return InstalledPackages.Where(i => packageKeys.FirstOrDefault(p => p.LooseEquals(i)) == null);
      }

      public void Install(PackageKey pck)
      {
         PackageKey installedPackage = InstalledPackages.FirstOrDefault(k => k.LooseEquals(pck));

         if (installedPackage != null)
            InstalledPackages.Remove(installedPackage);

         InstalledPackages.Add(pck);
      }

      public void Uninstall(PackageKey pck)
      {
         if (InstalledPackages.Contains(pck))
            InstalledPackages.Remove(pck);
      }
   }
}