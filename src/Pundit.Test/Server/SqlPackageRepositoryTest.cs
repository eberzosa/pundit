using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pundit.Core.Model;
using Pundit.Core.Server.Application;
using Pundit.Core.Server.Model;

namespace Pundit.Test.Server
{
   [TestFixture]
   public class SqlPackageRepositoryTest
   {
      private const string TestConnectionString = "Server=localhost;Database=pundit;Uid=pundit;Pwd=nopassword";

      private IPackageRepository _repo;

      [SetUp]
      public void SetUp()
      {
         _repo = new SqlPackageRepository(TestConnectionString);
      }

      [TearDown]
      public void TearDown()
      {
         if(_repo != null)
         {
            _repo.Dispose();
         }
      }

      [Test]
      public void WriteReadPackageTes()
      {
         var p = new Package("log4net", new Version("1.2.11"));
         long packageId = _repo.SavePackage(p);
         Assert.Greater(packageId, 0);

         Package p1 = _repo.GetPackage(packageId);
         Package p2 = _repo.GetPackage(p.Key);
         Assert.IsNotNull(p1);
         Assert.IsNotNull(p2);
         Assert.AreEqual(p1, p2);
      }
   }
}
