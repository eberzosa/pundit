using System;
using System.Collections.Generic;
using System.IO;
using EBerzosa.Utils;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;

namespace EBerzosa.Pundit.Core.Package
{
   public abstract class PackageWriter : PackageStreamer, IPackageWriter
   {
      protected readonly PackageSpec PackageSpec;
      protected readonly string RootDirectory;
      
      private readonly Dictionary<string, bool> _packedFiles = new Dictionary<string, bool>();
      private long _bytesWritten;


      public Action<PackageFileEventArgs> OnBeginPackingFile { get; set; }

      public Action<PackageFileEventArgs> OnEndPackingFile { get; set; }
      

      /// <summary>
      /// 
      /// </summary>
      /// <param name="packageSerializer"></param>
      /// <param name="rootDirectory">Root directory of a project (base path for all the file patterns)</param>
      /// <param name="packageSpec"></param>
      protected PackageWriter(string rootDirectory, PackageSpec packageSpec)
      {
         Guard.NotNull(rootDirectory, nameof(rootDirectory));
         Guard.NotNull(packageSpec, nameof(packageSpec));

         rootDirectory = new DirectoryInfo(rootDirectory).FullName;

         packageSpec.Validate();
         
         RootDirectory = rootDirectory;
         PackageSpec = packageSpec;
      }

      public long WriteAll()
      {
         WriteManifest();
         WriteFiles();

         return _bytesWritten;
      }

      protected abstract void WriteManifest();

      protected abstract void WriteEmptyDirectory(string path);

      private void WriteFiles()
      {
         foreach(SourceFiles files in PackageSpec.Files)
         {
            Resolve(files,
               RootDirectory,   //root directory to start search from
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

               WriteEmptyDirectory(dirPath);
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

         WriteFile(filePath, originalSize, unixPath);

         OnEndPackingFile?.Invoke(new PackageFileEventArgs(unixPath, originalSize));
      }

      protected abstract void WriteFile(string filePath, long size, string relativePath);
   }
}
