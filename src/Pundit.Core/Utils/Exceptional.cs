using System;
using System.IO;

namespace Pundit.Core.Utils
{
   static class Exceptional
   {
      public static bool IsVsDocFile(string fullPath)
      {
         string s = "";

         if(File.Exists(fullPath))
         {
            Stream fs = null;

            try
            {
               fs = File.OpenRead(fullPath);
            }
            catch(Exception)
            {
               return fullPath != null && fullPath.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase);
            }

            using (var rdr = new StreamReader(fs))
            {
               while (s.Length < 1000)
               {
                  s += rdr.ReadLine();
               }
            }

            return s.Contains("<doc>") && s.Contains("<assembly>") && s.Contains("<members>");
         }

         return false;
      }
   }
}
