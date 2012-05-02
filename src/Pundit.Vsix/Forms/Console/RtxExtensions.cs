using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Threading;

namespace System.Windows.Controls
{
   static class RtxExtensions
   {
      public static void ScrollToEnd2(this RichTextBox txt)
      {
         txt.ScrollToEnd();
         txt.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
      }

      public static void CursorToEnd(this RichTextBox txt)
      {
         TextPointer moveTo = txt.CaretPosition.GetNextInsertionPosition(LogicalDirection.Forward);
         if (moveTo != null)
         {
            TextPointer t = moveTo;
            while(t != null)
            {
               t = moveTo.GetNextInsertionPosition(LogicalDirection.Forward);
               if (t != null) moveTo = t;
            }

            txt.CaretPosition = moveTo;
         }
      }
   }
}
