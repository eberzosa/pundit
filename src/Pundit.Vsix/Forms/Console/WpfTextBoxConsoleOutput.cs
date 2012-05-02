using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using Pundit.Core.Model;

namespace Pundit.Vsix.Forms.Console
{
   class WpfTextBoxConsoleOutput : IConsoleOutput
   {
      private readonly RichTextBox _txt;
      private readonly Brush _defaultForeground = new SolidColorBrush(Colors.Black);

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
               return Brushes.Goldenrod;
         }

         return _defaultForeground;
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

         var tr = new TextRange(_txt.Document.ContentEnd, _txt.Document.ContentEnd) {Text = s};
         tr.ApplyPropertyValue(TextElement.ForegroundProperty, TranslateColor(color));

         _txt.ScrollToEnd2();
      }

      public void WriteLine(ConsoleColor defaultColor, string format, params object[] args)
      {
         Write(defaultColor, format, args);
         var p = new Paragraph {Margin = new Thickness(0.0)};
         _txt.Document.Blocks.Add(p);
         _txt.ScrollToEnd2();
      }

      public void Write(bool result)
      {
         if(result) WriteLine(ConsoleColor.Green, "OK");
         else WriteLine(ConsoleColor.Red, "FAIL");
         _txt.ScrollToEnd2();
      }

      public void StartProgress(int maxValue)
      {
      }

      public void UpdateProgress(int value, string hint)
      {
      }

      public void FinishProgress()
      {
      }

      public void FinishCommand()
      {
         
      }
   }
}
