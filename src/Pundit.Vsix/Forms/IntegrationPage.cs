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
   [Guid("EA09EABB-FC31-490E-823A-53089656DA0A")]
   [ComVisible(true)]
   [ClassInterface(ClassInterfaceType.AutoDual)]
   class IntegrationPage : DialogPage, IServiceProvider
   {
      private Integration _userControl;

      object IServiceProvider.GetService(Type serviceType)
      {
         return this.GetService(serviceType);
      }

      private Integration ContainerControl
      {
         get
         {
            if(_userControl == null)
            {
               _userControl = new Integration();
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
