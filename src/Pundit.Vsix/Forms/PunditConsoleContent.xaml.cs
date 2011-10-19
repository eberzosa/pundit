using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pundit.Vsix.Forms
{
   /// <summary>
   /// Interaction logic for PunditConsoleContent.xaml
   /// </summary>
   public partial class PunditConsoleContent : UserControl
   {
      public PunditConsoleContent()
      {
         InitializeComponent();

         InitConsole();
      }

      private void WriteLine(string line)
      {
         txtConsole.Text += line;
         txtConsole.Text += Environment.NewLine;
      }

      private void InitConsole()
      {
         WriteLine(Strings.ConsoleIntro);
         WriteLine("ready.");
      }
      
   }
}
