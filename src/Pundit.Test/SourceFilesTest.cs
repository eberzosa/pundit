using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pundit.Core.Model;
using NUnit.Framework;

namespace Pundit.Test
{
   [TestFixture]
   public class SourceFilesTest
   {
      [Test]
      public void SearchEmptyDirsTest()
      {
         string cd = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\test");

         SourceFiles sf = new SourceFiles("**/Folder1");

         string[] files, directories;
         sf.Resolve(cd, out files, out directories);
      }
   }
}
