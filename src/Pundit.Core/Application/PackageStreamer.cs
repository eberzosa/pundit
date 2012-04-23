using System;
using System.IO;
using NAnt.Core;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Application
{
   /// <summary>
   /// Streamer base
   /// </summary>
   public abstract class PackageStreamer : IDisposable
   {
      protected virtual void Dispose(bool disposing)
      {
         
      }

      private static string[] ParsePatternArray(string array)
      {
         return string.IsNullOrEmpty(array)
                   ? new string[0]
                   : array.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
      }

      protected void Resolve(SourceFiles sourceFiles, string manifestDir,
         out string searchBase,
         out string[] files,
         out string[] directories)
      {
         //1. resolve manifestDir to searchBase
         if (!Directory.Exists(manifestDir))
            throw new DirectoryNotFoundException("directory [" + manifestDir + "] not found");

         manifestDir = new DirectoryInfo(manifestDir).FullName;

         if (!string.IsNullOrEmpty(sourceFiles.BaseDirectory))
            manifestDir = new DirectoryInfo(
               Path.Combine(manifestDir, sourceFiles.BaseDirectory)).FullName;

         if (!Directory.Exists(manifestDir))
            throw new DirectoryNotFoundException("search base directory [" + manifestDir + "] not found");

         searchBase = manifestDir;

         //2. scan for files and directories
         //using NAnt.Core for now, will continue working on FileSet later
         var scanner = new DirectoryScanner(false);
         scanner.BaseDirectory = new DirectoryInfo(manifestDir);
         scanner.Excludes.AddRange(FileSet.DefaultExcludesList.ToArray());
         scanner.Excludes.AddRange(ParsePatternArray(sourceFiles.Exclude));
         scanner.Includes.AddRange(ParsePatternArray(sourceFiles.Include));

         scanner.Scan();

         files = new string[scanner.FileNames.Count];
         directories = new string[scanner.DirectoryNames.Count];

         scanner.FileNames.CopyTo(files, 0);
         scanner.DirectoryNames.CopyTo(directories, 0);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceFiles"></param>
      /// <param name="baseDir">full path to base dir</param>
      /// <param name="path">full path to file</param>
      /// <returns></returns>
      protected static string GetRelativeUnixPath(SourceFiles sourceFiles, string baseDir, string path)
      {
         if (!sourceFiles.Flatten)
         {
            path = path.Substring(baseDir.Length);

            path = PathUtils.GetUnixPath(path);
         }
         else
         {
            path = new FileInfo(path).Name;
         }

         switch (sourceFiles.FileKind)
         {
            case PackageFileKind.Binary:
               path = sourceFiles.Configuration.ToString().ToLower() + "/" + path;
               break;
            default:
               if (!string.IsNullOrEmpty(sourceFiles.TargetDirectory))
                  path = sourceFiles.TargetDirectory + "/" + path;
               break;
         }

         switch (sourceFiles.FileKind)
         {
            case PackageFileKind.Binary:
               path = "bin/" + path;
               break;
            case PackageFileKind.Include:
               path = "include/" + path;
               break;
            case PackageFileKind.Tools:
               path = "tools/" + path;
               break;
            case PackageFileKind.Other:
               path = "other/" + path;
               break;
         }

         return path;
      }


      public void Dispose()
      {
         Dispose(true);
      }
   }
}
