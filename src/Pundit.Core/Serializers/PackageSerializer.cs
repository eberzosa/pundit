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

      public void SerializePackageManifest(PackageManifest manifest, Stream stream)
      {
         var xmlManifest = manifest.ToXmlPackageManifest();
         _serializer.Write(xmlManifest, stream);
      }

      public void SerializePackageManifestRoot(PackageManifestRoot manifestRoot, Stream stream)
      {
         var xmlManifest = manifestRoot.ToXmlPackageManifestRoot();
         _serializer.Write(xmlManifest, stream);
      }

      public void SerializePackageSpec(PackageSpec packageSpec, Stream stream)
      {
         var spec = packageSpec.ToXmlPackageSpec();
         _serializer.Write(spec, stream);
      }

      public PackageManifest DeserializePackageManifest(Stream stream)
      {

         var deserializedPackage = _serializer.Read<XmlPackageLegacyCrap>(stream);
         return deserializedPackage.ToPackageManifest();
      }

      public PackageManifestRoot DeserializePackageManifestRoot(Stream stream)
      {
         var deserializedPackage = _serializer.Read<XmlPackageLegacyCrap>(stream);
         return deserializedPackage.ToPackageManifestRoot();
      }

      public PackageSpec DeserializePackageSpec(Stream stream)
      {
         var deserializedPackage = _serializer.Read<XmlPackageLegacyCrap>(stream);
         return deserializedPackage.ToPackageSpec();
      }
   }
}
