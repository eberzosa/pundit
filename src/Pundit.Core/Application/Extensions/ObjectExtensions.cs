using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace System
{
   static class ObjectExtensions
   {
      private const string XmlNamespace = "http://pundit-dm.com/1/pundit.xsd";

      public static void WriteXmlTo(this object obj, Stream s)
      {
         if (obj != null && s != null)
         {
            var settings = new XmlWriterSettings {Encoding = Encoding.UTF8, Indent = true};
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, XmlNamespace);

            using (XmlWriter writer = XmlWriter.Create(s, settings))
            {
               var x = new XmlSerializer(obj.GetType(), XmlNamespace);
               x.Serialize(writer, obj, namespaces);
            }
         }
      }

      public static string ToXmlString(this object obj)
      {
         if (obj != null)
         {
            using (var ms = new MemoryStream())
            {
               obj.WriteXmlTo(ms);
               return Encoding.UTF8.GetString(ms.ToArray());
            }
         }
         return null;
      }
   }
}
