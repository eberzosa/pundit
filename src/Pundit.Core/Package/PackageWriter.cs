using System;
using System.Collections.Generic;
using System.IO;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Serializers;
using EBerzosa.Utils;
using ICSharpCode.SharpZipLib.Zip;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;

namespace EBerzosa.Pundit.Core.Package
{
   public class PackageWriter : PackageStreamer
   {
      private readonly PackageSerializerFactory _packageSerializerFactory;

      private readonly string _rootDirectory;
      private readonly PackageSpec _packageSpecInfo;
      private readonly ZipOutputStream _zipStream;
      private readonly Dictionary<string, bool> _packedFiles = new Dictionary<string, bool>();
      private long _bytesWritten;


      public Action<PackageFileEventArgs> OnBeginPackingFile { get; set; }

      public Action<PackageFileEventArgs> OnEndPackingFile { get; set; }
      

      /// <summary>
      /// 
      /// </summary>
      /// <param name="packageSerializerFactory"></param>
      /// <param name="rootDirectory">Root directory of a project (base path for all the file patterns)</param>
      /// <param name="packageSpecInfo"></param>
      /// <param name="outputStream"></param>
      public PackageWriter(PackageSerializerFactory packageSerializerFactory, string rootDirectory, PackageSpec packageSpecInfo, Stream outputStream)
      {
         Guard.NotNull(packageSerializerFactory, nameof(packageSerializerFactory));
         Guard.NotNull(rootDirectory, nameof(rootDirectory));
         Guard.NotNull(packageSpecInfo, nameof(packageSpecInfo));
         Guard.NotNull(outputStream, nameof(outputStream));

         rootDirectory = new DirectoryInfo(rootDirectory).FullName;

         packageSpecInfo.Validate();

         _packageSerializerFactory = packageSerializerFactory;
         _rootDirectory = rootDirectory;
         _packageSpecInfo = packageSpecInfo;
         _zipStream = new ZipOutputStream(outputStream);
         _zipStream.SetLevel(9);
      }

      public long WriteAll()
      {
         if(_packageSpecInfo.Files == null || _packageSpecInfo.Files.Count == 0)
            throw new InvalidPackageException("manifest has no input files");

         WriteManifest();

         _bytesWritten += _zipStream.Length;

         WriteFiles();

         return _bytesWritten;
      }

      private void WriteManifest()
      {
         var entry = new ZipEntry(PackageManifest.DefaultManifestFileName);

         _zipStream.PutNextEntry(entry);

         _packageSpecInfo.Validate();
         _packageSerializerFactory.GetPundit().SerializePackageManifest(_packageSpecInfo, _zipStream);
      }

      private void WriteFiles()
      {
         foreach(SourceFiles files in _packageSpecInfo.Files)
         {
            Resolve(files,
               _rootDirectory,   //root directory to start search from
               out var searchBase,   //full path to the returned files and dirs search root
               out var archiveFiles,          //full path to found files
               out var archiveDirectories);   //full path to found folders

            foreach (string afile in archiveFiles)
               WriteFile(files, searchBase, afile);

            if (!files.IncludeEmptyDirs)
               continue;

            foreach(var adir in archiveDirectories)
            {
               var dirPath = GetRelativeUnixPath(files, searchBase, adir);
               if(!dirPath.EndsWith("/")) dirPath += "/";

               _zipStream.PutNextEntry(new ZipEntry(dirPath));
            }
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="info"></param>
      /// <param name="baseDir">file base dir (full path)</param>
      /// <param name="filePath">file to archive (full path)</param>
      private void WriteFile(SourceFiles info, string baseDir, string filePath)
      {
         string unixPath = GetRelativeUnixPath(info, baseDir, filePath);

         if (_packedFiles.ContainsKey(unixPath))
            return;

         _packedFiles[unixPath] = true;

         var originalSize = new FileInfo(filePath).Length;
         _bytesWritten += originalSize;

         OnBeginPackingFile?.Invoke(new PackageFileEventArgs(unixPath, originalSize));

         var entry = new ZipEntry(unixPath)
         {
            DateTime = DateTime.Now,
            Size = originalSize
         };
         _zipStream.PutNextEntry(entry);

         using (Stream fileStream = File.OpenRead(filePath))
            fileStream.CopyTo(_zipStream);

         OnEndPackingFile?.Invoke(new PackageFileEventArgs(unixPath, originalSize));
      }

      protected override void Dispose(bool disposing)
      {
         _zipStream.Close();
         _zipStream.Dispose();
      }
   }
}
