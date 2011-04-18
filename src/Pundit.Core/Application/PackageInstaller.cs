using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Xml.Serialization;
using log4net;
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
         _installed[pck] = true;
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
               InstalledPackagesIndex r = (InstalledPackagesIndex) xml.Deserialize(s);

               s.Close();

               return r;
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

   public class PackageInstaller
   {
      private static readonly ILog Log = LogManager.GetLogger(typeof (PackageInstaller));

      private readonly string _rootDirectory;
      private readonly VersionResolutionTable _versionTable;
      private readonly IRepository _localRepository;
      private InstalledPackagesIndex _index;

      private readonly string _libFolderPath;
      private readonly string _includeFolderPath;
      private readonly string _toolsFolderPath;
      private readonly string _otherFolderPath;

      public PackageInstaller(string rootDirectory, VersionResolutionTable versionTable,
         IRepository localRepository)
      {
         if (rootDirectory == null) throw new ArgumentNullException("rootDirectory");
         if (versionTable == null) throw new ArgumentNullException("versionTable");
         if (localRepository == null) throw new ArgumentNullException("localRepository");

         _rootDirectory = rootDirectory;
         _versionTable = versionTable;
         _localRepository = localRepository;

         _libFolderPath = Path.Combine(rootDirectory, "lib");
         _includeFolderPath = Path.Combine(rootDirectory, "include");
         _toolsFolderPath = Path.Combine(rootDirectory, "tools");
         _otherFolderPath = Path.Combine(rootDirectory, "other");
      }

      private void ClearAllFolders()
      {
         if (Directory.Exists(_libFolderPath))
            foreach (string file in Directory.GetFiles(_libFolderPath))
               File.Delete(file);

         if(Directory.Exists(_includeFolderPath))
            Directory.Delete(_includeFolderPath, true);

         if(Directory.Exists(_toolsFolderPath))
            Directory.Delete(_toolsFolderPath, true);

         if(Directory.Exists(_otherFolderPath))
            
            Directory.Delete(_otherFolderPath, true);
      }

      private bool DependenciesChanged(IEnumerable<PackageKey> currentDependencies, BuildConfiguration configuration)
      {
         if (configuration != _index.Configuration)
            return true;

         if (currentDependencies.Count() != _index.TotalPackagesCount)
            return true;

         return currentDependencies.Any(pck => !_index.IsInstalled(pck));
      }

      private void ResetFiles(BuildConfiguration configuration)
      {
         ClearAllFolders();

         _index = new InstalledPackagesIndex {Configuration = configuration};
      }

      private void Install(PackageKey pck, BuildConfiguration configuration)
      {
         if(!_index.IsInstalled(pck))
         {
            using (Stream s = _localRepository.Download(pck))
            {
               using (PackageReader reader = new PackageReader(s))
               {
                  Log.InfoFormat("installing: {0}...", pck);

                  reader.InstallTo(_rootDirectory, configuration);
               }
            }

            _index.Install(pck);
         }
         else
         {
            Log.InfoFormat("already installed: {0}", pck);
         }
      }

      public void InstallAll(BuildConfiguration configuration)
      {
         _index = InstalledPackagesIndex.ReadFromFolder(_rootDirectory);
         IEnumerable<PackageKey> currentDependencies = _versionTable.GetPackages();

         if (DependenciesChanged(currentDependencies, configuration))
         {
            ResetFiles(configuration);

            foreach (PackageKey pck in currentDependencies)
            {
               Install(pck, configuration);
            }

            _index.WriteToFolder(_rootDirectory);
         }
         else
         {
            Log.Info("no dependencies changed since the last time");
         }
      }
   }
}
