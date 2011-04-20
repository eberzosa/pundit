using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pundit.Core.Utils
{
   public static class Exceptional
   {
      public static bool IsVsDocFile(string fullPath)
      {
         string s = "";

         if(File.Exists(fullPath))
         {
            using(var rdr = new StreamReader(File.OpenRead(fullPath)))
            {
               while(s.Length < 1000)
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
