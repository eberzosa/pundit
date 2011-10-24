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
   [Guid("E0322F8D-1F16-4C63-8A01-67218284CF8B")]
   [ComVisible(true)]
   [ClassInterface(ClassInterfaceType.AutoDual)]
   class RepositoriesOptionsPage : DialogPage, IServiceProvider
   {
      private RepositoriesControl _userControl;

      object IServiceProvider.GetService(Type serviceType)
      {
         return this.GetService(serviceType);
      }

      private RepositoriesControl ContainerControl
      {
         get
         {
            if(_userControl == null)
            {
               _userControl = new RepositoriesControl();
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
