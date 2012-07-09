using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pundit.Core.Server.Application;
using Pundit.Core.Server.Model;

namespace Pundit.Test.Server.Repositories
{
   [TestFixture]
   public class ConfigurationRepositoryTest : RepositoryTestBase
   {
      private IConfigurationRepository _repo = new MySqlConfigurationRepository(ConnectionString);

      [Test]
      public void IncrementCounterTest()
      {
         long start = _repo.GetCounterValue("c1");

         long v = _repo.IncrementCounter("c1");
         Assert.AreEqual(start + 1, v);

         long v0 = _repo.GetCounterValue("c1");
         Assert.AreEqual(start + 1, v0);

         long v1 = _repo.IncrementCounter("c1");
         Assert.AreEqual(start + 2, v1);
      }
   }
}
