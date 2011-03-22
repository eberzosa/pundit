using System;

namespace Pundit.Console
{
   class ColorChange : IDisposable
   {
      private readonly ConsoleColor _oldcolor;

      public ColorChange(ConsoleColor color)
      {
         _oldcolor = System.Console.ForegroundColor;
         System.Console.ForegroundColor = color;
      }

      public void Dispose()
      {
         System.Console.ForegroundColor = _oldcolor;
      }
   }
}
