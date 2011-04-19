using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using log4net;
using Pundit.Core.Model;

namespace Pundit.Core.Application
{
   public class PackageReader : PackageStreamer
   {
      private readonly ILog _log = LogManager.GetLogger(typeof (PackageReader));

      private ZipInputStream _zipStream;

      public PackageReader(Stream packageStream)
      {
         _zipStream = new ZipInputStream(packageStream);
      }

      public Package ReadManifest()
      {
         ZipEntry entry;

         while((entry = _zipStream.GetNextEntry()) != null)
         {
            if(entry.IsFile && entry.Name == Package.DefaultPackageFileName)
            {
               Package manifest = Package.FromStream(_zipStream);

               return manifest;
            }
         }

         return null;
      }

      private void InstallLib(string libFolder, string entryName)
      {
         _log.DebugFormat("installing lib: {0}", entryName);

         if (!Directory.Exists(libFolder)) Directory.CreateDirectory(libFolder);

         string destFile = Path.Combine(libFolder, entryName);

         using (Stream os = File.Create(destFile))
         {
            _zipStream.CopyTo(os);
         }
      }

      private PackageFileKind GetKind(ZipEntry entry)
      {
         if(entry.IsFile)
         {
            int idx = entry.Name.IndexOf("/");

            if (idx != -1)
            {
               string folderName = entry.Name.Substring(0, idx);

               if (folderName == "bin")
                  return PackageFileKind.Binary;

               if (folderName == "include")
                  return PackageFileKind.Include;

               if (folderName == "tools")
                  return PackageFileKind.Tools;

               if (folderName == "other")
                  return PackageFileKind.Other;
            }
         }

         return PackageFileKind.Unknown;
      }

      private void InstallGenericFile(string root, string name)
      {
         
      }

      private void InstallLibrary(string root, string name, BuildConfiguration targetConfig)
      {
         name = name.Substring(name.IndexOf("/") + 1);
        
         BuildConfiguration config =
            (BuildConfiguration) Enum.Parse(typeof (BuildConfiguration), name.Substring(0, name.IndexOf("/")), true);

         bool install = (targetConfig == BuildConfiguration.Any) ||
                        (config == BuildConfiguration.Debug &&
                         (targetConfig == BuildConfiguration.Any || targetConfig == BuildConfiguration.Debug)) ||
                        (config == BuildConfiguration.Release &&
                         (targetConfig == BuildConfiguration.Any || targetConfig == BuildConfiguration.Release));

         if (install)
         {
            name = name.Substring(name.IndexOf("/") + 1);

            _log.DebugFormat("[lib|{0}]: {1}", config, name);

            string targetPath = Path.Combine(root, "lib");
            targetPath = Path.Combine(targetPath, name);

            if(File.Exists(targetPath)) File.Delete(targetPath);

            using(Stream ts = File.Create(targetPath))
            {
               _zipStream.CopyTo(ts);
            }
         }
      }

      public void InstallTo(string rootFolder, BuildConfiguration configuration)
      {
         ZipEntry entry;
         while ((entry = _zipStream.GetNextEntry()) != null)
         {
            if (entry.IsFile)
            {
               PackageFileKind kind = GetKind(entry);

               switch(kind)
               {
                  case PackageFileKind.Binary:
                     InstallLibrary(rootFolder, entry.Name, configuration);
                     break;
                  case PackageFileKind.Include:
                  case PackageFileKind.Tools:
                  case PackageFileKind.Other:
                     InstallGenericFile(rootFolder, entry.Name);
                     break;
               }
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
