using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pundit.Core.Model;

namespace Pundit.WinForms.Core
{
   public partial class PackageFilesForm : Form
   {
      public PackageFilesForm()
      {
         InitializeComponent();
      }

      public PackageFilesForm(IEnumerable<SourceFiles> files) : this()
      {
         Files = files;
      }

      [Browsable(false)]
      public IEnumerable<SourceFiles> Files
      {
         get { return ucFiles.Files; }
         set { ucFiles.Files = value; }
      }

      private void cmdOk_Click(object sender, EventArgs e)
      {
         Close();
      }

      private void cmdCancel_Click(object sender, EventArgs e)
      {
         Close();
      }
   }
}
