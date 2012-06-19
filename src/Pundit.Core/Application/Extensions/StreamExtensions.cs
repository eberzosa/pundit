using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace System
{
   static class StreamExtensions
   {
      public static T FromXmlStream<T>(this Stream s)
      {
         if (s == null) return default(T);
         var xmls = new XmlSerializer(typeof(T), ObjectExtensions.XmlNamespace);
         var result = (T) xmls.Deserialize(s);
         return result;
      }
   }
}
