using System;
using NUnit.Framework;
using NGem.Core.Model;

namespace NGem.Test
{
   [TestFixture]
   public class PomTest
   {
      [Test]
      public void TestSerialization()
      {
         Package g = new Package() {PackageId = "commonlib", Version = new Version("1.2.3.4")};

         g.Dependencies.Add(new PackageDependency() {PackageId = "log4net", VersionPattern = "1.2.*"});

         string s = g.ToString();
      }
   }
}
