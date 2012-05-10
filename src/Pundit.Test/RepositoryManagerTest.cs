using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

      [SetUp]
      public void SetUp()
      {
         _rootDir = Path.Combine(Path.GetTempPath(), "RepositoryManagerTest");
         if (Directory.Exists(_rootDir)) Directory.Delete(_rootDir, true);
         Directory.CreateDirectory(_rootDir);

         _manager = new RepositoryManager(Path.Combine(_rootDir, "localrp"));

         //_repo = new Repo();
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
         Assert.AreEqual(2, rc.Id);
      }

      [Test, Ignore]
      public void PlaySimpleAddSnaphotTest()
      {
      }

      [Test, Ignore]
      public void PlayAddsAndDeletesTest()
      {
         
      }
   }
}
