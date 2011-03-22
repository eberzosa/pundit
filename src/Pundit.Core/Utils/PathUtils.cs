using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pundit.Core.Utils
{
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
   }
}
