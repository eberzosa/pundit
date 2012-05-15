using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
   static class FormsRtxExtensions
   {
      public static string GetLastLineText(this RichTextBox txt)
      {
         return txt.Lines.Length > 0 ? txt.Lines[txt.Lines.Length - 1] : null;
      }

      public static void SetLastLineText(this RichTextBox txt, string lineValue)
      {
         if(txt.Lines.Length > 0)
         {
            txt.Lines[txt.Lines.Length - 1] = lineValue;
         }
      }

      public static void GoToEnd(this RichTextBox txt)
      {
         txt.Select(txt.TextLength, 0);
      }

      public static void GoToLine(this RichTextBox txt, int lineNumber)
      {
         int lineIdx = 0;
         int charIdx = 0;
         foreach(string line in txt.Lines)
         {
            if(lineNumber == lineIdx)
            {
               txt.Select(charIdx, 0);
               break;
            }

            lineIdx++;
            charIdx += line.Length;
         }
      }

      public static void GoToLastLine(this RichTextBox txt)
      {
         txt.GoToLine(txt.Lines.Length);
      }

      public static bool IsOnLastLine(this RichTextBox txt)
      {
         int sel = txt.SelectionStart;
         int line = txt.GetLineFromCharIndex(sel);
         return line == txt.Lines.Length - 1;
      }
   }
}
