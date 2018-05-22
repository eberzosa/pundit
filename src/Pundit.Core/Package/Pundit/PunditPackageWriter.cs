using System;
using System.IO;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Serializers;
using EBerzosa.Utils;
using ICSharpCode.SharpZipLib.Zip;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Package
{
   public class PunditPackageWriter : PackageWriter
   {
      private readonly IPackageSerializer _packageSerializer;
      private readonly ZipOutputStream _zipStream;

      public PunditPackageWriter(IPackageSerializer packageSerializer, string rootDirectory, PackageSpec packageSpec, Stream outputStream) 
         : base(rootDirectory, packageSpec)
      {
         Guard.NotNull(packageSerializer, nameof(packageSerializer));
         Guard.NotNull(outputStream, nameof(outputStream));

         _packageSerializer = packageSerializer;
         _zipStream = new ZipOutputStream(outputStream);
         _zipStream.SetLevel(9);
      }


      protected override void WriteManifest()
      {
         var entry = new ZipEntry(PackageManifest.DefaultManifestFileName);

         _zipStream.PutNextEntry(entry);
         
         _packageSerializer.SerializePackageManifest(PackageSpec, _zipStream);
      }

      protected override void WriteEmptyDirectory(string path)
      {
         _zipStream.PutNextEntry(new ZipEntry(path));
      }

      protected override void WriteFile(string filePath, long size, string relativePath)
      {

         var entry = new ZipEntry(relativePath)
         {
            DateTime = DateTime.Now,
            Size = size
         };
         _zipStream.PutNextEntry(entry);

         using (Stream fileStream = File.OpenRead(filePath))
            fileStream.CopyTo(_zipStream);
      }

      protected override void Dispose(bool disposing)
      {
         if (!disposing)
            return;

         _zipStream.Close();
         _zipStream.Dispose();
      }
   }
}