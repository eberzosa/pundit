using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pundit.Core;

namespace Pundit.Test
{
   [TestFixture]
   public class LocalConfigurationTest
   {
      string _db = Path.Combine(Path.GetTempPath(), ".pundit.db");

      [SetUp]
      public void SetUp()
      {
         Environment.SetEnvironmentVariable(LocalConfiguration.LocalRepositoryRootVar, Path.GetTempPath());
         
         if(File.Exists(_db)) File.Delete(_db);
      }

      [TearDown]
      public void TearDown()
      {
         if (File.Exists(_db)) File.Delete(_db);
      }

      [Test]
      public void ListRepositoriesTest()
      {
         
      }
   }
}
