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
         VersionPattern p1 = new VersionPattern("11.1.1.*");
         VersionPattern p2 = new VersionPattern("12.4.*");
      }

      [Test]
      [ExpectedException(typeof(ArgumentException))]
      public void InvalidPatternTest()
      {
         new VersionPattern("r");
      }

      [Test]
      public void Match1Test()
      {
         VersionPattern vp = new VersionPattern("2.3");

         Assert.IsTrue(vp.Matches(new Version("2.3")));
         Assert.IsTrue(vp.Matches(new Version("2.3.1")));
         Assert.IsTrue(vp.Matches(new Version("2.3.4.1")));
      }

      [Test]
      public void Match2Test()
      {
         VersionPattern vp = new VersionPattern("2.3.1");

         Assert.IsFalse(vp.Matches(new Version("2.3")));
         Assert.IsTrue(vp.Matches(new Version("2.3.1")));
         Assert.IsTrue(vp.Matches(new Version("2.3.1.4")));
      }

      [Test]
      public void Match3Test()
      {
         VersionPattern vp = new VersionPattern("2.3.1.4");

         Assert.IsFalse(vp.Matches(new Version("2.3")));
         Assert.IsFalse(vp.Matches(new Version("2.3.1")));
         Assert.IsTrue(vp.Matches(new Version("2.3.1.4")));
      }
   }
}
