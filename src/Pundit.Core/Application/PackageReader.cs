using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using log4net;
using Pundit.Core.Model;
using Pundit.Core.Utils;

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

      private static PackageFileKind GetKind(ZipEntry entry)
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

      private void InstallGenericFile(string root, string name, string kindName, string packageId)
      {
         string shortName = name.Substring(name.IndexOf("/") + 1);
         string fullName = kindName + "/" + packageId + "/" + shortName;

         if(_log.IsDebugEnabled) _log.DebugFormat("[{0}/{1}]: {2}", kindName, packageId, fullName);

         string fullPath = Path.Combine(root, fullName);
         string fullDirPath = new FileInfo(fullName).Directory.FullName;

         //_log.Debug("installing to " + fullPath);
         //_log.Debug("dir: " + fullDirPath);

         PathUtils.EnsureDirectoryExists(fullDirPath);
         if(File.Exists(fullPath)) File.Delete(fullPath);

         using(Stream df = File.Create(fullPath))
         {
            _zipStream.CopyTo(df);
         }
      }

      private void InstallLibrary(string root, string name, BuildConfiguration targetConfig)
      {
         name = name.Substring(name.IndexOf("/") + 1);
        
         BuildConfiguration config =
            (BuildConfiguration) Enum.Parse(typeof (BuildConfiguration), name.Substring(0, name.IndexOf("/")), true);

         _log.Debug("config: " + config);

         bool install = (targetConfig == BuildConfiguration.Any) || (config == BuildConfiguration.Any) ||
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

            try
            {
               using (Stream ts = File.Create(targetPath))
               {
                  _zipStream.CopyTo(ts);
               }
            }
            catch(UnauthorizedAccessException)
            {
               if(Exceptional.IsVsDocFile(targetPath))
               {
                  if(_log.IsWarnEnabled) _log.Warn("  ! couldn't overwrite documentation file, however it can be safely ignored");
               }
            }
         }
         else
         {
            _log.Debug("ignoring " + name);
         }
      }

      /// <summary>
      /// Installs the package to a specific location. If destination files exist
      /// they will be silently overwritten.
      /// </summary>
      /// <param name="rootFolder">Solution's root folder</param>
      /// <param name="configuration">Desired configuration name</param>
      public void InstallTo(string rootFolder, BuildConfiguration configuration)
      {
         Package pkg = ReadManifest();

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
                     InstallGenericFile(rootFolder, entry.Name,
                        kind.ToString().ToLower(), pkg.PackageId);
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
