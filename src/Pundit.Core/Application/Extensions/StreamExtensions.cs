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
      public static T FromXmlStream<T>(this Stream s, bool useNamespace = true)
      {
         if (s == null) return default(T);
         var xmls = useNamespace
                       ? new XmlSerializer(typeof (T), ObjectExtensions.XmlNamespace)
                       : new XmlSerializer(typeof (T));
         var result = (T) xmls.Deserialize(s);
         return result;
      }
   }
}
