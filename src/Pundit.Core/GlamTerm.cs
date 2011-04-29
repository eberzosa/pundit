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
         ForeNormalColor = Console.ForegroundColor;
         ForeWarnColor = ConsoleColor.Yellow;
         ForeErrorColor = ConsoleColor.Red;
      }

      public static void Write(ConsoleColor defaultColor, bool newLine, string format, params object[] args)
      {
         using(new ForeColorProtector(defaultColor))
         {
            if(format != null) Console.Out.Write(format, args);
         }

         if(newLine) Console.Out.WriteLine();
      }

      public static void Write(ConsoleColor color, string format, params object[] args)
      {
         Write(color, false, format, args);
      }

      public static void WriteLine(ConsoleColor defaultColor, string format, params object[] args)
      {
         Write(defaultColor, true, format, args);
      }

      public static void Write(string format, params object[] args)
      {
         Write(ForeNormalColor, false, format, args);
      }

      public static void WriteLine(string format, params object[] args)
      {
         Write(ForeNormalColor, true, format, args);
      }

      public static void WriteLine()
      {
         Write(ForeNormalColor, true, null);
      }

      public static void WriteWarnLine(string format, params object[] args)
      {
         Write(ForeWarnColor, true, format, args);
      }

      public static void WriteErrorLine(string format, params object[] args)
      {
         Write(ForeErrorColor, true, format, args);
      }

      public static void WriteStatsWord(string word, ConsoleColor wordColor)
      {
         if (word.Length > 4) word = word.Substring(0, 4);
         if (word.Length < 4) word = word.PadLeft(4);

         int width = Console.WindowWidth;

         Console.SetCursorPosition(width - 8, Console.CursorTop);
         Write(ConsoleColor.White, "[");
         Write(wordColor, word);
         WriteLine(ConsoleColor.White, "]");
      }

      public static void WriteOk()
      {
         WriteStatsWord(" ok ", ConsoleColor.Green);
      }

      public static void WriteFail()
      {
         WriteStatsWord("fail", ConsoleColor.Red);
      }

      public static void WriteBool(bool result)
      {
         if(result) WriteOk();
         else WriteFail();
      }
   }
}
