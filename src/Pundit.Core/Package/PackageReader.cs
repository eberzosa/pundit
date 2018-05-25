using System;
using System.IO;
using EBerzosa.Pundit.Core.Model.Package;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;
using Pundit.Core.Utils;

namespace EBerzosa.Pundit.Core.Package
{
   public abstract class PackageReader : PackageStreamer, IPackageReader
   {
      public event EventHandler<ResolvedFileEventArgs> InstallingResolvedFile;


      public abstract PackageManifest ReadManifest();

      public abstract void InstallTo(string rootFolder, PackageDependency originalDependency, BuildConfiguration configuration);

      public abstract void ExtractTo(string rootFolder);

      protected void InstallToInternal(string rootFolder, string packageId, BuildConfiguration configuration, string filePath)
      {
         PackageFileKind kind = GetKind(filePath);

         switch (kind)
         {
            case PackageFileKind.Binary:
               InstallLibrary(packageId, rootFolder, filePath, configuration);
               break;

            case PackageFileKind.Include:
            case PackageFileKind.Tools:
            case PackageFileKind.Other:
               InstallGenericFile(rootFolder, filePath, kind, packageId);
               break;
         }
      }

      protected void InstallLibrary(string packageId, string root, string fileToInstall, BuildConfiguration targetConfig)
      {
         var name = fileToInstall.Substring(fileToInstall.IndexOf("/") + 1);

         BuildConfiguration config =
            (BuildConfiguration)Enum.Parse(typeof(BuildConfiguration), name.Substring(0, name.IndexOf("/")), true);

         //var name = fileToInstall.Substring(fileToInstall.IndexOf("/") + 1);

         //var posFirstSlash = name.IndexOf("/");

         //if (posFirstSlash < 0 || !Enum.TryParse(name.Substring(0, name.IndexOf("/")), out BuildConfiguration config))
         //   config = BuildConfiguration.Any;

         bool install = (targetConfig == BuildConfiguration.Any) || (config == BuildConfiguration.Any) ||
                        (config == BuildConfiguration.Debug &&
                         (targetConfig == BuildConfiguration.Any || targetConfig == BuildConfiguration.Debug)) ||
                        (config == BuildConfiguration.Release &&
                         (targetConfig == BuildConfiguration.Any || targetConfig == BuildConfiguration.Release));

         if (install)
         {
            name = name.Substring(name.IndexOf("/") + 1);

            InstallingResolvedFile?.Invoke(null, new ResolvedFileEventArgs(packageId, PackageFileKind.Binary, config, name));

            string targetPath = Path.Combine(root, "lib", name);
            
            try
            {
               var directoryName = Path.GetDirectoryName(targetPath);

               if (!Directory.Exists(directoryName))
                  Directory.CreateDirectory(directoryName);

               else if (File.Exists(targetPath))
                  File.Delete(targetPath);

               using (Stream ts = File.Create(targetPath))
                  GetSourceStream(fileToInstall).CopyTo(ts);
            }
            catch (UnauthorizedAccessException)
            {
               if (Exceptional.IsVsDocFile(targetPath))
               {
               }
               else
               {
                  throw;
               }
            }
         }
         else
         {
            //_log.Debug("ignoring " + name);
         }
      }

      protected void InstallGenericFile(string root, string fileToInstall, PackageFileKind kind, string packageId)
      {
         string kindName = kind.ToString().ToLower();
         string relativeKindRoot = Path.Combine(root, kindName);
         if (!Directory.Exists(relativeKindRoot)) Directory.CreateDirectory(relativeKindRoot);

         string shortName = fileToInstall.Substring(fileToInstall.IndexOf("/") + 1);
         string fullName = kindName + "/" + packageId + "/" + shortName;

         InstallingResolvedFile?.Invoke(this, new ResolvedFileEventArgs(packageId, kind, BuildConfiguration.Any, fullName));

         string fullPath = PathUtils.FixPathSeparators(Path.Combine(root, fullName));
         string fullDirPath = new FileInfo(fullPath).Directory.FullName;

         PathUtils.EnsureDirectoryExists(fullDirPath);
         if (File.Exists(fullPath)) File.Delete(fullPath);

         using (Stream df = File.Create(fullPath))
            GetSourceStream(fileToInstall).CopyTo(df);
      }

      protected void ExtractFileTo(string rootFolder, string fileToExtract, bool isFile)
      {
         if (fileToExtract == PackageManifest.DefaultManifestFileName)
            return;

         var dstFile = Path.Combine(rootFolder, fileToExtract);
         var dstDir = isFile ? Path.GetDirectoryName(dstFile) : dstFile;

         if (!Directory.Exists(dstDir))
            Directory.CreateDirectory(dstDir);

         if (!isFile)
            return;

         using (var dstStream = File.Create(dstFile))
            GetSourceStream(fileToExtract).CopyTo(dstStream);
      }
      

      protected abstract Stream GetSourceStream(string fileToInstall);


      public static PackageFileKind GetKind(string filePath)
      {
         int idx = filePath.IndexOf("/");

         if (idx != -1)
         {
            string folderName = filePath.Substring(0, idx);

            if (folderName == "bin")
               return PackageFileKind.Binary;

            if (folderName == "include")
               return PackageFileKind.Include;

            if (folderName == "tools")
               return PackageFileKind.Tools;

            if (folderName == "other")
               return PackageFileKind.Other;
         }

         return PackageFileKind.Unknown;
      }
   }
}