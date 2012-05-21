using System;
using System.Drawing;
using System.Windows.Forms;
using Pundit.Core.Model;

namespace Pundit.Vsix.Forms.Console
{
   class FormsTextBoxConsoleOutput : IConsoleOutput
   {
      public event Action<string> ExecuteCommand;

      private static readonly Color NormalColor = Color.Gray;

      private readonly Control _host;
      private readonly RichTextBox _txt;

      public FormsTextBoxConsoleOutput(Control host, RichTextBox txt)
      {
         if (host == null) throw new ArgumentNullException("host");
         if (txt == null) throw new ArgumentNullException("txt");

         _host = host;
         _txt = txt;
         _txt.Font = new Font("Consolas", 10);
         _txt.PreviewKeyDown += TxtPreviewKeyDown;
         _txt.KeyDown += TxtKeyDown;
         PrintConsolePrompt();
      }

      void TxtKeyDown(object sender, KeyEventArgs e)
      {
         if (e.KeyValue == 13)
         {
            e.SuppressKeyPress = true;
            if (ExecuteCommand != null)
            {
               string s = _txt.GetLastLineText();
               if (s != null && s.Length > VSPackage.Console_Prompt.Length)
               {
                  s = s.Trim();
                  if(s.StartsWith(VSPackage.Console_Prompt)) s = s.Substring(VSPackage.Console_Prompt.Length).Trim();
                  Execute(s);
               }
            }
         }
      }

      private void Execute(string s)
      {
         WriteLine(null);
         ExecuteCommand(s);
      }

      void TxtPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
      {
      }

      void PrintConsolePrompt()
      {
         Write(Color.Black, VSPackage.Console_Prompt);
      }

      public void FixPrompt()
      {
         UiInvoke(() =>
                     {
                        string prompt = _txt.GetLastLineText();
                        if (prompt != null)
                        {
                           if (!prompt.StartsWith(VSPackage.Console_Prompt))
                           {
                              _txt.GoToLastLine();
                              PrintConsolePrompt();
                              _txt.GoToEnd();

                           }
                        }

                     });
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

      private void UiInvoke(Action a)
      {
         if (a != null)
         {
            if (_host.InvokeRequired)
            {
               _host.Invoke(a);
            }
            else
            {
               a();
            }
         }
      }

      #region Implementation of IConsoleOutput

      public void Write(string format, params object[] args)
      {
         if (format != null)
         {
            UiInvoke(() => _txt.AppendText(string.Format(format, args)));
         }
      }

      public void WriteLine(string format, params object[] args)
      {
         Write(format + Environment.NewLine, args);
      }

      public void Write(ConsoleColor color, string format, params object[] args)
      {
         Write(TranslateColor(color), format, args);
      }

      private void Write(Color color, string format, params object[] args)
      {
         UiInvoke(() =>
                     {
                        _txt.SelectionColor = color;
                        _txt.AppendText(string.Format(format, args));
                        _txt.SelectionColor = NormalColor;
                     });
      }

      public void WriteLine(ConsoleColor defaultColor, string format, params object[] args)
      {
         Write(defaultColor, format + Environment.NewLine, args);
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
         FixPrompt();
      }

      #endregion
   }
}
