using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pundit.Core;
using Pundit.Core.Model;

namespace Pundit.Test.Server.Integration
{
   [TestFixture, Ignore]
   class RestClientIntegrationTest
   {
      private IRemoteRepository _client;

      [SetUp]
      public void SetUp()
      {
         _client = RemoteRepositoryFactory.Create("http://localhost:81/repository/v1");
      }

      [Test]
      public void GetSnapshotTest()
      {
         RemoteSnapshot rs = _client.GetSnapshot(null);
      }

      [Test]
      public void PublishTest()
      {
         Stream s = File.OpenRead(Utils.GetLog4Net1210net20Package());
         _client.Publish(s);
      }

      [Test]
      public void DownloadTest()
      {
         var ms = new MemoryStream();
         using(Stream s = _client.Download("net20", "log4net", "1.2.10.0"))
         {
            s.CopyTo(ms);
         }
      }
   }
}
