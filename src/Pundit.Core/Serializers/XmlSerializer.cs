using System.IO;
using System.Text;
using System.Xml;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Serializers
{
   internal class XmlSerializer : ISerializer
   {
      public void Write<T>(T obj, Stream stream)
      {
         var settings = new XmlWriterSettings
         {
            Encoding = Encoding.UTF8,
            Indent = true
         };

         using (var writer = XmlWriter.Create(stream, settings))
         {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            serializer.Serialize(writer, obj);
         }
      }

      public T Read<T>(Stream stream)
      {
         var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
         return (T)serializer.Deserialize(stream);
      }
   }
}
