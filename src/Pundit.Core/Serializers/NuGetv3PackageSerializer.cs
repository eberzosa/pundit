using System.IO;
using ExpressMapper.Extensions;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Serializers
{
   internal class NuGetv3PackageSerializer<TManifest, TSpec> : IPackageSerializer
   {
      private readonly ISerializer _serializer;

      public NuGetv3PackageSerializer(ISerializer serializer)
      {
         _serializer = serializer;
      }

      public void SerializePackageManifest(PackageManifest package, Stream stream)
      {
         
         _serializer.Write(package.Map<PackageManifest, TManifest>(), stream);
      }

      public void SerializePackageSpec(PackageSpec packageSpec, Stream stream)
      {
         _serializer.Write(packageSpec.Map<PackageSpec, TSpec>(), stream);
      }

      public PackageManifest DeserializePackageManifest(Stream stream)
      {
         var deserializedPackage = _serializer.Read<TManifest>(stream);
         return deserializedPackage.Map<TManifest, PackageManifest>();
      }

      public PackageSpec DeserializePackageSpec(Stream stream)
      {
         var deserializedPackage = _serializer.Read<TSpec>(stream);
         return deserializedPackage.Map<TSpec, PackageSpec>();
      }
   }
}
