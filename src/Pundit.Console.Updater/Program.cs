using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
            ProcessStartInfo psi = new ProcessStartInfo("upgrade.exe", "/S")
                                      {
                                         WorkingDirectory = ExeFolder
                                      };

            p = Process.Start(psi);

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
