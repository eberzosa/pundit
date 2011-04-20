using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pundit.Core;
using Pundit.Core.Model;

namespace Pundit.GuiConfigure
{
   public partial class SearchForm : Form
   {
      public SearchForm()
      {
         InitializeComponent();
      }

      private IEnumerable<PackageKey> Search(string text)
      {
         var result = new List<PackageKey>();

         var names = new List<string>();
         names.Add("local");
         names.AddRange(LocalRepository.Registered.Names);

         foreach(string repoName in names)
         {
            IRepository repo = RepositoryFactory.CreateFromUri(LocalRepository.GetRepositoryUriFromName(repoName));

            //repo.Search()
         }

         return result;
      }

      private void txtText_KeyUp(object sender, KeyEventArgs e)
      {
         if(e.KeyCode == Keys.Return)
         {
            
         }
      }
   }
}
