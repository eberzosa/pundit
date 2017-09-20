using EBerzosa.Pundit.Core.Model.Xml;

namespace EBerzosa.Pundit.Core.Serializers
{
   public class PackageSerializerFactory
   {
      public IPackageSerializer GetPundit() => new PackageSerializer<XmlPackageManifest, XmlPackageSpec>(new XmlSerializer());

      public IPackageSerializer GetNuGetv3() => new PackageSerializer<NuGet.Packaging.Manifest, NuGet.Packaging.Manifest>(new NuGetv3Serializer());
   }
}