using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pundit.Core.Application;
using Pundit.Core.Application.Server;
using Pundit.Core.Model;

namespace Pundit.Test.Server
{
   [TestFixture]
   public class RepositoryServerTest
   {
      private string _rootDir;
      private RepositoryServer _server;

      [SetUp]
      public void SetUp()
      {
         _rootDir = Path.Combine(Path.GetTempPath(), "RepositoryServerTest");
         if(Directory.Exists(_rootDir)) Directory.Delete(_rootDir, true);
         Directory.CreateDirectory(_rootDir);

         _server = new RepositoryServer(_rootDir);
      }

      [TearDown]
      public void TearDown()
      {
         _server.Dispose();
         if (Directory.Exists(_rootDir)) Directory.Delete(_rootDir, true);
      }

      [Test]
      public void PublishTest()
      {
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
      public void IncrementalDeltasTest()
      {
         RemoteSnapshot shapshot0 = _server.GetSnapshot(null);


      }
   }
}
