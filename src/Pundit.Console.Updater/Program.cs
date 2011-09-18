using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Pundit.Console.Updater
{
   class Program
   {
      static int Main(string[] args)
      {
         Thread.Sleep(1000);  //give some time to shutdown

         Process p = null;

         try
         {
            p = Process.Start("upgrade.exe", "/S");

            p.WaitForExit();

            System.Console.WriteLine("upgraded.");
         }
         catch(Exception ex)
         {
            System.Console.WriteLine(ex.Message + ex.StackTrace);

            return 1;
         }

         return 0;
      }
   }
}
