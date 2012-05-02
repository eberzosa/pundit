using System;
using System.Drawing;
using System.Windows.Forms;
using Pundit.Core.Model;

namespace Pundit.Vsix.Forms.Console
{
   class FormsTextBoxConsoleOutput : IConsoleOutput
   {
      private readonly RichTextBox _txt;

      public FormsTextBoxConsoleOutput(RichTextBox txt)
      {
         if (txt == null) throw new ArgumentNullException("txt");

         _txt = txt;
         _txt.PreviewKeyDown += TxtPreviewKeyDown;
         Write(Color.Black, Strings.Console_Prompt);
      }

      void TxtPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
      {
         _txt.Select(_txt.TextLength, 0);
         _txt.SelectionColor = Color.Gray;
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
         throw new NotImplementedException();
      }

      public void WriteLine(string format, params object[] args)
      {
         throw new NotImplementedException();
      }

      public void Write(ConsoleColor color, string format, params object[] args)
      {
         _txt.SelectionColor = TranslateColor(color);
         _txt.AppendText(string.Format(format, args));
      }

      private void Write(Color color, string format, params object[] args)
      {
         _txt.SelectionColor = color;
         _txt.AppendText(string.Format(format, args));
      }

      public void WriteLine(ConsoleColor defaultColor, string format, params object[] args)
      {
         Write(defaultColor, format, args);
         _txt.AppendText(Environment.NewLine);
      }

      public void Write(bool result)
      {
         throw new NotImplementedException();
      }

      public void StartProgress(int maxValue)
      {
         throw new NotImplementedException();
      }

      public void UpdateProgress(int value, string hint = null)
      {
         throw new NotImplementedException();
      }

      public void FinishProgress()
      {
         throw new NotImplementedException();
      }

      public void FinishCommand()
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}
