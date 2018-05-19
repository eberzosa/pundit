using System.IO;
using EBerzosa.Pundit.Core.Model.Package;
using Mapster;
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
         _serializer.Write(package.Adapt<PackageManifest, TManifest>(), stream);
      }

      public void SerializePackageSpec(PackageSpec packageSpec, Stream stream)
      {
         _serializer.Write(packageSpec.Adapt<PackageSpec, TSpec>(), stream);
      }

      public PackageManifest DeserializePackageManifest(Stream stream)
      {
         var deserializedPackage = _serializer.Read<TManifest>(stream);
         return deserializedPackage.Adapt<TManifest, PackageManifest>();
      }

      public PackageSpec DeserializePackageSpec(Stream stream)
      {
         var deserializedPackage = _serializer.Read<TSpec>(stream);
         return deserializedPackage.Adapt<TSpec, PackageSpec>();
      }
   }
}
