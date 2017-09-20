using System.IO;

namespace EBerzosa.Pundit.Core.Serializers
{
   public interface ISerializer
   {
      void Write<T>(T obj, Stream stream);

      T Read<T>(Stream stream);
   }
}