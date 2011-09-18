using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Pundit.Core
{
   public class AutoUpdate
   {
      public class Product
      {
         public string Name { get; set; }

         public Version Version { get; set; }

         public string DownloadUri { get; set; }
      }

      //http://pundit.codeplex.com/wikipage?title=Latest%20Version

      private const string BeginToken = "version begin";
      private const string EndToken = "version end";

      public static Product CheckUpdates()
      {
         WebClient wc = new WebClient();
         string latest = wc.DownloadString("http://dl.dropbox.com/u/228208/pundit/latest.txt");

         return new Product
                   {
                      DownloadUri = "http://dl.dropbox.com/u/228208/pundit/latest.exe",
                      Name = "Console Tools",
                      Version = new Version(latest)
                   };
      }

      public static string Download(Product p, string targetFolder)
      {
         string path = Path.Combine(targetFolder, "upgrade.exe");

         WebClient wc = new WebClient();
         wc.DownloadFile(p.DownloadUri, path);

         return path;
      }
   }
}
