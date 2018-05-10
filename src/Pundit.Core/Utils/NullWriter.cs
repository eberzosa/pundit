namespace EBerzosa.Pundit.Core.Utils
{
   internal class NullWriter : IWriter
   {
      public IWriter Title(string message) => this;

      public IWriter Header(string message) => null;

      public IWriter Text(string message) => this;

      public IWriter Reserved(string message) => this;

      public IWriter Info(string message) => this;

      public IWriter Success(string message) => this;

      public IWriter Error(string message) => this;

      public IWriter Warning(string message) => this;

      public IWriter HightLight(string message) => this;

      public IWriter Empty() => this;

      public IWriter BeginWrite() => this;

      public IWriter EndWrite() => this;

      public IWriter BeginColumns(int?[] length) => this;

      public IWriter EndColumns() => this;
   }
}
