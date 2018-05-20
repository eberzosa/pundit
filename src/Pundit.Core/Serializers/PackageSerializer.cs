using System.IO;
using EBerzosa.Pundit.Core.Converters;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Model.Xml;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Serializers
{
   internal class PackageSerializer : IPackageSerializer
   {
      private readonly ISerializer _serializer;

      public PackageSerializer(ISerializer serializer)
      {
         _serializer = serializer;
      }

      public void SerializePackageManifest(PackageManifest package, Stream stream)
      {
         _serializer.Write(package.ToXmlPackageManifest(), stream);
      }

      public void SerializePackageSpec(PackageSpec packageSpec, Stream stream)
      {
         _serializer.Write(packageSpec.ToXmlPackageSpec(), stream);
      }

      public PackageManifest DeserializePackageManifest(Stream stream)
      {
         var deserializedPackage = _serializer.Read<XmlPackageManifest>(stream);
         return deserializedPackage.ToPackageManifest();
      }

      public PackageSpec DeserializePackageSpec(Stream stream)
      {
         var deserializedPackage = _serializer.Read<XmlPackageSpec>(stream);
         return deserializedPackage.ToPackageSpec();
      }
   }
}
