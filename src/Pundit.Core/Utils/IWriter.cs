namespace EBerzosa.Pundit.Core
{
   public interface IWriter
   {
      IWriter Title(string message);

      IWriter Header(string message);

      IWriter Text(string message);

      IWriter Reserved(string message);

      IWriter Info(string message);

      IWriter Success(string message);

      IWriter Error(string message);

      IWriter Warning(string message);

      IWriter HightLight(string message);

      IWriter Empty();

      IWriter BeginWrite();

      IWriter EndWrite();

      IWriter BeginColumns(int?[] length);

      IWriter EndColumns();

   }
}
