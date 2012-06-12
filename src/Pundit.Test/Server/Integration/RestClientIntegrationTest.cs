using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pundit.Core;
using Pundit.Core.Model;

namespace Pundit.Test.Server.Integration
{
   [TestFixture]
   class RestClientIntegrationTest
   {
      private IRemoteRepository _client;

      [SetUp]
      public void SetUp()
      {
         _client = RemoteRepositoryFactory.Create("http://localhost:81/repository/v1");
      }

      [Test]
      public void PublishTest()
      {
         RemoteSnapshot rs = _client.GetSnapshot(null);
      }
   }
}
