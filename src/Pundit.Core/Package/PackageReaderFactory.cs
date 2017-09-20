using System.IO;
using EBerzosa.Pundit.Core.Serializers;
using Pundit.Core.Application;

namespace EBerzosa.Pundit.Core.Application
{
   public class PackageReaderFactory
   {
      private readonly PackageSerializerFactory _packageSerializerFactory;

      public PackageReaderFactory(PackageSerializerFactory packageSerializer)
      {
         _packageSerializerFactory = packageSerializer;
      }

      public PackageReader Get(Stream stream)
      {
         return new PackageReader(_packageSerializerFactory, stream);
      }
   }
}