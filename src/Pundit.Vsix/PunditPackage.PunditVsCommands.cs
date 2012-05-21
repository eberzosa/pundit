using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;

namespace Pundit.Vsix
{
   //IPunditVsCommands
   public partial class PunditPackage
   {
      public void ShowConsoleToolWindow()
      {
         ShowToolWindow();
      }

      public void OpenFileInEditor(string fullPath)
      {
         IVsCommandWindow service = (IVsCommandWindow)this.GetService(typeof(SVsCommandWindow));
         if (service != null)
         {
            string command = string.Format("File.OpenFile \"{0}\"", fullPath);
            service.ExecuteCommand(command);
         }
      }
   }
}
