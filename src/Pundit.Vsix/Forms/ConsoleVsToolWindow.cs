using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.Shell;

namespace Pundit.Vsix.Forms
{
   [Guid("D8F68431-0AE8-4742-AE23-F6B62A17B1B3")]
   public class ConsoleVsToolWindow : ToolWindowPane
   {
      public ConsoleVsToolWindow() : base(null)
      {
         this.Caption = Strings.PunditConsoleWindowTitle;
         this.BitmapResourceID = 301;
         this.BitmapIndex = 0;
         base.Content = new PunditConsoleContent();
      }
   }
}
