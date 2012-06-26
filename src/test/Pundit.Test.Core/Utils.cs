using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pundit.Test
{
   static class Utils
   {
      public static string GetLog4Net1211net20Package()
      {
         return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestRepo", "packed", "log4net-1.2.11-0-net20.pundit");
      }

      public static string GetLog4Net1210net20Package()
      {
         return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestRepo", "packed", "log4net-1.2.10-0-net20.pundit");
      }

      public static string GetLog4Net12101234net20Package()
      {
         return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestRepo", "packed", "log4net-1.2.10-1234-net20.pundit");
      }

      public static string GetCasteCore30net40Package()
      {
         return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestRepo", "packed", "Castle.Core-3.0.0-2227-net40.pundit");
      }
   }
}
