using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Pundit.Vsix.Model
{
   /// <summary>
   /// plain xml-serializable class for storing pundit options
   /// </summary>
   public class ExtensionSettings
   {
      public bool AutoResolveEnabled { get; set; }

      public long AutoResolveFrequencySec { get; set; }

      public string Serialize()
      {
         var s = new XmlSerializer(GetType());
         using(var ms = new MemoryStream())
         {
            s.Serialize(ms, this);
            return Encoding.UTF8.GetString(ms.ToArray());
         }
      }

      public static ExtensionSettings Deserialize(string s)
      {
         var sr = new XmlSerializer(typeof (ExtensionSettings));
         using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(s)))
         {
            return sr.Deserialize(ms) as ExtensionSettings;
         }
      }
   }
}
