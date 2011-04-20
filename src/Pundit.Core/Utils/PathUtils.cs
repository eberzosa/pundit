using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pundit.Core.Utils
{
   //todo: make internal
   public static class PathUtils
   {
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
         return String.Format(new FileSizeFormatProvider(), "{0:fs}", size);
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
   }
}
