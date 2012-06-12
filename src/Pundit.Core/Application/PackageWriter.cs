using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Pundit.Core.Model;
using Pundit.Core.Model.EventArguments;

namespace Pundit.Core.Application
{
   /// <summary>
   /// Composes binary package from a manifest definition XML file
   /// </summary>
   public class PackageWriter : PackageStreamer
   {
      /// <summary>
      /// Occurs when a file is about to be packed into the binary
      /// </summary>
      public event EventHandler<PackageFileEventArgs> BeginPackingFile;

      /// <summary>
      /// Occurs when a file has just finished packing into the binary
      /// </summary>
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

      /// <summary>
      /// Writes all manifest data into target stream. The stream is packed into a zip package.
      /// </summary>
      /// <returns>Total size of the original unpacked data excluding the xml manifest.</returns>
      public long WriteAll()
      {
         if(_packageInfo.Files == null || _packageInfo.Files.Count == 0)
            throw new InvalidPackageException("manifest has no input files");

         WriteManifest(false);

         //_bytesWritten += _zipStream.Length;  //don't

         WriteFiles();

         _zipStream.Flush();
         return _bytesWritten;
      }

      private void WriteManifest(bool includeDevTime)
      {
         var entry = new ZipEntry(Package.DefaultManifestFileName);

         _zipStream.PutNextEntry(entry);

         var manifestPackage = new Package(_packageInfo, includeDevTime);

         manifestPackage.WriteTo(_zipStream);
      }

      class ResolvedFile
      {
         public string SearchBase;
         public string FullPath;
         public bool IsDirectory;
         public SourceFiles Info;

         public ResolvedFile(string searchBase, string fullPath, bool isDirectory, SourceFiles info)
         {
            this.SearchBase = searchBase;
            this.FullPath = fullPath;
            this.IsDirectory = isDirectory;
            this.Info = info;
         }
      }

      private List<ResolvedFile> ResolveAllFiles(IEnumerable<SourceFiles> files, string rootDirectory)
      {
         List<ResolvedFile> result = new List<ResolvedFile>();

         foreach (SourceFiles ifiles in files)
         {
            string[] archiveFiles, archiveDirectories;
            string searchBase;

            Resolve(ifiles,
               rootDirectory,             //root directory to start search from
               out searchBase,            //full path to the returned files and dirs search root
               out archiveFiles,          //full path to found files
               out archiveDirectories);   //full path to found folders

            foreach(string afile in archiveFiles)
               result.Add(new ResolvedFile(searchBase, afile, false, ifiles));

            foreach (string adir in archiveDirectories)
               result.Add(new ResolvedFile(searchBase, adir, true, ifiles));
         }

         return result;
      }

      private void WriteFiles()
      {
         List<ResolvedFile> allFiles = ResolveAllFiles(_packageInfo.Files, _rootDirectory);
         int idx = 0;
         foreach(ResolvedFile file in allFiles)
         {
            if(!file.IsDirectory)
            {
               WriteFile(file, idx++, allFiles.Count);
            }
            else if(file.Info.IncludeEmptyDirs)
            {
               string dirPath = GetRelativeUnixPath(file.Info, file.SearchBase, file.FullPath);
               if(!dirPath.EndsWith("/")) dirPath += "/";

               var ed = new ZipEntry(dirPath);
               _zipStream.PutNextEntry(ed);
            }
         }
      }

      private void WriteFile(ResolvedFile file, int fileNo, int filesCount)
      {
         string unixPath = GetRelativeUnixPath(file.Info, file.SearchBase, file.FullPath);

         if (!_packedFiles.ContainsKey(unixPath))
         {
            _packedFiles[unixPath] = true;

            long originalSize = new FileInfo(file.FullPath).Length;
            _bytesWritten += originalSize;

            if (BeginPackingFile != null)
            {
               BeginPackingFile(this, new PackageFileEventArgs(unixPath, originalSize, fileNo, filesCount));
            }

            var entry = new ZipEntry(unixPath) {DateTime = DateTime.Now, Size = originalSize};
            _zipStream.PutNextEntry(entry);

            using (Stream fileStream = File.OpenRead(file.FullPath))
            {
               fileStream.CopyTo(_zipStream);
               _zipStream.Flush();
            }

            if (EndPackingFile != null)
            {
               EndPackingFile(this, new PackageFileEventArgs(unixPath, originalSize, fileNo, filesCount));
            }
         }
      }

      protected override void Dispose(bool disposing)
      {
         _zipStream.Flush();
         _zipStream.Close();
         _zipStream.Dispose();
      }
   }
}
