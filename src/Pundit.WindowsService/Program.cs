using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using log4net.Config;

namespace Pundit.WindowsService
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      static void Main(string[] args)
      {
         XmlConfigurator.Configure();

         if(args != null && args.Length > 0 && args[0] == "console")
            RunAsConsole();
         else
            RunAsService();
      }

      static void RunAsService()
      {
         ServiceBase.Run(new[] {new PunditService()});
      }

      static void RunAsConsole()
      {
         //AttachConsole(ATTACH_PARENT_PROCESS);

         Console.WriteLine("creating standalone host...");
         using(StandaloneHost host = new StandaloneHost())
         {
            Console.WriteLine("starting...");
            host.Run();
            Console.WriteLine("started; press any key to shutdown");
            Console.ReadLine();
            Console.WriteLine("shutting down...");
         }
         Console.WriteLine("bye");
      }

      const uint ATTACH_PARENT_PROCESS = 0x0ffffffff;  // default value if not specifing a process ID

      [DllImport("kernel32.dll", SetLastError = true)]
      static extern bool AttachConsole(uint dwProcessId);
   }
}
