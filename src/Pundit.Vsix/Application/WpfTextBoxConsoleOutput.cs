using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Pundit.Core.Model;

namespace Pundit.Vsix.Application
{
   class WpfTextBoxConsoleOutput : IConsoleOutput
   {
      private readonly TextBox _txt;
      private Brush _bg = new SolidColorBrush(Colors.Black);
      private Brush _fg = new SolidColorBrush(Colors.White);

      public WpfTextBoxConsoleOutput(TextBox txt)
      {
         if (txt == null) throw new ArgumentNullException("txt");

         _txt = txt;
      }

      private Brush TranslateColor(ConsoleColor color)
      {
         return _fg;
      }

      private void ScrollToEnd()
      {
         //_txt.Select(_txt.Text.Length, _txt.Text.Length);
         _txt.CaretIndex = _txt.Text.Length;
         _txt.ScrollToLine(_txt.LineCount - 1);
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
         _txt.Text += string.Format(format, args);
         ScrollToEnd();
      }

      public void WriteLine(ConsoleColor defaultColor, string format, params object[] args)
      {
         Write(defaultColor, format, args);
         _txt.Text += Environment.NewLine;
         ScrollToEnd();
      }

      public void Write(bool result)
      {
         _txt.Text += result ? "OK" : "FAIL!";
         ScrollToEnd();
      }
   }
}
