using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pundit.Core.Application.Repositories;
using Pundit.Core.Model;

namespace Pundit.Test.Repositories
{
   [TestFixture]
   class NugetRepositoryTest : TestBase
   {
      private NugetRepository _nuget;

      [SetUp]
      public void SetUp()
      {
         _nuget = new NugetRepository();
      }

      [Test]
      public void Snapshot_Get_ReturnsAll()
      {
         RemoteSnapshot snapshot = _nuget.GetSnapshot(null);

         Assert.IsNotNull(snapshot);
      }
   }
}
