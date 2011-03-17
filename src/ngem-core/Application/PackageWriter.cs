using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using NGem.Core.Model;

namespace NGem.Core.Application
{
   public class PackageWriter : IDisposable
   {
      private readonly string _rootDirectory;
      private readonly Package _packageInfo;
      private readonly ZipOutputStream _zipStream;


      /// <summary>
      /// 
      /// </summary>
      /// <param name="rootDirectory">Root directory of a project (base path for all the file patterns)</param>
      /// <param name="packageInfo"></param>
      /// <param name="outputStream"></param>
      public PackageWriter(string rootDirectory, DevPackage packageInfo, Stream outputStream)
      {
         if (rootDirectory == null) throw new ArgumentNullException("rootDirectory");
         if (packageInfo == null) throw new ArgumentNullException("packageInfo");
         if (outputStream == null) throw new ArgumentNullException("outputStream");
         if (!outputStream.CanWrite) throw new ArgumentException("stream is not writable", "outputStream");

         packageInfo.Validate();

         _rootDirectory = rootDirectory;
         _packageInfo = packageInfo;
         _zipStream = new ZipOutputStream(outputStream);
         _zipStream.SetLevel(9);
      }

      public void WriteAll()
      {
         
      }

      //private 

      private void WriteManifest()
      {
         ZipEntry entry = new ZipEntry("manifest.");

         //zipStream.
      }

      private void WriteFingerprints()
      {
         
      }

      public void Dispose()
      {
         _zipStream.Close();
         _zipStream.Dispose();
      }
   }
}
