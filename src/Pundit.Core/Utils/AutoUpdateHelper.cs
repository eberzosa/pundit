using System;
using System.Net;

namespace Pundit.Core.Utils
{
   public static class AutoUpdateHelper
   {
      public static Version CheckLatestVersion()
      {
         try
         {
            string vs = new WebClient().DownloadString(Globals.GetLatestVersionNumberUri);
            Version v;
            if (Version.TryParse(vs, out v)) return v;
         }
         catch(Exception ex)
         {
            //todo:
         }

         return null;
      }
   }
}
