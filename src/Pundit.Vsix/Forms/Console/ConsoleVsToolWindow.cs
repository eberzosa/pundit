using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace Pundit.Vsix.Forms.Console
{
   [Guid("D8F68431-0AE8-4742-AE23-F6B62A17B1B3")]
   public class ConsoleVsToolWindow : ToolWindowPane
   {
      private static PunditConsoleContent _instance;

      public ConsoleVsToolWindow() : base(null)
      {
         Caption = VSPackage.PunditConsoleWindowTitle;
         BitmapResourceID = 400;
         BitmapIndex = 0;
         _instance = new PunditConsoleContent();
         base.Content = _instance;
      }
   }
}
