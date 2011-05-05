using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;
using Pundit.Core.Utils;

namespace Pundit.Core.Application
{
   public class PackageWriter : PackageStreamer
   {
      public event EventHandler<PackageFileEventArgs> BeginPackingFile;
      public event EventHandler<PackageFileEventArgs> EndPackingFile;

      private readonly string _rootDirectory;
      private readonly DevPackage _packageInfo;
      private readonly ZipOutputStream _zipStream;
      private readonly Dictionary<string, bool> _packedFiles = new Dictionary<string, bool>();
      private long _bytesWritten;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="rootDirectory">Root directory of a project (base path for all the file patterns)</param>
      /// <param name="packageInfo"></param>
      /// <param name="outputStream"></param>
      public PackageWriter(string rootDirectory, DevPackage packageInfo, Stream outputStream)
      {
         if (rootDirectory == null) throw new ArgumentNullException("rootDirectory");
         if (packageInfo == null) throw new ArgumentNullException("packageInfo");
         if (outputStream == null) throw new ArgumentNullException("outputStream");
         if (!outputStream.CanWrite) throw new ArgumentException("stream is not writable", "outputStream");

         rootDirectory = new DirectoryInfo(rootDirectory).FullName;

         packageInfo.Validate();

         _rootDirectory = rootDirectory;
         _packageInfo = packageInfo;
         _zipStream = new ZipOutputStream(outputStream);
         _zipStream.SetLevel(9);
      }

      public long WriteAll(bool includeDevTime = false)
      {
         WriteManifest(includeDevTime);

         _bytesWritten += _zipStream.Length;

         WriteFiles();

         return _bytesWritten;
      }

      private void WriteManifest(bool includeDevTime)
      {
         var entry = new ZipEntry(Package.DefaultManifestFileName);

         _zipStream.PutNextEntry(entry);

         var manifestPackage = new Package(_packageInfo, includeDevTime);

         manifestPackage.WriteTo(_zipStream);
      }

      private void WriteFiles()
      {
         foreach(SourceFiles files in _packageInfo.Files)
         {
            string[] archiveFiles, archiveDirectories;
            string searchBase;

            Resolve(files,
               _rootDirectory,   //root directory to start search from
               out searchBase,   //full path to the returned files and dirs search root
               out archiveFiles,          //full path to found files
               out archiveDirectories);   //full path to found folders

            foreach(string afile in archiveFiles)
            {
               WriteFile(files, searchBase, afile);
            }

            if(files.IncludeEmptyDirs)
            {
               foreach(string adir in archiveDirectories)
               {
                  string dirPath = GetRelativeUnixPath(files, searchBase, adir);
                  if(!dirPath.EndsWith("/")) dirPath += "/";

                  ZipEntry ed = new ZipEntry(dirPath);
                  _zipStream.PutNextEntry(ed);
               }
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

         if (!_packedFiles.ContainsKey(unixPath))
         {
            _packedFiles[unixPath] = true;

            long originalSize = new FileInfo(filePath).Length;
            _bytesWritten += originalSize;

            if(BeginPackingFile != null)
               BeginPackingFile(this, new PackageFileEventArgs(unixPath, originalSize));

            ZipEntry entry = new ZipEntry(unixPath);
            entry.DateTime = DateTime.Now;
            entry.Size = originalSize;
            _zipStream.PutNextEntry(entry);

            using (Stream fileStream = File.OpenRead(filePath))
            {
               fileStream.CopyTo(_zipStream);
            }

            if(EndPackingFile != null)
               EndPackingFile(this, new PackageFileEventArgs(unixPath, originalSize));
         }
      }

      protected override void Dispose(bool disposing)
      {
         _zipStream.Close();
         _zipStream.Dispose();
      }
   }
}
