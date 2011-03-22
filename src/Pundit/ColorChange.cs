using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NGem
{
   class ColorChange : IDisposable
   {
      private readonly ConsoleColor _oldcolor;

      public ColorChange(ConsoleColor color)
      {
         _oldcolor = Console.ForegroundColor;
         Console.ForegroundColor = color;
      }

      public void Dispose()
      {
         Console.ForegroundColor = _oldcolor;
      }
   }
}
