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
   public partial class PackageDependencies : UserControl
   {
      readonly PackageSearchForm _searchForm = new PackageSearchForm();
      private BindingList<PackageDependency> _dependencies = new BindingList<PackageDependency>();

      public PackageDependencies()
      {
         InitializeComponent();

         _searchForm.PackageSelected += _searchForm_PackageSelected;

         gridDependencies.AutoGenerateColumns = false;
         gridDependencies.DataSource = _dependencies;
      }

      void _searchForm_PackageSelected(Pundit.Core.Model.PackageKey obj)
      {
         PackageDependency pd = new PackageDependency(obj.PackageId,
            obj.Version.Major + "." + obj.Version.Minor);
         pd.Platform = obj.Platform;

         _dependencies.Add(pd);
      }

      private void cmdAdd_Click(object sender, EventArgs e)
      {
         _searchForm.Show();
      }
   }
}
