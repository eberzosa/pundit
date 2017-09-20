using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using NuGet;

namespace EBerzosa.Pundit.Core.Serializers
{
   internal class NuGetv3Serializer : ISerializer
   {
      public void Write<T>(T obj, Stream stream)
      {
         var manifest = obj as NuGet.Packaging.Manifest;

         using (var ms = new MemoryStream())
         {
            manifest.Save(ms, validate: false);

            ms.Seek(0, SeekOrigin.Begin);

            var content = ms.ReadToEnd();
            content = Regex.Replace(content, @"(xmlns:?[^=]*=[""][^""]*[""])", String.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            var contentArray = Encoding.UTF8.GetBytes(content);
            stream.Write(contentArray, 0, contentArray.Length);
            //File.WriteAllText(manifest.Metadata.Id + NuGet.Constants.ManifestExtension, content);
         }
      }

      public T Read<T>(Stream stream)
      {
         //var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
         //return (T)serializer.Deserialize(stream);

         return default(T);
      }
   }
}
