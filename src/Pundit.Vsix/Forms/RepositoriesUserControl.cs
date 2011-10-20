using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pundit.Vsix.Forms
{
   public partial class RepositoriesUserControl : UserControl
   {
      private IServiceProvider _serviceProvider;

      public RepositoriesUserControl(IServiceProvider serviceProvider)
      {
         InitializeComponent();

         _serviceProvider = serviceProvider;
      }
   }
}
