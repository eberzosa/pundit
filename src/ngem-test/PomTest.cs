using System;
using NUnit.Framework;
using NGem.Core.Model;

namespace NGem.Test
{
   [TestFixture]
   public class PomTest
   {
      [Test]
      public void PackageSerializationTest()
      {
         Package g = new Package() {PackageId = "commonlib", Version = new Version("1.2.3.4")};

         g.Dependencies.Add(new PackageDependency() {PackageId = "log4net", VersionPattern = "1.2.*"});

         string s = g.ToString();

         Assert.IsNotNull(s);
      }

      [Test]
      public void DevPackageSerializationTest()
      {
         DevPackage dp = new DevPackage()
                            {
                               PackageId = "nunit.framework",
                               Version = new Version("4.5.1.345"),
                               Author = "NUnit team"
                            };

         dp.Files.Add(new PackageFiles("framework\\nunit.framework.*", PackageFileKind.Binary, "NET40"));

         string s = dp.ToString();

         Assert.IsNotNull(s);
      }
   }
}
