﻿using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace Pundit.Vsix.Forms.Console
{
   [Guid("D8F68431-0AE8-4742-AE23-F6B62A17B1B3")]
   public class ConsoleVsToolWindow : ToolWindowPane
   {
      private static PunditConsoleContent _instance;

      public ConsoleVsToolWindow() : base(null)
      {
         this.Caption = VSPackage.PunditConsoleWindowTitle;
         this.BitmapResourceID = 400;
         this.BitmapIndex = 0;
         _instance = new PunditConsoleContent();
         base.Content = _instance;
      }

      public static void ResolveDependencies()
      {
         if(_instance != null)
         {
            _instance.ResolveDependencies();
         }
      }

      public static void Search(string text, bool formatXml)
      {
         if(_instance != null)
         {
            _instance.Search(text, formatXml);
         }
      }
   }
}
