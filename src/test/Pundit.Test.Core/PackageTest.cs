using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pundit.Core.Model;

namespace Pundit.Test
{
   [TestFixture]
   public class PackageTest
   {
      [Test]
      public void GenerateXmlTest()
      {
         Package pkg = new Package("pcore", new Version("1.2.3.4"));
         string s;
         using (MemoryStream ms = new MemoryStream())
         {
            pkg.WriteTo(ms);
            s = Encoding.UTF8.GetString(ms.ToArray());
         }

         Assert.IsTrue(s.Contains("pundit.xsd"));
      }

      [Test]
      public void BackwardCompatibilityTest()
      {
         string oldManifest = @"<package xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xsi:type=""DevPackage"" coreVersion=""1.0.0.11"">
  <packageId>Pundit.Core</packageId>
   <version>1.2.3.4</version>
  <platform>net40</platform>
</package>";

         DevPackage oldPackage = DevPackage.FromStreamXml(new MemoryStream(Encoding.UTF8.GetBytes(oldManifest)));
         Assert.AreEqual("Pundit.Core", oldPackage.PackageId);
         Assert.AreEqual("1.2.3.4", oldPackage.VersionString);
         Assert.AreEqual("net40", oldPackage.Platform);
      }
   }
}
