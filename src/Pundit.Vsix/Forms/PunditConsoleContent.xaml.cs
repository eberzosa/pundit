using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using Pundit.Core.Application.Console;
using Pundit.Core.Model;
using Pundit.Vsix.Application;

namespace Pundit.Vsix.Forms
{
   /// <summary>
   /// Interaction logic for PunditConsoleContent.xaml
   /// </summary>
   public partial class PunditConsoleContent : UserControl
   {
      private IConsoleOutput _console;

      public PunditConsoleContent()
      {
         InitializeComponent();

         InitConsole();
      }

      private void InitConsole()
      {
         _console = new WpfTextBoxConsoleOutput(txtConsole);

         _console.WriteLine(Strings.ConsoleIntro, Assembly.GetExecutingAssembly().GetName().Version);
      }

      private void CommandKeyUp(object sender, KeyEventArgs e)
      {
         if(e.Key == Key.Return)
         {
            try
            {
               IConsoleCommand cmd = CommandFactory.CreateCommand(_console, Environment.CurrentDirectory, txtCommand.Text.Split(' '));

               cmd.Execute();
            }
            catch(Exception ex)
            {
               _console.WriteLine(ConsoleColor.Red, ex.Message);
            }
            finally
            {
               txtCommand.Text = string.Empty;
            }
         }
      }
      
   }
}
