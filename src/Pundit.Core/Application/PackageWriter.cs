using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using log4net;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Application
{
   public class PackageWriter : IDisposable
   {
      private static readonly ILog Log = LogManager.GetLogger(typeof (PackageWriter));
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

      public long WriteAll()
      {
         WriteManifest();

         _bytesWritten += _zipStream.Length;

         WriteFiles();

         return _bytesWritten;
      }

      private void WriteManifest()
      {
         ZipEntry entry = new ZipEntry(Package.DefaultPackageFileName);

         _zipStream.PutNextEntry(entry);

         Package manifestPackage = new Package(_packageInfo);

         manifestPackage.WriteTo(_zipStream);
      }

      private void WriteFiles()
      {
         foreach(SourceFiles files in _packageInfo.Files)
         {
            string[] archiveFiles, archiveDirectories;
            string searchBase;

            files.Resolve(_rootDirectory, out searchBase, out archiveFiles, out archiveDirectories);

            foreach(string afile in archiveFiles)
            {
               WriteFile(files, searchBase, afile);
            }

            if(files.IncludeEmptyDirs)
            {
               foreach(string adir in archiveDirectories)
               {
                  string dirPath = files.GetRelativeUnixPath(searchBase, adir);
                  dirPath += "/";

                  ZipEntry ed = new ZipEntry(dirPath);
                  _zipStream.PutNextEntry(ed);
               }
            }
         }
      }

      private void WriteFile(SourceFiles info, string baseDir, string filePath)
      {
         string unixPath = info.GetRelativeUnixPath(baseDir, filePath);

         if (!_packedFiles.ContainsKey(unixPath))
         {
            _packedFiles[unixPath] = true;

            long originalSize = new FileInfo(filePath).Length;
            _bytesWritten += originalSize;

            Log.Debug(string.Format("packaging {0} ({1})... ", unixPath,
                          String.Format(new FileSizeFormatProvider(), "{0:fs}", originalSize)));

            ZipEntry entry = new ZipEntry(unixPath);
            _zipStream.PutNextEntry(entry);

            using (Stream fileStream = File.OpenRead(filePath))
            {
               fileStream.CopyTo(_zipStream);
            }
         }
      }

      public void Dispose()
      {
         _zipStream.Close();
         _zipStream.Dispose();
      }
   }
}
