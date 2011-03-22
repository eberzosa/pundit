using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using NAnt.Core;
using NUnit.Framework;

namespace NGem.Test
{
   [TestFixture]
   public class ThirdPartyTryOutTest
   {
      [Test, Ignore]
      public void DirScanerTest()
      {
         DirectoryScanner dscan = new DirectoryScanner(false);

         dscan.BaseDirectory = new DirectoryInfo("c:\\devel");

         dscan.Includes.Add("**/tools/*");

         dscan.Scan();

         StringCollection dirs = dscan.DirectoryNames;
         StringCollection files = dscan.FileNames;
      }
   }
}
