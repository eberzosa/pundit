using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace Pundit.Vsix.Forms
{
   public partial class PageFooter : UserControl
   {
      public PageFooter()
      {
         InitializeComponent();

         InitializeLabels();         
      }

      private void InitializeLabels()
      {
         lblVersion.Text = string.Format(lblVersion.Text, Assembly.GetExecutingAssembly().GetName().Version);
      }

      private void lnkLicense_Click(object sender, System.EventArgs e)
      {
         Process.Start("http://pundit.codeplex.com/license");
      }

      private void lnkDocs_Click(object sender, System.EventArgs e)
      {
         Process.Start("http://pundit.codeplex.com/documentation");
      }
   }
}
