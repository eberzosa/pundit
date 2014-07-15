using System;
using System.Drawing;
using Pundit.Core.Model;
using SC = System.Console;

namespace Pundit.Console
{
   class GlamTerm : IConsoleOutput
   {
      private bool? _canUpdate;

      private class ForeColorProtector : IDisposable
      {
         private readonly ConsoleColor _oldColour;

         public ForeColorProtector(ConsoleColor color)
         {
            _oldColour = SC.ForegroundColor;

            SC.ForegroundColor = color;
         }

         public void Dispose()
         {
            SC.ForegroundColor = _oldColour;
         }
      }

      ConsoleColor ForeNormalColor { get; set; }
      ConsoleColor ForeWarnColor { get; set; }
      ConsoleColor ForeErrorColor { get; set; }

      public GlamTerm()
      {
         ForeNormalColor = SC.ForegroundColor;
         ForeWarnColor = ConsoleColor.Yellow;
         ForeErrorColor = ConsoleColor.Red;
      }

      private void Write(ConsoleColor defaultColor, bool newLine, string format, params object[] args)
      {
         using(new ForeColorProtector(defaultColor))
         {
            if(format != null) SC.Out.Write(format, args);
         }
         
         if(newLine) SC.Out.WriteLine();
      }

      public void Write(ConsoleColor color, string format, params object[] args)
      {
         Write(color, false, format, args);
      }

      public void WriteLine(ConsoleColor defaultColor, string format, params object[] args)
      {
         Write(defaultColor, true, format, args);
      }

      public bool CanUpdate
      {
         get
         {
            if(_canUpdate == null)
            {
               try
               {
                  var w = SC.WindowWidth;
                  _canUpdate = true;
               }
               catch
               {
                  _canUpdate = false;
               }
            }

            return _canUpdate.Value;
         }
      }

      public void Write(string format, params object[] args)
      {
         Write(ForeNormalColor, false, format, args);
      }

      public void WriteLine(string format, params object[] args)
      {
         Write(ForeNormalColor, true, format, args);
      }

      public void WriteLine()
      {
         Write(ForeNormalColor, true, null);
      }

      private int WindowWidth
      {
         get
         {
            return CanUpdate ? SC.WindowWidth : 80;
         }
      }

      private void MoveCursor(int left, int top)
      {
         if(CanUpdate) SC.SetCursorPosition(left, top);
      }

      private void MoveCursor(int left)
      {
         MoveCursor(left, CursorPosition.Y);
      }

      private Point CursorPosition
      {
         get
         {
            int left = CanUpdate ? SC.CursorLeft : 0;
            int top = CanUpdate ? SC.CursorTop : 0;
            return new Point(left, top);
         }
      }

      private void WriteStatsWord(string word, ConsoleColor wordColor)
      {
         if (word.Length > 4) word = word.Substring(0, 4);
         if (word.Length < 4) word = word.PadLeft(4);

         MoveCursor(WindowWidth - 8, CursorPosition.Y);

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

      #region [ Progress Bar ]

      private int _progressMaxValue;
      private int _progressCurrentValue;

      public void StartProgress(int maxValue)
      {
         _progressCurrentValue = 0;
         _progressMaxValue = maxValue;
         UpdateProgress(0);
      }

      private string Multiply(char ch, int times)
      {
         string r = string.Empty;
         for (int i = 0; i < times; i++) r += ch;
         return r;
      }

      public void UpdateProgress(int value, string hint = null)
      {
         //[========               ] 035%
         int percent = _progressMaxValue == 0 ? 0 : value * 100 / _progressMaxValue;
         if (percent > 100) percent = 100;
         if (hint != null || percent != _progressCurrentValue)
         {
            //draw hint
            if(hint != null)
            {
               MoveCursor(0);
               int spaces = WindowWidth - hint.Length%WindowWidth + 1;
               Write(ConsoleColor.DarkGreen, hint);
               Write(ConsoleColor.Gray, Multiply(' ', spaces));
            }

            //draw line
            _progressCurrentValue = percent;
            int blocksTotal = WindowWidth - 
                              1 - //[
                              1 - //]
                              1 - //<space>
                              4 - //percentage
                              2 - //end spacing
                              0;
            int blocksPainted = percent*blocksTotal/100;
            int darkBlocks, liteBlocks, spaceBlocks;
            if(blocksPainted == 0)
            {
               darkBlocks = 0;
               liteBlocks = 1;
               spaceBlocks = blocksTotal - 1;
            }
            else
            {
               darkBlocks = blocksPainted - 1;
               liteBlocks = 1;
               spaceBlocks = blocksTotal - blocksPainted;
            }

            MoveCursor(0);
            Write(ConsoleColor.White, "[");
            Write(ConsoleColor.Green, Multiply('=', darkBlocks));
            Write(ConsoleColor.White, Multiply('=', liteBlocks));
            Write(ConsoleColor.Green, Multiply(' ', spaceBlocks));
            Write(ConsoleColor.White, "] ");
            Write(ConsoleColor.Yellow, percent.ToString().PadLeft(3));
            Write(ConsoleColor.Green, "% ");
         }
      }

      public void FinishProgress()
      {
         if(_progressMaxValue != 0) UpdateProgress(_progressMaxValue);
         SC.WriteLine();
      }

      public void FinishCommand()
      {
         WriteLine(ConsoleColor.Yellow, "all done.");
      }

      public void FixPrompt()
      {
      }

      #endregion

      public void ReturnCarriage()
      {
         if (CanUpdate)
         {
            MoveCursor(0);
         }
      }

      public void ClearToEnd()
      {
         if(CanUpdate)
         {
            int spaces = WindowWidth - CursorPosition.X - 1;
            if (spaces > 0)
            {
               string s = "";
               for (int i = 0; i < spaces; i++) s += " ";
               Write(s);
            }
         }
      }
   }
}
