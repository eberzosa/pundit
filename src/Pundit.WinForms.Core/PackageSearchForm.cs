using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Pundit.Core;
using Pundit.Core.Model;

namespace Pundit.WinForms.Core
{
   public partial class PackageSearchForm : Form
   {
      public event Action<PackageKey> PackageSelected;
      private List<PackageKey> _result;

      public PackageSearchForm()
      {
         InitializeComponent();

         txtText.Focus();
      }

      private List<PackageKey> Search(string text)
      {
         var result = new List<PackageKey>();

         var names = new List<string>();
         names.Add("local");
         names.AddRange(LocalRepository.Registered.ActiveNames);

         foreach(string repoName in names)
         {
            IRepository repo = RepositoryFactory.CreateFromUri(LocalRepository.GetRepositoryUriFromName(repoName));

            result.AddRange(repo.Search(text));
         }

         return result.Distinct().ToList();
      }

      private void Search()
      {
         _result = Search(txtText.Text);

         gridResult.AutoGenerateColumns = false;
         gridResult.DataSource = _result;
      }

      private void txtText_KeyUp(object sender, KeyEventArgs e)
      {
         if(e.KeyCode == Keys.Return)
         {
            Search();
         }
      }

      private void cmdClose_Click(object sender, System.EventArgs e)
      {
         Close();
      }

      private void cmdFind_Click(object sender, System.EventArgs e)
      {
         Search();
      }

      private void gridResult_DoubleClick(object sender, EventArgs e)
      {
         if (gridResult.SelectedRows.Count == 1)
         {
            PackageKey key = _result[gridResult.SelectedRows[0].Index];

            if (PackageSelected != null)
            {
               PackageSelected(key);
            }
         }
      }
   }
}
