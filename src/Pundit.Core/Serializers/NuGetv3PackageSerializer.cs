//using System.IO;
//using EBerzosa.Pundit.Core.Converters;
//using EBerzosa.Pundit.Core.Model.Package;
//using Mapster;
//using Pundit.Core.Model;

//namespace EBerzosa.Pundit.Core.Serializers
//{
//   internal class NuGetv3PackageSerializer : IPackageSerializer
//   {
//      private readonly ISerializer _serializer;

//      public NuGetv3PackageSerializer(ISerializer serializer)
//      {
//         _serializer = serializer;
//      }

//      public void SerializePackageManifest(PackageManifest package, Stream stream)
//      {
//         _serializer.Write(package.ToXmlPackageManifest(), stream);
//      }

//      public void SerializePackageSpec(PackageSpec packageSpec, Stream stream)
//      {
//         _serializer.Write(packageSpec.ToXmlPackageSpec(), stream);
//      }

//      public PackageManifest DeserializePackageManifest(Stream stream)
//      {
//         var deserializedPackage = _serializer.Read<TManifest>(stream);
//         return deserializedPackage.Adapt<TManifest, PackageManifest>();
//      }

//      public PackageSpec DeserializePackageSpec(Stream stream)
//      {
//         var deserializedPackage = _serializer.Read<TSpec>(stream);
//         return deserializedPackage.Adapt<TSpec, PackageSpec>();
//      }
//   }
//}
