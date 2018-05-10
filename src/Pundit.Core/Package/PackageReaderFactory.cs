using System.IO;
using EBerzosa.Pundit.Core.Package;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Serializers;

namespace EBerzosa.Pundit.Core.Application
{
   public class PackageReaderFactory
   {
      private readonly PackageSerializerFactory _packageSerializerFactory;

      public PackageReaderFactory(PackageSerializerFactory packageSerializer)
      {
         _packageSerializerFactory = packageSerializer;
      }

      public IPackageReader Get(RepositoryType repoType, Stream stream)
      {
         if (repoType == RepositoryType.NuGet)
            return new NuGetPackageReader(stream);

         return new PackageReader(_packageSerializerFactory, stream);
      }
   }
}