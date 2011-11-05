using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using Pundit.Core;
using Pundit.Core.Application;
using Pundit.Core.Application.Repository;
using Pundit.Core.Model;

namespace Pundit.Test
{
   [TestFixture, Ignore]
   public class SqliteRepositoryTest
   {
      private string _localFile;
      private SqliteRepository _repo;
      private string _dataPath;

      [SetUp]
      public void SetUp()
      {
         _localFile = Path.GetTempFileName();
         File.Delete(_localFile);

         _repo = (SqliteRepository)RepositoryFactory.CreateFromUri("sqlite://" + _localFile);
         _dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
      }

      [TearDown]
      public void TearDown()
      {
         _repo.Dispose();
         File.Delete(_localFile);
      }

      private void PublishSome()
      {
         string path = Path.Combine(_dataPath, "stateless-2.3.1-1-net35.pundit");

         using (Stream s = File.OpenRead(path))
         {
            _repo.Publish(s);
         }
      }

      [Test]
      public void PublishTest()
      {  
         string path = Path.Combine(_dataPath, "stateless-2.3.1-1-net35.pundit");

         using (Stream s = File.OpenRead(path))
         {
            _repo.Publish(s);
         }

         Stream ps = _repo.Download(new PackageKey("stateless", new Version("2.3.1.1"), "net35"));
         
         Assert.AreEqual(ps.Length, new FileInfo(path).Length);
      }

      [Test]
      public void GetVersionsTest()
      {
         string path = Path.Combine(_dataPath, "stateless-2.3.1-1-net35.pundit");
         using (Stream s = File.OpenRead(path))
         {
            _repo.Publish(s);
         }

         Version[] v1 = _repo.GetVersions(new UnresolvedPackage("stateless", "net35"), new VersionPattern("2.3"));
         Version[] v2 = _repo.GetVersions(new UnresolvedPackage("stateless", ""), new VersionPattern("2.3"));
         Version[] v3 = _repo.GetVersions(new UnresolvedPackage("stateless", ""), new VersionPattern("2.3.2"));

         Assert.AreEqual(1, v1.Length);
         Assert.AreEqual(0, v2.Length);
         Assert.AreEqual(0, v3.Length);
      }

      [Test]
      public void GetManifestTest()
      {
         string path = Path.Combine(_dataPath, "stateless-2.3.1-1-net35.pundit");
         Package p;
         using (Stream s = File.OpenRead(path))
         {
            p = new PackageReader(s).ReadManifest();
         }
         using (Stream s = File.OpenRead(path))
         {
            _repo.Publish(s);
         }

         Package p2 = _repo.GetManifest(p.Key);

         Assert.IsTrue(p.Key.Equals(p2.Key));
      }

      [Test]
      public void PackageExistsTest()
      {
         PublishSome();

         bool[] r = _repo.PackagesExist(new[]
                                           {
                                              new PackageKey("stateless", new Version("2.3.1.1"), "net35"),
                                              new PackageKey("stateless", new Version("2.3.1.2"), "net35"),
                                           });

         Assert.AreEqual(2, r.Length);
         Assert.IsTrue(r[0]);
         Assert.IsFalse(r[1]);
      }

      [Test]
      public void SearchTest()
      {
         PublishSome();

         PackageKey[] r = _repo.Search("stat");

         Assert.AreEqual(1, r.Length);
      }
   }
}
