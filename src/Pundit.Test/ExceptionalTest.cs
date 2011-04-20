using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pundit.Core.Utils;

namespace Pundit.Test
{
   [TestFixture]
   public class ExceptionalTest
   {
      [Test]
      public void VsDocFileTest()
      {
         string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
         path = Path.Combine(path, "vsdoc.xml");

         Assert.IsTrue(Exceptional.IsVsDocFile(path));
      }

      [Test]
      public void NotVsDocFileTest()
      {
         string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
         path = Path.Combine(path, "nodoc.xml");

         Assert.IsFalse(Exceptional.IsVsDocFile(path));
      }
   }
}
