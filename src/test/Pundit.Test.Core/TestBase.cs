using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Pundit.Test
{
   class TestBase
   {
      public TestBase()
      {
         ServicePointManager.ServerCertificateValidationCallback = ServerCertificateValidationCallback;
      }

      private bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
      {
         return true;
      }
   }
}
