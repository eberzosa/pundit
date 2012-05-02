using System;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using Pundit.Core.Application.Console;
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
         _console = new WpfTextBoxConsoleOutput(txtConsole);
         _console.Write(Strings.Console_Prompt);
      }

      private string LastSolutionDirectory
      {
         get { return PunditPackage.LastSolutionDirectory == null ? null : PunditPackage.LastSolutionDirectory.FullName; }
      }

      private string LastManifestDirectory
      {
         get
         {
            string sd = LastSolutionDirectory;

            if (sd == null) return null;

            return new DirectoryInfo(sd).Parent.FullName;
         }
      }

      private void ExecuteCommand(string[] args)
      {
         try
         {
            IConsoleCommand cmd = CommandFactory.CreateCommand(_console,
               LastManifestDirectory,
               args);

            cmd.Execute();
         }
         catch (NoCurrentDirectoryException)
         {
            _console.WriteLine(ConsoleColor.Red, "this command requires a solution to be opened");
         }
         catch (Exception ex)
         {
            _console.WriteLine(ConsoleColor.Red, ex.Message);
         }
         finally
         {
            //txtCommand.Text = string.Empty;
         }         
      }

      public void ResolveDependencies()
      {
         ExecuteCommand(new[] { "resolve" });
      }

      private void CommandKeyDown(object sender, KeyEventArgs e)
      {
         txtConsole.CursorToEnd();

         if(e.Key == Key.Return)
         {

         }
      }      
   }
}
