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
      public const string TestConnectionString = "Server=localhost;Database=pundit;Uid=pundit;Pwd=nopassword";

      private IPackageRepository _repo;

      [SetUp]
      public void SetUp()
      {
         _repo = new MySqlPackageRepository(TestConnectionString);

         long totalCount;
         foreach (DbPackage dbp in _repo.GetPackages(-1, -1, true, out totalCount))
         {
            _repo.DeletePackage(dbp.Package.Key);
         }
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
      public void NoPackagesTest()
      {
         long count;
         var packages = _repo.GetPackages(-1, -1, true, out count);
         Assert.AreEqual(0, packages.Count());
      }

      [Test]
      public void GetAllRevisionsTest()
      {
         string packageId1 = "fake-" + Guid.NewGuid().ToString();
         string packageId2 = "shite-" + Guid.NewGuid().ToString();

         for(int i = 0; i < 5; i++)
         {
            var p = new Package(packageId1, new Version("1.2.11." + i));
            _repo.SavePackage(p, 123, true);
         }
         for (int i = 0; i < 7; i++)
         {
            var p = new Package(packageId2, new Version("7.4.5." + i));
            _repo.SavePackage(p, 123, true);
         }

         var revs1 = _repo.GetAllRevisions(new PackageKey(packageId1, new Version("1.2.11.0"), null));
         var revs2 = _repo.GetAllRevisions(new PackageKey(packageId2, new Version("7.4.5.0"), null));

         Assert.AreEqual(5, revs1.Count());
         Assert.AreEqual(7, revs2.Count());
      }

      [Test]
      public void WriteReadPackageTest()
      {
         var p = new Package("fake", new Version("1.2.11"));
         p.Dependencies.Add(new PackageDependency("rhino", "1.2.3"));
         p.Dependencies.Add(new PackageDependency("mocks", "4.5.6"));
         long packageId = _repo.SavePackage(p, 123, true);
         Assert.Greater(packageId, 0);

         DbPackage p1 = _repo.GetPackage(packageId);
         DbPackage p2 = _repo.GetPackage(p.Key);
         Assert.IsNotNull(p1);
         Assert.IsNotNull(p2);
         Assert.AreEqual(p1, p2);
         Assert.AreEqual(2, p1.Package.Dependencies.Count);
         Assert.AreEqual(2, p2.Package.Dependencies.Count);
      }
   }
}
