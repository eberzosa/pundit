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
   public partial class EditDependenciesForm : Form
   {
      public EditDependenciesForm()
      {
         InitializeComponent();
      }

      public EditDependenciesForm(IEnumerable<PackageDependency> dependencies) : this()
      {
         ucD.Dependencies = dependencies;
      }

      [Browsable(false)]
      public IEnumerable<PackageDependency> Dependencies
      {
         get { return ucD.Dependencies; }
         set { ucD.Dependencies = value; }
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
