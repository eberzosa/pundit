using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.Shell;
using Pundit.WinForms.Core;

namespace Pundit.Vsix.Forms
{
   [System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Microsoft.Interoperability",
    "CA1408:DoNotUseAutoDualClassInterfaceType")]
   [Guid("49E22212-E822-489E-A2A2-E0F91EE79CEB")]
   [ComVisible(true)]
   [ClassInterface(ClassInterfaceType.AutoDual)]
   class VsixOptionsPage : DialogPage, IServiceProvider
   {
      private VsixOptions _userControl;

      object IServiceProvider.GetService(Type serviceType)
      {
         return this.GetService(serviceType);
      }

      private VsixOptions ContainerControl
      {
         get
         {
            if(_userControl == null)
            {
               _userControl = new VsixOptions();
               _userControl.Location = new Point(0, 0);
            }

            return _userControl;
         }
      }

      protected override System.Windows.Forms.IWin32Window Window
      {
         get { return ContainerControl; }
      }

      protected override void OnApply(DialogPage.PageApplyEventArgs e)
      {
         ContainerControl.Save();
         base.OnApply(e);
      }
   }
}
