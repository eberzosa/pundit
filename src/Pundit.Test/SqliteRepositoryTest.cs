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
   [TestFixture]
   public class SqliteRepositoryTest
   {
      private string _localFile;
      private SqliteLocalRepository _repo;
      private string _dataPath;

      [SetUp]
      public void SetUp()
      {
         _localFile = Path.GetTempFileName();
         File.Delete(_localFile);

         _repo = new SqliteLocalRepository(_localFile);
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
            _repo.Put(s);
         }
      }

      [Test]
      public void PublishTest()
      {  
         string path = Path.Combine(_dataPath, "stateless-2.3.1-1-net35.pundit");

         using (Stream s = File.OpenRead(path))
         {
            _repo.Put(s);
         }

         Stream ps = _repo.Get(new PackageKey("stateless", new Version("2.3.1.1"), "net35"));
         
         Assert.AreEqual(ps.Length, new FileInfo(path).Length);
      }

      [Test]
      public void GetVersionsTest()
      {
         string path = Path.Combine(_dataPath, "stateless-2.3.1-1-net35.pundit");
         using (Stream s = File.OpenRead(path))
         {
            _repo.Put(s);
         }

         var v1 = _repo.GetVersions(new UnresolvedPackage("stateless", "net35"), new VersionPattern("2.3"));
         var v2 = _repo.GetVersions(new UnresolvedPackage("stateless", ""), new VersionPattern("2.3"));
         var v3 = _repo.GetVersions(new UnresolvedPackage("stateless", ""), new VersionPattern("2.3.2"));

         Assert.AreEqual(1, v1.Count);
         Assert.AreEqual(0, v2.Count);
         Assert.AreEqual(0, v3.Count);
      }

      [Test]
      public void GetManifestTest()
      {
         string path = Path.Combine(_dataPath, "stateless-2.3.1-1-net35.pundit");
         Package p;
         using (Stream s = File.OpenRead(path))
         {
            p = new PackageReader(s).Manifest;
         }
         using (Stream s = File.OpenRead(path))
         {
            _repo.Put(s);
         }

         Package p2 = _repo.GetManifest(p.Key);

         Assert.IsTrue(p.Key.Equals(p2.Key));
      }

      [Test]
      public void PackageExistsTest()
      {
         PublishSome();

         ICollection<bool> r = _repo.BinariesExists(new[]
                                           {
                                              new PackageKey("stateless", new Version("2.3.1.1"), "net35"),
                                              new PackageKey("stateless", new Version("2.3.1.2"), "net35"),
                                           });

         Assert.AreEqual(2, r.Count);
         Assert.IsTrue(r.First());
         Assert.IsFalse(r.ElementAt(1));
      }

      [Test]
      public void SearchTest()
      {
         PublishSome();

         ICollection<PackageKey> r = _repo.Search("stat");

         Assert.AreEqual(1, r.Count);
      }
   }
}
