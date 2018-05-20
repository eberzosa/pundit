using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
      
      public static string FileSizeToString(long size) => String.Format(new ByteFormatProvider(), "{0:bf}", size);

      public static string GetOSPath(string path) => path.Replace('/', Path.DirectorySeparatorChar);

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

      public static string FixPathSeparators(string s)
      {
         return s?.Replace('/', Path.DirectorySeparatorChar);
      }
   }
}
