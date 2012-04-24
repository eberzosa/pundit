using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Test.Mocks;

namespace Pundit.Test
{
   [TestFixture]
   public class PackageWriterTest
   {
      private MemoryStream _outStream;
      private string _rootDir;
      private string _srcRootDir;

      [SetUp]
      public void SetUp()
      {
         _outStream = new MemoryStream();
         _rootDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestRepo");
         _srcRootDir = Path.Combine(_rootDir, "src");
      }

      [Test]
      public void SimpleBinaryPackTest()
      {
         //package definition
         DevPackage pkg = new DevPackage();
         pkg.PackageId = "log4net";
         pkg.Platform = "net40";
         pkg.Version = new Version("1.2.11.0");
         pkg.Files.Add(new SourceFiles("src/log4net", "*", PackageFileKind.Binary));

         //pack the file
         long writtenSize;
         using(var writer = new PackageWriter(_rootDir, pkg, _outStream))
         {
            writtenSize = writer.WriteAll();
         }
         long twoFilesLength =
            new FileInfo(Path.Combine(_srcRootDir, "log4net", "log4net.dll")).Length +
            new FileInfo(Path.Combine(_srcRootDir, "log4net", "log4net.xml")).Length;
         Assert.AreEqual(writtenSize, twoFilesLength);

         //unpack the file
         _outStream.Position = 0;
         Package upkg;
         var originalDependency = new PackageDependency("log4net", "1.2.11") {Platform = "net40"};
         var target = new StatCollectorInstallTarget();
         using(var reader = new PackageReader(_outStream))
         {
            upkg = reader.Manifest;
            reader.InstallTo(target, originalDependency, BuildConfiguration.Debug);
         }
         Assert.AreEqual(pkg.PackageId, upkg.PackageId);
         Assert.AreEqual(pkg.Platform, upkg.Platform);
         Assert.AreEqual(pkg.Version, upkg.Version);
         Assert.AreEqual(2, target.CreatedStreams.Count);
         Assert.AreEqual("lib/log4net.dll", target.CreatedStreams[0]);
         Assert.AreEqual("lib/log4net.xml", target.CreatedStreams[1]);
      }

   }
}
