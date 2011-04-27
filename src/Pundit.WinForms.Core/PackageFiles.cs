using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pundit.Core.Model;

namespace Pundit.WinForms.Core
{
   public partial class PackageFiles : UserControl
   {
      private IEnumerable<SourceFiles> _allFiles;

      public PackageFiles()
      {
         InitializeComponent();

         cbConfiguration.SelectedIndex = 0;
      }

      [Browsable(false)]
      public IEnumerable<SourceFiles> Files
      {
         set { _allFiles = value; }
         get { return _allFiles; }
      }

      private void cbConfiguration_SelectedIndexChanged(object sender, EventArgs e)
      {

      }
   }
}
