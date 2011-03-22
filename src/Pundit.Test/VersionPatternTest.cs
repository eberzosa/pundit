using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pundit.Core.Model;
using NUnit.Framework;

namespace Pundit.Test
{
   [TestFixture]
   public class VersionPatternTest
   {
      [Test]
      public void ValidConstructorTest()
      {
         VersionPattern p1 = new VersionPattern("*");
         VersionPattern p2 = new VersionPattern("12.*");
      }

      [Test]
      [ExpectedException(typeof(ArgumentException))]
      public void InvalidPatternTest()
      {
         new VersionPattern("r");
      }

      [Test]
      public void VersionComparisonTest()
      {
         VersionPattern p1 = new VersionPattern("1.20.*");
         VersionPattern p2 = new VersionPattern("1.20.4");

         //Version v1 = new Version(1, 20, int.MaxValue, int.MaxValue);
         //Version v2 = new Version(1, 20, 4, int.MaxValue);

         Assert.IsTrue(p1 > p2);
      }

      [Test]
      public void CompatibilityTest()
      {
         VersionPattern p1 = new VersionPattern("1.20.*");
         VersionPattern p2 = new VersionPattern("1.20.4");
         VersionPattern p3 = new VersionPattern("1.21.4.6");

         Assert.IsTrue(VersionPattern.AreCompatible(p1, p2));
         Assert.IsFalse(VersionPattern.AreCompatible(p2, p3));
      }
   }
}
