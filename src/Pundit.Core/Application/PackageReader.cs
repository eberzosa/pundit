using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using Pundit.Core.Model;

namespace Pundit.Core.Application
{
   public class PackageReader : IDisposable
   {
      private ZipInputStream _zipStream;

      public PackageReader(Stream packageStream)
      {
         _zipStream = new ZipInputStream(packageStream);
      }

      public Package ReadManifest()
      {
         ZipEntry entry;

         while((entry = _zipStream.GetNextEntry()) != null)
         {
            if(entry.IsFile && entry.Name == Package.DefaultPackageFileName)
            {
               Package manifest = Package.FromStream(_zipStream);

               return manifest;
            }
         }

         return null;
      }

      public void Dispose()
      {
         _zipStream.Close();
         _zipStream.Dispose();
      }
   }
}
