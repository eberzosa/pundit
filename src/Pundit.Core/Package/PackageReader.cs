﻿using System;
using System.IO;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Serializers;
using EBerzosa.Utils;
using ICSharpCode.SharpZipLib.Zip;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;
using Pundit.Core.Utils;

namespace EBerzosa.Pundit.Core.Package
{
   public class PackageReader : PackageStreamer, IPackageReader
   {
      private readonly PackageSerializerFactory _packageSerializer;
      public event EventHandler<ResolvedFileEventArgs> InstallingResolvedFile;

      private ZipInputStream _zipStream;

      public PackageReader(PackageSerializerFactory packageSerializerFactory, Stream packageStream)
      {
         Guard.NotNull(packageSerializerFactory, nameof(packageSerializerFactory));
         Guard.NotNull(packageStream, nameof(packageStream));

         _packageSerializer = packageSerializerFactory;
         _zipStream = new ZipInputStream(packageStream);
      }

      public PackageManifest ReadManifest()
      {
         ZipEntry entry;

         while((entry = _zipStream.GetNextEntry()) != null)
         {
            if(entry.IsFile && entry.Name == PackageManifest.DefaultManifestFileName)
            {
               return _packageSerializer.GetPundit().DeserializePackageManifest(_zipStream);
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

      private void InstallGenericFile(string root, string name, PackageFileKind kind, string packageId)
      {
         string kindName = kind.ToString().ToLower();
         string relativeKindRoot = Path.Combine(root, kindName);
         if (!Directory.Exists(relativeKindRoot)) Directory.CreateDirectory(relativeKindRoot);

         string shortName = name.Substring(name.IndexOf("/") + 1);
         string fullName = kindName + "/" + packageId + "/" + shortName;

         if(InstallingResolvedFile != null)
            InstallingResolvedFile(this, new ResolvedFileEventArgs(packageId, kind, BuildConfiguration.Any, fullName));

         string fullPath = PathUtils.FixPathSeparators(Path.Combine(root, fullName));
         string fullDirPath = new FileInfo(fullPath).Directory.FullName;

         PathUtils.EnsureDirectoryExists(fullDirPath);
         if(File.Exists(fullPath)) File.Delete(fullPath);

         using(Stream df = File.Create(fullPath))
         {
            _zipStream.CopyTo(df);
         }
      }

      private void InstallLibrary(string packageId, string root, string name, BuildConfiguration targetConfig)
      {
         name = name.Substring(name.IndexOf("/") + 1);

         BuildConfiguration config =
            (BuildConfiguration)Enum.Parse(typeof(BuildConfiguration), name.Substring(0, name.IndexOf("/")), true);

         bool install = (targetConfig == BuildConfiguration.Any) || (config == BuildConfiguration.Any) ||
                        (config == BuildConfiguration.Debug &&
                         (targetConfig == BuildConfiguration.Any || targetConfig == BuildConfiguration.Debug)) ||
                        (config == BuildConfiguration.Release &&
                         (targetConfig == BuildConfiguration.Any || targetConfig == BuildConfiguration.Release));

         if (install)
         {
            name = name.Substring(name.IndexOf("/") + 1);

            InstallingResolvedFile?.Invoke(null, new ResolvedFileEventArgs(packageId, PackageFileKind.Binary, config, name));

            string targetPath = Path.Combine(root, "lib");
            if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);

            targetPath = Path.Combine(targetPath, name);

            try
            {

               if (File.Exists(targetPath)) File.Delete(targetPath);

               using (Stream ts = File.Create(targetPath))
               {
                  _zipStream.CopyTo(ts);
               }
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

      /// <summary>
      /// Installs the package to a specific location. If destination files exist
      /// they will be silently overwritten.
      /// </summary>
      /// <param name="rootFolder">Solution's root folder</param>
      /// <param name="originalDependency"></param>
      /// <param name="configuration">Desired configuration name</param>
      public void InstallTo(string rootFolder, PackageDependency originalDependency, BuildConfiguration configuration)
      {
         PackageManifest pkg = ReadManifest();

         ZipEntry entry;
         while ((entry = _zipStream.GetNextEntry()) != null)
         {
            if (entry.IsFile)
            {
               PackageFileKind kind = GetKind(entry);

               switch(kind)
               {
                  case PackageFileKind.Binary:
                     InstallLibrary(pkg.PackageId, rootFolder, entry.Name, configuration);
                     break;

                  case PackageFileKind.Include:
                  case PackageFileKind.Tools:
                  case PackageFileKind.Other:
                     InstallGenericFile(rootFolder, entry.Name, kind, pkg.PackageId);
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
