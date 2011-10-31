using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using Pundit.Core;
using Pundit.Core.Application.Repository;
using Pundit.Core.Model;

namespace Pundit.Test
{
   [TestFixture, Ignore]
   public class SqliteRepositoryTest
   {
      private string _localFile;
      private SqliteRepository _repo;

      [SetUp]
      public void SetUp()
      {
         _localFile = Path.GetTempFileName();
         File.Delete(_localFile);

         _repo = (SqliteRepository)RepositoryFactory.CreateFromUri("sqlite://" + _localFile);
      }

      [TearDown]
      public void TearDown()
      {
         _repo.Dispose();
         File.Delete(_localFile);
      }

      [Test]
      public void PublishTest()
      {  
         string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
         path = Path.Combine(path, "stateless-2.3.1-1-net35.pundit");

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
         
      }
   }
}
