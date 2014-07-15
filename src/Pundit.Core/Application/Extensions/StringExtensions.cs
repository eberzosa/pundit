using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace System
{
   static class StringExtensions
   {
      public static T FromXmlString<T>(this string s)
      {
         var xmls = new XmlSerializer(typeof(T), ObjectExtensions.XmlNamespace);
         var ms = new MemoryStream(Encoding.UTF8.GetBytes(s));
         var obj = (T)xmls.Deserialize(ms);
         return obj;
      }
   }
}
