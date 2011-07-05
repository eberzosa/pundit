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
      private readonly BindingList<PackageDependency> _dependencies = new BindingList<PackageDependency>();

      public PackageDependencies()
      {
         InitializeComponent();

         _searchForm.PackageSelected += _searchForm_PackageSelected;

         gridDependencies.AutoGenerateColumns = false;
         gridDependencies.DataSource = _dependencies;
         gridDependencies.ReadOnly = false;

         DataGridViewComboBoxColumn column = (DataGridViewComboBoxColumn) gridDependencies.Columns["Scope"];
         column.DataSource = Enum.GetValues(typeof(DependencyScope));
      }

      public PackageDependencies(IEnumerable<PackageDependency> dependencies) : this()
      {
         Dependencies = dependencies;
      }

      [Browsable(false)]
      public IEnumerable<PackageDependency> Dependencies
      {
         get { return new List<PackageDependency>(_dependencies); }
         set
         {
            _dependencies.Clear();

            if (value != null)
            {
               foreach (var d in value) _dependencies.Add(d);
            }
         }
      }

      void _searchForm_PackageSelected(PackageKey obj)
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

      private void gridDependencies_SelectionChanged(object sender, EventArgs e)
      {
         cmdRemove.Enabled = gridDependencies.SelectedRows.Count > 0;
      }

      private void cmdRemove_Click(object sender, EventArgs e)
      {
         _dependencies.RemoveAt(gridDependencies.SelectedRows[0].Index);
      }
   }
}
