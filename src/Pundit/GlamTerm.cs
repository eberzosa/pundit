using System;
using Pundit.Core.Model;
using ConsoleS = System.Console;

namespace Pundit.Console
{
   class GlamTerm : IConsoleOutput
   {
      private class ForeColorProtector : IDisposable
      {
         private readonly ConsoleColor _oldColour;

         public ForeColorProtector(ConsoleColor color)
         {
            _oldColour = ConsoleS.ForegroundColor;

            ConsoleS.ForegroundColor = color;
         }

         public void Dispose()
         {
            ConsoleS.ForegroundColor = _oldColour;
         }
      }

      ConsoleColor ForeNormalColor { get; set; }
      ConsoleColor ForeWarnColor { get; set; }
      ConsoleColor ForeErrorColor { get; set; }

      public GlamTerm()
      {
         ForeNormalColor = ConsoleS.ForegroundColor;
         ForeWarnColor = ConsoleColor.Yellow;
         ForeErrorColor = ConsoleColor.Red;
      }

      private void Write(ConsoleColor defaultColor, bool newLine, string format, params object[] args)
      {
         using(new ForeColorProtector(defaultColor))
         {
            if(format != null) ConsoleS.Out.Write(format, args);
         }
         
         if(newLine) ConsoleS.Out.WriteLine();
      }

      public void Write(ConsoleColor color, string format, params object[] args)
      {
         Write(color, false, format, args);
      }

      public void WriteLine(ConsoleColor defaultColor, string format, params object[] args)
      {
         Write(defaultColor, true, format, args);
      }

      public void Write(string format, params object[] args)
      {
         Write(ForeNormalColor, false, format, args);
      }

      public void WriteLine(string format, params object[] args)
      {
         Write(ForeNormalColor, true, format, args);
      }

      private void WriteLine()
      {
         Write(ForeNormalColor, true, null);
      }

      private void WriteStatsWord(string word, ConsoleColor wordColor)
      {
         if (word.Length > 4) word = word.Substring(0, 4);
         if (word.Length < 4) word = word.PadLeft(4);

         int width = 80;

         try
         {
            width = ConsoleS.WindowWidth;
         }
         catch
         {
         }

         try
         {
            ConsoleS.SetCursorPosition(width - 8, ConsoleS.CursorTop);
         }
         catch
         {
         }


         Write(ConsoleColor.White, "[");
         Write(wordColor, word);
         WriteLine(ConsoleColor.White, "]");
      }

      private void WriteOk()
      {
         WriteStatsWord(" ok ", ConsoleColor.Green);
      }

      private void WriteFail()
      {
         WriteStatsWord("fail", ConsoleColor.Red);
      }

      public void Write(bool result)
      {
         if(result) WriteOk();
         else WriteFail();
      }
   }
}
