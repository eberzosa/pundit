using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using Pundit.Core.Model;

namespace Pundit.Vsix.Application
{
   class WpfTextBoxConsoleOutput : IConsoleOutput
   {
      private readonly RichTextBox _txt;
      private Brush _bg = new SolidColorBrush(Colors.Black);
      private Brush _fg = new SolidColorBrush(Colors.White);

      public WpfTextBoxConsoleOutput(RichTextBox txt)
      {
         if (txt == null) throw new ArgumentNullException("txt");

         _txt = txt;
      }

      private Brush TranslateColor(ConsoleColor color)
      {
         switch (color)
         {
            case ConsoleColor.Red:
               return Brushes.Red;
            case ConsoleColor.Green:
               return Brushes.Green;
            case ConsoleColor.Yellow:
               return Brushes.Yellow;
         }

         return _fg;
      }

      private void ScrollToEnd()
      {
         //_txt.Select(_txt.Text.Length, _txt.Text.Length);
         //_txt.CaretIndex = _txt.Text.Length;
         //_txt.ScrollToLine(_txt.LineCount - 1);
         _txt.ScrollToEnd();
         _txt.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
      }

      public void Write(string format, params object[] args)
      {
         Write(ConsoleColor.White, format, args);
      }

      public void WriteLine(string format, params object[] args)
      {
         WriteLine(ConsoleColor.White, format, args);
      }

      public void Write(ConsoleColor color, string format, params object[] args)
      {
         string s = string.Format(format, args);
         /*Paragraph p = new Paragraph();
         p.Margin = new Thickness(0.0);
         p.Foreground = TranslateColor(color);
         p.Inlines.Add(s);
         _txt.Document.Blocks.Add(p);*/

         TextRange tr = new TextRange(_txt.Document.ContentEnd, _txt.Document.ContentEnd);
         tr.Text = s;
         tr.ApplyPropertyValue(TextElement.ForegroundProperty, TranslateColor(color));

         ScrollToEnd();
      }

      public void WriteLine(ConsoleColor defaultColor, string format, params object[] args)
      {
         Write(defaultColor, format, args);
         Paragraph p = new Paragraph();
         p.Margin = new Thickness(0.0);
         _txt.Document.Blocks.Add(p);
         ScrollToEnd();
      }

      public void Write(bool result)
      {
         if(result) WriteLine(ConsoleColor.Green, "OK");
         else WriteLine(ConsoleColor.Red, "FAIL");
         ScrollToEnd();
      }

      public void StartProgress(int maxValue)
      {
         throw new NotImplementedException();
      }

      public void UpdateProgress(int value)
      {
      }
   }
}
