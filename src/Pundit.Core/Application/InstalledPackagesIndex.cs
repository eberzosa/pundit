using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Pundit.Core.Model;

namespace Pundit.Core.Application
{
   public class InstalledPackagesIndex
   {
      private const string CacheFileName = ".pundit-install-index";
      private readonly Dictionary<PackageKey, bool> _installed = new Dictionary<PackageKey, bool>();

      [XmlArray("installed")]
      [XmlArrayItem("package")]
      public PackageKey[] InstalledPackages
      {
         get { return _installed.Keys.ToArray(); }
         set
         {
            _installed.Clear();

            foreach (PackageKey key in value)
               _installed[key] = true;
         }
      }

      [XmlAttribute("configuration")]
      public BuildConfiguration Configuration { get; set; }

      [XmlIgnore]
      public int TotalPackagesCount
      {
         get { return _installed.Count; }
      }

      public bool IsInstalled(PackageKey pck)
      {
         return _installed.ContainsKey(pck);
      }

      public void Install(PackageKey pck)
      {
         PackageKey installedPackage = _installed.Keys.FirstOrDefault(k => k.LooseEquals(pck));

         if(installedPackage != null)
         {
            _installed.Remove(installedPackage);
         }

         PackageKey pck1 = (PackageKey)pck.Clone();

         _installed[pck1] = true;
      }

      public void Uninstall(PackageKey pck)
      {
         if (_installed.ContainsKey(pck))
            _installed.Remove(pck);
      }

      public static InstalledPackagesIndex ReadFromFolder(string folder)
      {
         var xml = new XmlSerializer(typeof(InstalledPackagesIndex));
         string fullPath = Path.Combine(folder, CacheFileName);

         if (File.Exists(fullPath))
         {
            using (Stream s = File.OpenRead(fullPath))
            {
               InstalledPackagesIndex r = null;

               try
               {
                  r = (InstalledPackagesIndex)xml.Deserialize(s);
               }
               catch(InvalidOperationException)
               {
                     
               }
               finally
               {
                  s.Close();
               }

               return r ?? new InstalledPackagesIndex();
            }
         }

         return new InstalledPackagesIndex();
      }

      public void WriteToFolder(string folder)
      {
         var xml = new XmlSerializer(typeof(InstalledPackagesIndex));
         string fullPath = Path.Combine(folder, CacheFileName);

         if(File.Exists(fullPath)) File.Delete(fullPath);

         using (Stream s = File.Create(fullPath))
         {
            xml.Serialize(s, this);
         }

         var fi = new FileInfo(fullPath);
         fi.Attributes |= FileAttributes.Hidden;
      }
   }
}