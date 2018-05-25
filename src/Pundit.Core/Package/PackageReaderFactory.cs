using System.IO;
using EBerzosa.Pundit.Core.Package.Pundit;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Serializers;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.Core.Package
{
   public class PackageReaderFactory
   {
      private readonly IPackageSerializer _packageSerializer;

      public PackageReaderFactory(IPackageSerializer packageSerializer)
      {
         Guard.NotNull(packageSerializer, nameof(packageSerializer));

         _packageSerializer = packageSerializer;
      }

      public IPackageReader Get(RepositoryType repoType, Stream stream)
      {
         if (repoType == RepositoryType.NuGet)
            return new NuGetPackageReader(stream);

         return new PunditPackageReader(_packageSerializer, stream);
      }
   }
}