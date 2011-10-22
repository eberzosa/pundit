using System;

namespace Pundit.Core.Model
{
   public interface IConsoleOutput
   {
      void Write(string format, params object[] args);
      void WriteLine(string format, params object[] args);
      void Write(ConsoleColor color, string format, params object[] args);
      void WriteLine(ConsoleColor defaultColor, string format, params object[] args);
      void Write(bool result);
   }
}
