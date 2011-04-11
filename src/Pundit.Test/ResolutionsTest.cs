using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Test.Mocks;

namespace Pundit.Test
{
   [TestFixture]
   public class ResolutionsTest
   {
      private IRepository _repo;

      [SetUp]
      public void SetUp()
      {
         var repo = new MockedRepository();

         //log4net
         repo.SetVersions("log4net", new Version(1, 2, 10, 0), new Version(1, 2, 8, 0));
         repo.SetManifest(
            new PackageKey("log4net", new Version(1, 2, 10, 0), null),
            new Package("log4net", new Version(1, 2, 10, 0)));

         //Company.Logging
         repo.SetVersions("Company.Logging",
                          new Version(2, 0, 1, 23),
                          new Version(2, 0, 2, 100),
                          new Version(2, 1, 0, 234),
                          new Version(2, 1, 4, 567),
                          new Version(2, 1, 8, 1987),
                          new Version(3, 0, 0, 4863));
         Package loggingManifest = new Package("Company.Logging", new Version(3, 0, 0, 4863));
         loggingManifest.Dependencies.Add(new PackageDependency("log4net", "1.2.10"));
         repo.SetManifest(
            new PackageKey("Company.Logging", new Version(3, 0, 0, 4863), null),
            loggingManifest);

         _repo = repo;
      }

      [Test]
      public void SingleDependencyHappyTest()
      {
         Package pkg = new Package("Self.Library", new Version(1, 2, 0, 0));
         pkg.Dependencies.Add(new PackageDependency("log4net", "1.2"));

         DependencyResolution dr = new DependencyResolution(pkg, new[] { _repo });
         DependencyNode node = dr.Resolve();

         Assert.IsNotNull(node);
         Assert.IsTrue(node.HasVersions);
         Assert.IsTrue(node.HasManifest);
         Assert.AreEqual(1, node.Children.Count());

         DependencyNode log4net = node.Children.FirstOrDefault();
         Assert.IsNotNull(log4net);
         Assert.IsTrue(log4net.HasVersions);
         Assert.IsTrue(log4net.HasManifest);
      }

      [Test]
      public void SimpleTreeDependencyHappyTest()
      {
         Package pkg = new Package("Self.Library", new Version(1, 3, 0, 500));
         pkg.Dependencies.Add(new PackageDependency("Company.Logging", "3.0"));
         pkg.Dependencies.Add(new PackageDependency("log4net", "1.2.10"));

         DependencyResolution dr = new DependencyResolution(pkg, new[] { _repo });
         DependencyNode result = dr.Resolve();

         Assert.AreEqual(2, result.Children.Count());
         DependencyNode node1 = result.Children.ElementAt(0);
         DependencyNode node2 = result.Children.ElementAt(1);

         Assert.AreEqual(new Version(3, 0, 0, 4863), node1.ActiveVersion);
         Assert.AreEqual(new Version(1, 2, 10, 0), node2.ActiveVersion);
         Assert.AreEqual(1, node1.Children.Count());
         Assert.AreEqual(new Version(1, 2, 10, 0), node1.Children.First().ActiveVersion);
      }
   }
}
