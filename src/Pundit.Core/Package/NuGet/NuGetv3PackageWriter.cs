using System;
using System.IO;
using EBerzosa.Pundit.Core.Converters;
using EBerzosa.Utils;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Package
{
   internal class NuGetv3PackageWriter : PackageWriter
   {
      private readonly Stream _stream;
      private readonly NuGet.Packaging.PackageBuilder _packageBuilder;

      public NuGetv3PackageWriter(string rootDirectory, PackageSpec packageSpec, Stream outputStream)
         : base(rootDirectory, packageSpec)
      {
         Guard.NotNull(outputStream, nameof(outputStream));

         _stream = outputStream;

         _packageBuilder = new NuGet.Packaging.PackageBuilder();
      }
      

      protected override void WriteManifest()
      {
         var metadata = PackageSpec.ToNuGetManifestMetadata();
         _packageBuilder.Populate(metadata);
      }

      protected override long GetCurrentSize()
      {
         return _stream.Length;
      }

      protected override void WriteEmptyDirectory(string path)
      {
         _packageBuilder.AddFiles("", null, path);
      }

      protected override void WriteFile(string filePath, long size, string relativePath)
      {
         _packageBuilder.AddFiles("", filePath, relativePath);
      }

      protected override void Dispose(bool disposing)
      {
         if (!disposing)
            return;

         _packageBuilder.Save(_stream);
         _stream.Dispose();
      }
   }
}