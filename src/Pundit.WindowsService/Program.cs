using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Pundit.WindowsService
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      static void Main()
      {
         ServiceBase[] ServicesToRun;
         ServicesToRun = new ServiceBase[] 
			{ 
				new PunditService() 
			};
         ServiceBase.Run(ServicesToRun);
      }
   }
}
