using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Pundit.Vsix
{
   public partial class PunditPackage : Package
   {
      private Timer _pingUpdatesTimer;

      private void StartBackgroundActivity()
      {
         if (_pingUpdatesTimer == null)
         {
            _pingUpdatesTimer = new Timer(PingUpdatesTimer, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
         }
      }

      private void StopBackgroundActivity()
      {
         if(_pingUpdatesTimer != null)
         {
            _pingUpdatesTimer.Dispose();
            _pingUpdatesTimer = null;
         }
      }

      private void PingUpdatesTimer(object state)
      {
         try
         {
            IVsStatusbar statusbar = GetService(typeof (SVsStatusbar)) as IVsStatusbar;

            if(statusbar != null)
            {
               int frozen;
               statusbar.IsFrozen(out frozen);
               bool isFrozen = Convert.ToBoolean(frozen);

               if(!isFrozen)
               {
                  statusbar.SetText("checking for Pundit updates... (" + DateTime.Now + ")");
               }
            }
         }
         catch
         {
            
         }
      }
      
   }
}
