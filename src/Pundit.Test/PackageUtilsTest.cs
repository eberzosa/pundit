using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Test
{
   [TestFixture]
   public class PackageUtilsTest
   {
      [Test]
      public void GetPackageKeyFromFileNameTest()
      {
         string name = "Pundit.Core-1.0.0-4-noarch.pundit";

         PackageKey key = PackageUtils.GetPackageKeyFromFileName(name);

         Assert.AreEqual("Pundit.Core", key.PackageId);
         Assert.AreEqual(new Version(1, 0, 0, 4), key.Version);
         Assert.AreEqual("noarch", key.Platform);
      }
   }
}
