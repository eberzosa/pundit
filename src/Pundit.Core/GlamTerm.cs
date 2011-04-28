using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core
{
   public static class GlamTerm
   {
      private class ForeColorProtector : IDisposable
      {
         private readonly ConsoleColor _oldColour;

         public ForeColorProtector(ConsoleColor color)
         {
            _oldColour = Console.ForegroundColor;

            Console.ForegroundColor = color;
         }

         public void Dispose()
         {
            Console.ForegroundColor = _oldColour;
         }
      }

      public static ConsoleColor ForeNormalColor { get; set; }
      public static ConsoleColor ForeWarnColor { get; set; }
      public static ConsoleColor ForeErrorColor { get; set; }

      static GlamTerm()
      {
         ForeNormalColor = ConsoleColor.White;
         ForeWarnColor = ConsoleColor.Yellow;
         ForeErrorColor = ConsoleColor.Red;
      }

      public static void WriteLine(string format, params object[] arg)
      {
         using(new ForeColorProtector(ForeNormalColor))
         {
            Console.Out.WriteLine(format, arg);
         }
      }

      public static void WriteWarnLine(string format, params object[] args)
      {
         using(new ForeColorProtector(ForeWarnColor))
         {
            Console.Out.WriteLine(format, args);
         }
      }

      public static void WriteErrorLine(string format, params object[] args)
      {
         using (new ForeColorProtector(ForeErrorColor))
         {
            Console.Out.WriteLine(format, args);
         }
      }
   }
}
