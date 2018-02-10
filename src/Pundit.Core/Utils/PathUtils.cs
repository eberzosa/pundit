using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using EBerzosa.Pundit.Core.Utils;
using NAnt.Core;

namespace Pundit.Core.Utils
{
   //todo: make internal
   public static class PathUtils
   {
      public static readonly List<string> DefaultExcludesList = new List<string>
                                                                {
                                                                   "**/CVS",
                                                                   "**/CVS/**",
                                                                   "**/.svn",
                                                                   "**/.svn/**",
                                                                   "**/_svn",
                                                                   "**/_svn/**",
                                                                   "**/.cvsignore",
                                                                   "**/SCCS",
                                                                   "**/SCCS/**",
                                                                   "**/vssver.scc",
                                                                   "**/vssver2.scc",
                                                                   "**/_vti_cnf/**"
                                                                };

      public static string GetUnixPath(string path)
      {
         while (path.StartsWith("" + Path.DirectorySeparatorChar)) path = path.Substring(1);

         path = path.Replace(Path.DirectorySeparatorChar, '/');

         return path;
      }

      public static string GetRelativePath(string baseDir, string fullFilePath)
      {
         string path;

         if(fullFilePath.StartsWith(baseDir, StringComparison.CurrentCultureIgnoreCase))
         {
            path = fullFilePath.Substring(baseDir.Length);
         }
         else
         {
            path = Path.GetFileName(fullFilePath);
         }

         return path;
      }

      public static string FileSizeToString(long size)
      {
         size.ToString();
         return String.Format(new ByteFormatProvider(), "{0:fs}", size);
      }

      public static string GetOSPath(string path)
      {
         return path.Replace('/', Path.DirectorySeparatorChar);
      }

      /// <summary>
      /// Ensures that the directory exists. Otherwise creates the directory (including all levels)
      /// </summary>
      /// <param name="fullPath"></param>
      public static void EnsureDirectoryExists(string fullPath)
      {
         if (string.IsNullOrEmpty(fullPath))
            throw new ArgumentNullException("fullPath");

         fullPath = fullPath.Replace('/', Path.DirectorySeparatorChar);
         string diskName = fullPath.Substring(0, 3); //dumb, will return "X:\")
         string[] parts = fullPath.Substring(3).Split(Path.DirectorySeparatorChar);
         string currentPath = (diskName[diskName.Length - 1] == Path.DirectorySeparatorChar) ?
             diskName : (diskName + Path.DirectorySeparatorChar);

         foreach (string part in parts)
         {

            currentPath = Path.Combine(currentPath, part);

            if (!Directory.Exists(currentPath))
               Directory.CreateDirectory(currentPath);
         }
      }

      private static string[] ParsePatternArray(string array)
      {
         return string.IsNullOrEmpty(array)
                   ? new string[0]
                   : array.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
      }


      public static IEnumerable<FileInfo> SearchFiles(string baseDirectory, string includePattern, string excludePattern)
      {
         var scanner = new DirectoryScanner(false);
         scanner.BaseDirectory = new DirectoryInfo(baseDirectory);
         scanner.Excludes.AddRange(DefaultExcludesList.ToArray());

         if (!string.IsNullOrEmpty(excludePattern)) scanner.Excludes.AddRange(ParsePatternArray(excludePattern));
         if (!string.IsNullOrEmpty(includePattern)) scanner.Includes.AddRange(ParsePatternArray(includePattern));

         scanner.Scan();

         var files = new string[scanner.FileNames.Count];
         scanner.FileNames.CopyTo(files, 0);

         return files.Select(f => new FileInfo(f));
      }

      public static string FixPathSeparators(string s)
      {
         return s == null ? null : s.Replace('/', Path.DirectorySeparatorChar);
      }

      public static string ExeFolder
      {
         get
         {
            Assembly asm = Assembly.GetExecutingAssembly();

            return (asm == null || asm.Location == null ? null : Path.GetDirectoryName(asm.Location))
               ?? Environment.CurrentDirectory
               ?? AppDomain.CurrentDomain.BaseDirectory;
         }
      }

   }
}
