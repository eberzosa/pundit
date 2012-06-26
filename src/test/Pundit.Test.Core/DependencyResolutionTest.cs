using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Test.Mocks;

namespace Pundit.Test
{
   [TestFixture]
   public class DependencyResolutionTest
   {
      private ILocalRepository _repo;

      [SetUp]
      public void SetUp()
      {
         var repo = new MockedRepository();

         //log4net
         repo.SetVersions("log4net", new Version(1, 2, 10, 0), new Version(1, 2, 8, 0), new Version(1, 1, 0, 0));
         repo.SetManifest(
            new PackageKey("log4net", new Version(1, 2, 10, 0), null),
            new Package("log4net", new Version(1, 2, 10, 0)));
         repo.SetManifest(
            new PackageKey("log4net", new Version(1, 2, 8, 0), null),
            new Package("log4net", new Version(1, 2, 8, 0)));
         repo.SetManifest(
            new PackageKey("log4net", new Version(1, 1, 0, 0), null),
            new Package("log4net", new Version(1, 1, 0, 0)));

         //Company.Logging
         repo.SetVersions("Company.Logging",
                          new Version(2, 0, 1, 23),
                          new Version(2, 0, 2, 100),
                          new Version(2, 1, 0, 234),
                          new Version(2, 1, 4, 567),
                          new Version(2, 1, 8, 1987),
                          new Version(3, 0, 0, 4863));
         Package loggingManifest30 = new Package("Company.Logging", new Version(3, 0, 0, 4863));
         loggingManifest30.Dependencies.Add(new PackageDependency("log4net", "1.2"));
         repo.SetManifest(
            new PackageKey("Company.Logging", new Version(3, 0, 0, 4863), null),
            loggingManifest30);

         _repo = repo;
      }

      /// <summary>
      /// Single dependency, trivial resolution to highest version
      /// </summary>
      [Test]
      public void TrivialTest()
      {
         Package pkg = new Package("Self.Library", new Version(1, 2, 0, 0));
         pkg.Dependencies.Add(new PackageDependency("log4net", "1.2"));

         DependencyResolution dr = new DependencyResolution(pkg, _repo );
         DependencyNode node = dr.Resolve().Item2;

         Assert.IsNotNull(node);
         Assert.IsTrue(node.HasVersions);
         Assert.IsTrue(node.HasManifest);
         Assert.AreEqual(1, node.Children.Count());

         DependencyNode log4net = node.Children.FirstOrDefault();
         Assert.IsNotNull(log4net);
         Assert.IsTrue(log4net.HasVersions);
         Assert.IsTrue(log4net.HasManifest);
      }

      /// <summary>
      /// Two dependencies:
      /// 
      /// Self.Library => (Company.Logging 3.0; log4net 1.2)
      /// Company.Logging => (log4net 1.2.10)
      /// 
      /// Simple resolution, no intersections or downgrades
      /// </summary>
      [Test]
      public void TrivialTreeTest()
      {
         Package pkg = new Package("Self.Library", new Version(1, 3, 0, 500));
         pkg.Dependencies.Add(new PackageDependency("Company.Logging", "3.0"));
         pkg.Dependencies.Add(new PackageDependency("log4net", "1.2.10"));

         DependencyResolution dr = new DependencyResolution(pkg, _repo);
         DependencyNode result = dr.Resolve().Item2;

         Assert.AreEqual(2, result.Children.Count());
         DependencyNode node1 = result.Children.ElementAt(0);
         DependencyNode node2 = result.Children.ElementAt(1);

         Assert.AreEqual(new Version(3, 0, 0, 4863), node1.ActiveVersion);
         Assert.AreEqual(new Version(1, 2, 10, 0), node2.ActiveVersion);
         Assert.AreEqual(1, node1.Children.Count());
         Assert.AreEqual(new Version(1, 2, 10, 0), node1.Children.First().ActiveVersion);
      }

      /// <summary>
      /// Simple intersection involved:
      /// 
      /// Self.Library => (Company.Logging 3.0; log4net 1.2.8)
      /// Company.Logging 3.0 => (log4net 1.2)
      /// 
      /// log4net dependency 1: {1.2.8}
      /// log4net dependency 2: {1.2.8, 1.2.10}
      /// 
      /// log4net must be downgraded to 1.2.8 for the dependency 2
      /// 
      /// </summary>
      [Test]
      public void SimpleVersionIntersectionTest()
      {
         var pkg = new Package("Self.Library", new Version(2, 0, 0, 501));
         pkg.Dependencies.Add(new PackageDependency("Company.Logging", "3.0"));
         pkg.Dependencies.Add(new PackageDependency("log4net", "1.2.8"));

         var dr = new DependencyResolution(pkg, _repo );
         var result = dr.Resolve();
         VersionResolutionTable table = result.Item1;
         IEnumerable<PackageKey> packages = table.GetPackages();

         Assert.IsFalse(table.HasConflicts);
         Assert.AreEqual(2, packages.Count());
         Assert.AreEqual(new Version(3, 0, 0, 4863), packages.ElementAt(0).Version);
         //Assert.AreEqual(new Version(2, 0, 0, 501), packages.ElementAt(0).Version);
         Assert.AreEqual(new Version(1, 2, 8, 0), packages.ElementAt(1).Version);

      }

      /// <summary>
      /// Simple conflict:
      /// 
      /// Self.Library => (Company.Logging 3.0; log4net 1.1.0)
      /// Company.Logging 3.0 => (log4net 1.2)
      /// 
      /// log4net dependency 1: {1.1.0}
      /// log4net dependency 2: {1.2.8, 1.2.10}
      /// 
      /// log4net dependency cannot be satisfied
      /// 
      /// </summary>
      [Test]
      public void SimpleConflictTest()
      {
         var pkg = new Package("Self.Library", new Version(2, 0, 0, 501));
         pkg.Dependencies.Add(new PackageDependency("Company.Logging", "3.0"));
         pkg.Dependencies.Add(new PackageDependency("log4net", "1.1"));

         var dr = new DependencyResolution(pkg, _repo );
         var result = dr.Resolve();
         VersionResolutionTable table = result.Item1;

         Assert.IsTrue(table.HasConflicts);
         Assert.AreEqual(1, table.ConflictCount);

         var resolved = table.GetPackages();
         var unresolved = table.GetConflictedPackages();

         Assert.AreEqual(1, resolved.Count());
         Assert.AreEqual(1, unresolved.Count());

         string cd = dr.DescribeConflict(result.Item2, unresolved.First());
      }
   }
}
