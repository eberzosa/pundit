using System.Windows.Controls;
using Pundit.Core.Model;

namespace Pundit.Vsix.Forms.Console
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
         var container = new FormsContainer();
         FormsHost.Child = container;
         var fco = new FormsTextBoxConsoleOutput(container, container.TextBox);
         fco.ExecuteCommand += UserExecuteCommand;
         _console = fco;

         ExtensionApplication.Instance.AssignConsole(_console);
      }

      void UserExecuteCommand(string rawText)
      {
         ExtensionApplication.Instance.ExecuteCommand(rawText);
      }
   }
}
