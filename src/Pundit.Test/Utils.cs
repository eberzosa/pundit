using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pundit.Test
{
   static class Utils
   {
      public static string GetAnyPacked()
      {
         return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestRepo", "packed", "log4net-1.2.11-0-net20.pundit");
      }
   }
}
