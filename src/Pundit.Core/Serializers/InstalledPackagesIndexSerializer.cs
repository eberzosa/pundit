using System.IO;
using EBerzosa.Pundit.Core.Converters;
using Pundit.Core.Application;

namespace EBerzosa.Pundit.Core.Serializers
{
   public class InstalledPackagesIndexSerializer
   {
      private readonly ISerializer _serializer;

      public InstalledPackagesIndexSerializer(ISerializer serializer)
      {
         _serializer = serializer;
      }
      public void SerializePackageManifest(InstalledPackagesIndex installedPackagesIndex, Stream stream)
      {
         var manifest = installedPackagesIndex.ToXmlInstalledPackagesIndex();
         _serializer.Write(manifest, stream);
      }

      public InstalledPackagesIndex DeserializeInstalledPackagesIndex(Stream stream, string filePath)
      {
         var deserialisedIndex = _serializer.Read<XmlInstalledPackagesIndex>(stream);
         return deserialisedIndex.ToInstalledPackagesIndex(filePath);
      }
   }
}
