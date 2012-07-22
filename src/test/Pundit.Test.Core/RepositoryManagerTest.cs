using System;
using System.IO;
using NUnit.Framework;
using Pundit.Core.Application;
using Pundit.Core.Model;

namespace Pundit.Test
{
   [TestFixture]
   public class RepositoryManagerTest
   {
      private string _rootDir;
      private IRepositoryManager _manager;
      private Repo _repo;
      private Package _p1;
      private Package _p2;

      [SetUp]
      public void SetUp()
      {
         _rootDir = Path.Combine(Path.GetTempPath(), "RepositoryManagerTest");
         if (Directory.Exists(_rootDir)) Directory.Delete(_rootDir, true);
         Directory.CreateDirectory(_rootDir);

         _manager = new SqlRepositoryManager(Path.Combine(_rootDir, "localrp"));

         _repo = new Repo("primary", "http://primary.com");
         _repo = _manager.Register(_repo);

         using(Stream s = File.OpenRead(Utils.GetLog4Net1210net20Package()))
         {
            using (var rdr = new PackageReader(s))
            {
               _p1 = rdr.Manifest;
            }
         }

         using(Stream s = File.OpenRead(Utils.GetCasteCore30net40Package()))
         {
            using (var rdr = new PackageReader(s))
            {
               _p2 = rdr.Manifest;
            }
         }
      }

      [TearDown]
      public void TearDown()
      {
         _manager.Dispose();
         if (Directory.Exists(_rootDir)) Directory.Delete(_rootDir, true);
      }

      [Test]
      public void CreateRepositoryTest()
      {
         Repo r = new Repo("test1", "fake://fake");
         Repo rc = _manager.Register(r);

         Assert.IsNotNull(rc);
         Assert.AreEqual(4, rc.Id);
         Assert.IsTrue(rc.IsEnabled);
         Assert.AreEqual(1, rc.RefreshIntervalInHours);
         Assert.AreEqual("test1", rc.Tag);
         Assert.AreEqual(new Uri("fake://fake"), r.Uri);
      }

      [Test]
      public void PlaySimpleAddSnaphotTest()
      {
         var key1 = new PackageSnapshotKey(_p1, SnapshotPackageDiff.Add, null);
         var key2 = new PackageSnapshotKey(_p2, SnapshotPackageDiff.Add, null);
         var snapshot = new RemoteSnapshot(false, new[] {key1, key2}, "delta1");

         var r1 = _manager.LocalRepository.Search("log4net");
         var r2 = _manager.LocalRepository.Search("castle");
         Assert.AreEqual(0, r1.Count);
         Assert.AreEqual(0, r2.Count);

         _manager.PlaySnapshot(_repo, snapshot);
         r1 = _manager.LocalRepository.Search("log4net");
         r2 = _manager.LocalRepository.Search("castle");
         Assert.AreEqual(1, r1.Count);
         Assert.AreEqual(1, r2.Count);
      }

      [Test, Ignore]
      public void PlayAddsAndDeletesTest()
      {
         
      }
   }
}
