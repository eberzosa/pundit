using System.Collections.Specialized;
using System.IO;
using NAnt.Core;
using NUnit.Framework;

namespace Pundit.Test
{
   [TestFixture]
   public class NAntDirectoryScannerTest
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
