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
