using System;
using System.Windows.Forms;
using Pundit.WinForms.Core;

namespace Pundit.WinForms.App
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main(string[] args)
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);

         if(args != null && args.Length > 0 && args[0] == "--global")
            Application.Run(new GlobalSettingsForm());
         else
            Application.Run(new MainForm());
      }
   }
}
