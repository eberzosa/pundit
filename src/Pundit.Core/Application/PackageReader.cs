using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;

namespace Pundit.Core.Application
{
   /// <summary>
   /// Reads package manifest from stream
   /// </summary>
   public class PackageReader : PackageStreamer
   {
      /// <summary>
      /// Throws when a file is about to be installed to the target folder
      /// </summary>
      public event EventHandler<ResolvedFileEventArgs> InstallingResolvedFile;

      private ZipInputStream _zipStream;

      /// <summary>
      /// Create an instance of <see cref="PackageReader"/>
      /// </summary>
      /// <param name="packageStream"></param>
      public PackageReader(Stream packageStream)
      {
         _zipStream = new ZipInputStream(packageStream);
         Manifest = ReadManifest();
      }

      /// <summary>
      /// Gets this package's manifest definition
      /// </summary>
      public Package Manifest { get; private set; }

      private Package ReadManifest()
      {
         ZipEntry entry;

         while((entry = _zipStream.GetNextEntry()) != null)
         {
            if(entry.IsFile && entry.Name == Package.DefaultManifestFileName)
            {
               Package manifest = Package.FromStreamXml(_zipStream);

               return manifest;
            }
         }

         return null;
      }

      private static PackageFileKind GetKind(ZipEntry entry)
      {
         if(entry.IsFile)
         {
            int idx = entry.Name.IndexOf("/", StringComparison.InvariantCultureIgnoreCase);

            if (idx != -1)
            {
               string folderName = entry.Name.Substring(0, idx);
               return FolderNameToFileKind.ContainsKey(folderName)
                         ? FolderNameToFileKind[folderName]
                         : PackageFileKind.Other;
            }
         }

         return PackageFileKind.Unknown;
      }

      private void InstallGenericFile(IInstallTarget installTarget, string name, PackageFileKind kind, string packageId)
      {
         string kindName = kind.ToString().ToLower();
         string shortName = name.Substring(name.IndexOf("/", StringComparison.CurrentCultureIgnoreCase) + 1);
         string fullName = kindName + "/" + packageId + "/" + shortName;

         var path = new List<string>();
         path.Add(kindName);
         path.Add(packageId);
         path.Add(shortName);

         if(InstallingResolvedFile != null)
            InstallingResolvedFile(this, new ResolvedFileEventArgs(packageId, kind, BuildConfiguration.Any, fullName));

         using(Stream df = installTarget.CreateTargetStream(path))
         {
            _zipStream.CopyTo(df);
         }
      }

      private void InstallLibrary(string packageId, IInstallTarget installTarget, string name, BuildConfiguration targetConfig, string subfolderName)
      {
         name = name.Substring(name.IndexOf("/", StringComparison.InvariantCultureIgnoreCase) + 1);

         var config = (BuildConfiguration) Enum.Parse(typeof (BuildConfiguration),
            name.Substring(0, name.IndexOf("/", StringComparison.InvariantCultureIgnoreCase)), true);

         bool install = (targetConfig == BuildConfiguration.Any) || (config == BuildConfiguration.Any) ||
                        (config == BuildConfiguration.Debug &&
                         (targetConfig == BuildConfiguration.Any || targetConfig == BuildConfiguration.Debug)) ||
                        (config == BuildConfiguration.Release &&
                         (targetConfig == BuildConfiguration.Any || targetConfig == BuildConfiguration.Release));

         if (install)
         {
            name = name.Substring(name.IndexOf("/", StringComparison.InvariantCultureIgnoreCase) + 1);

            if (InstallingResolvedFile != null)
               InstallingResolvedFile(this, new ResolvedFileEventArgs(packageId, PackageFileKind.Binary, config, name));

            var targetPath = new List<string>();
            targetPath.Add("lib");
            if (!string.IsNullOrEmpty(subfolderName)) targetPath.Add(subfolderName);
            targetPath.Add(name);

            try
            {
               using (Stream ts = installTarget.CreateTargetStream(targetPath))
               {
                  _zipStream.CopyTo(ts);
               }
            }
            catch(UnauthorizedAccessException)
            {
               //todo: think about it (can I just skip it?)
            }
         }
      }

      /// <summary>
      /// Installs the package to a specific location. If destination files exist
      /// they will be silently overwritten.
      /// </summary>
      /// <param name="installTarget"></param>
      /// <param name="originalDependency">Original dependency used as a source for metainformation while installing the package.
      /// Parameters like scope, subfolder name etc are read from it</param>
      /// <param name="configuration">Desired configuration name</param>
      public void InstallTo(IInstallTarget installTarget, PackageDependency originalDependency, BuildConfiguration configuration)
      {
         if (installTarget == null) throw new ArgumentNullException("installTarget");

         ZipEntry entry = _zipStream.GetNextEntry();
         while (entry != null)
         {
            if (entry.IsFile)
            {
               PackageFileKind kind = GetKind(entry);

               switch(kind)
               {
                  case PackageFileKind.Binary:
                     InstallLibrary(Manifest.PackageId, installTarget, entry.Name, configuration,
                                    (originalDependency != null && originalDependency.CreatePlatformFolder)
                                       ? originalDependency.Platform
                                       : null);
                     break;
                  case PackageFileKind.Include:
                  case PackageFileKind.Tools:
                  case PackageFileKind.Other:
                     InstallGenericFile(installTarget, entry.Name, kind, Manifest.PackageId);
                     break;
               }
            }

            try
            {
               entry = _zipStream.GetNextEntry();
            }
            catch(ArgumentOutOfRangeException)
            {
               //for some unknown reason this sometimes happens when enumerating after last entry, but it is safe to ignore
               entry = null;
            }
         }
      }

      protected override void Dispose(bool disposing)
      {
         if (_zipStream != null)
         {
            _zipStream.Close();
            _zipStream.Dispose();
            _zipStream = null;
         }
      }
   }
}
