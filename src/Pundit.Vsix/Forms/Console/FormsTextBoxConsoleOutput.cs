using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Pundit.Core.Model;

namespace Pundit.Vsix.Forms.Console
{
   class FormsTextBoxConsoleOutput : IConsoleOutput
   {
      public event Action<string> ExecuteCommand;

      private static readonly int[] AllowedKeys =
         new[]
            {
               37,   //arrow left
               38,   //arrow up
               39,   //arrow right
               40,   //arrow down
               33,   //pgup
               34    //pgdn
            };

      private static readonly Color NormalColor = Color.Gray;

      private readonly RichTextBox _txt;

      public FormsTextBoxConsoleOutput(RichTextBox txt)
      {
         if (txt == null) throw new ArgumentNullException("txt");

         _txt = txt;
         _txt.Font = new Font("Consolas", 10);
         _txt.PreviewKeyDown += TxtPreviewKeyDown;
         PrintConsolePrompt();
      }

      void TxtPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
      {
         if (!AllowedKeys.Contains(e.KeyValue))
         {
            if (e.KeyValue == 13)
            {
               if (ExecuteCommand != null)
               {
                  string s = _txt.GetLastLineText();
                  if (s != null && s.Length > VSPackage.Console_Prompt.Length)
                  {
                     WriteLine(null);
                     ExecuteCommand(s.Substring(VSPackage.Console_Prompt.Length).Trim());
                     FixConsolePrompt();
                  }
               }
            }
            else
            {
               FixConsolePrompt();
               _txt.GoToEnd();
               _txt.SelectionColor = NormalColor;
            }
         }
      }

      void PrintConsolePrompt()
      {
         Write(Color.Black, VSPackage.Console_Prompt);
      }

      void FixConsolePrompt()
      {
         string prompt = _txt.GetLastLineText();
         if(prompt != null)
         {
            if(!prompt.StartsWith(VSPackage.Console_Prompt))
            {
               _txt.GoToLastLine();
               PrintConsolePrompt();
               _txt.GoToEnd();
            }
         }
      }

      private Color TranslateColor(ConsoleColor color)
      {
         switch(color)
         {
            case ConsoleColor.Red:
               return Color.Red;
            case ConsoleColor.Green:
               return Color.Green;
            case ConsoleColor.Yellow:
               return Color.Goldenrod;
            default:
               return Color.Black;
         }
      }

      #region Implementation of IConsoleOutput

      public void Write(string format, params object[] args)
      {
         if (format != null)
         {
            _txt.AppendText(string.Format(format, args));
         }
      }

      public void WriteLine(string format, params object[] args)
      {
         Write(format, args);
         _txt.AppendText(Environment.NewLine);
      }

      public void Write(ConsoleColor color, string format, params object[] args)
      {
         _txt.SelectionColor = TranslateColor(color);
         Write(format, args);
      }

      private void Write(Color color, string format, params object[] args)
      {
         _txt.SelectionColor = color;
         _txt.AppendText(string.Format(format, args));
         _txt.SelectionColor = NormalColor;
      }

      public void WriteLine(ConsoleColor defaultColor, string format, params object[] args)
      {
         Write(defaultColor, format, args);
         _txt.AppendText(Environment.NewLine);
      }

      public void Write(bool result)
      {
         Write(ConsoleColor.Black, "[ ");
         Write(
            result ? ConsoleColor.Green : ConsoleColor.Red,
            result ? "ok" : "fail");
         WriteLine(ConsoleColor.Black, " ]");
      }

      public void StartProgress(int maxValue)
      {
      }

      public void UpdateProgress(int value, string hint = null)
      {
      }

      public void FinishProgress()
      {
      }

      public void FinishCommand()
      {
         WriteLine(ConsoleColor.Gray, "______________");
      }

      #endregion
   }
}
