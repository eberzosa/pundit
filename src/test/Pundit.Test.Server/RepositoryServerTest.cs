/*using System;
using System.IO;
using NUnit.Framework;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Core.Server.Application;
using Pundit.Core.Server.Model;

namespace Pundit.Test.Server
{
   [TestFixture, Ignore]
   public class RepositoryServerTest
   {
      private string _rootDir;
      private IRemoteRepository _server;
      private IPackageRepository _package;

      [SetUp]
      public void SetUp()
      {
         _rootDir = Path.Combine(Path.GetTempPath(), "RepositoryServerTest");
         if(Directory.Exists(_rootDir)) Directory.Delete(_rootDir, true);
         Directory.CreateDirectory(_rootDir);

         _package = new MySqlPackageRepository(SqlPackageRepositoryTest.TestConnectionString);
         _server = new RemoteRepository(_package, _rootDir);
      }

      [TearDown]
      public void TearDown()
      {
         _package.Dispose();
         _server.Dispose();
         if (Directory.Exists(_rootDir)) Directory.Delete(_rootDir, true);
      }

      [Test]
      public void PublishTest()
      {
         _package.DeletePackage(new PackageKey("log4net", new Version("1.2.11.0"), "net20"));
         try
         {
            _server.Download("net20", "log4net", "1.2.11.0");
         }
         catch(FileNotFoundException)
         {
            //expect this
         }

         using (Stream s = File.OpenRead(Utils.GetLog4Net1211net20Package()))
         {
            _server.Publish(s);
         }

         using(Stream cs = _server.Download("net20", "log4net", "1.2.11.0"))
         {
            Assert.IsNotNull(cs);
            using(var rdr = new PackageReader(cs))
            {
               Assert.AreEqual("log4net", rdr.Manifest.PackageId);
               Assert.AreEqual("net20", rdr.Manifest.Platform);
               Assert.AreEqual("1.2.11.0", rdr.Manifest.VersionString);
            }
         }
      }

      [Test]
      public void EmptyRepositoryDeltaTest()
      {
         RemoteSnapshot s = _server.GetSnapshot(null);
         Assert.AreEqual(false, s.IsDelta);
         Assert.AreEqual(0, s.Changes.Length);
         Assert.IsNull(s.NextChangeId);
      }

      [Test]
      public void IncrementalDeltasTest()
      {
         using (Stream s = File.OpenRead(Utils.GetLog4Net1211net20Package()))
         {
            _server.Publish(s);
         }

         RemoteSnapshot snapshot0 = _server.GetSnapshot(null);
         Assert.IsTrue(snapshot0.IsDelta);
         Assert.AreEqual(1, snapshot0.Changes.Length);
         Assert.AreEqual("1", snapshot0.NextChangeId);

         using(Stream s = File.OpenRead(Utils.GetCasteCore30net40Package()))
         {
            _server.Publish(s);
         }

         RemoteSnapshot snapshot1 = _server.GetSnapshot(snapshot0.NextChangeId);
         Assert.IsTrue(snapshot0.IsDelta);
         Assert.AreEqual(1, snapshot1.Changes.Length);
         Assert.AreEqual("2", snapshot1.NextChangeId);

         RemoteSnapshot snapshot2 = _server.GetSnapshot(null);
         Assert.IsTrue(snapshot2.IsDelta);
         Assert.AreEqual(2, snapshot2.Changes.Length);
         Assert.AreEqual("2", snapshot2.NextChangeId);
      }

      [Test]
      public void OverwriteRevisionDeltaTest()
      {
         using(Stream s = File.OpenRead(Utils.GetLog4Net1210net20Package()))
         {
            _server.Publish(s);
         }
         RemoteSnapshot snapshot0 = _server.GetSnapshot(null);
         Assert.AreEqual("1.2.10.0", snapshot0.Changes[0].Manifest.VersionString);

         //next "revision" must delete exiting
         using(Stream s = File.OpenRead(Utils.GetLog4Net12101234net20Package()))
         {
            _server.Publish(s);
         }
         RemoteSnapshot snapshot1 = _server.GetSnapshot(null);
         Assert.AreEqual(3, snapshot1.Changes.Length);

         Assert.AreEqual("1.2.10.0", snapshot1.Changes[0].Manifest.VersionString);
         Assert.AreEqual(SnapshotPackageDiff.Add, snapshot1.Changes[0].Diff);

         Assert.AreEqual("1.2.10.0", snapshot1.Changes[1].Manifest.VersionString);
         Assert.AreEqual(SnapshotPackageDiff.Del, snapshot1.Changes[1].Diff);

         Assert.AreEqual("1.2.10.1234", snapshot1.Changes[2].Manifest.VersionString);
         Assert.AreEqual(SnapshotPackageDiff.Add, snapshot1.Changes[2].Diff);
      }
   }
}
*/