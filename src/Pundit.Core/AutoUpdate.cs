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

      public static Product[] CheckUpdates()
      {
         List<Product> products = new List<Product>();

         string txt;
         
         HttpWebRequest rq =
            (HttpWebRequest) WebRequest.Create("http://pundit.codeplex.com/wikipage?title=Latest%20Version");

         HttpWebResponse rs = (HttpWebResponse) rq.GetResponse();

         using(StreamReader rdr = new StreamReader(rs.GetResponseStream()))
         {
            txt = rdr.ReadToEnd();
         }

         int ib = txt.IndexOf(BeginToken);
         int ie = txt.IndexOf(EndToken, ib);

         string vchunk = txt.Substring(ib + BeginToken.Length, ie - ib - BeginToken.Length).Trim()
            .Replace("<br />", Environment.NewLine).Trim();

         using(StringReader rdr = new StringReader(vchunk))
         {
            string line;

            while((line = rdr.ReadLine()) != null)
            {
               if(!string.IsNullOrEmpty(line))
               {
                  int idx = line.IndexOf(':');
                  int idx2 = line.IndexOf(',', idx);

                  string name = line.Substring(0, idx);
                  Version v = new Version(line.Substring(idx + 1, idx2 - idx - 1).Trim());
                  string url = line.Substring(idx2 + 1).Trim();

                  products.Add(new Product
                                  {
                                     Name = name,
                                     Version = v,
                                     DownloadUri = url
                                  });
               }
            }
         }
         
         return products.ToArray();
      }
   }
}
