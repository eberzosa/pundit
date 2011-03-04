using System;
using NGem.Model;
using NUnit.Framework;

namespace NGem.Test
{
   [TestFixture]
   public class PomTest
   {
      [Test]
      public void TestSerialization()
      {
         Gem g = new Gem {PackageId = "commonlib", Version = new Version("1.2.3.4")};

         g.Dependencies.Add(new Dependency {PackageId = "log4net", VersionPattern = "1.2.*"});

         string s = g.ToString();
      }
   }
}
