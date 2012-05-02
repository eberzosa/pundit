using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pundit.Vsix.Forms.Console
{
   public partial class FormsContainer : UserControl
   {
      public FormsContainer()
      {
         InitializeComponent();
      }

      public RichTextBox TextBox
      {
         get { return txtRich; }
      }
   }
}
