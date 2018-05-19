using System.IO;
using EBerzosa.Pundit.Core.Model.Package;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Serializers
{
   public interface IPackageSerializer
   {
      void SerializePackageManifest(PackageManifest package, Stream stream);

      void SerializePackageSpec(PackageSpec packageSpec, Stream stream);

      PackageManifest DeserializePackageManifest(Stream stream);

      PackageSpec DeserializePackageSpec(Stream stream);
   }
}