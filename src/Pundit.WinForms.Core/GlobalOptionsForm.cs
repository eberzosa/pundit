using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pundit.Core;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.WinForms.Core
{
   public partial class GlobalOptionsForm : Form
   {
      private BindingList<RegisteredRepository> _rr;

      public GlobalOptionsForm()
      {
         InitializeComponent();

         txtLocalRepoPath.Text = LocalRepository.GlobalRootPath;
         txtUsedSpace.Text = PathUtils.FileSizeToString(LocalRepository.OccupiedSpace);

         _rr = new BindingList<RegisteredRepository>(new List<RegisteredRepository>(
            LocalRepository.Registered.RepositoriesArray
            ));
         lstRepos.DataSource = _rr;
         lstRepos.DisplayMember = "Name";
         txtRepoName.DataBindings.Add("Text", _rr, "Name");
         txtRepoUri.DataBindings.Add("Text", _rr, "Uri");
         cbRepoPublish.DataBindings.Add("Checked", _rr, "UseForPublishing");
      }

      private void cmdCancel_Click(object sender, EventArgs e)
      {
         Close();
      }

      private void SaveRepositories()
      {
         LocalRepository.Registered.RepositoriesArray = _rr.ToArray();

         using(Stream s = File.Create(LocalRepository.GlobalSettingsFilePath))
         {
            LocalRepository.Registered.SaveTo(s);
         }
      }

      private void cmdOk_Click(object sender, EventArgs e)
      {
         SaveRepositories();

         Close();
      }

      private void cmdNavigateToLocalRepoFolder_Click(object sender, EventArgs e)
      {
         Process.Start("explorer.exe", txtLocalRepoPath.Text);
      }

      private void cmdAddRepo_Click(object sender, EventArgs e)
      {
         RegisteredRepository rr = new RegisteredRepository();
         rr.Name = "new repository";
         _rr.Add(rr);
         lstRepos.SelectedIndex = lstRepos.Items.Count - 1;
      }

      private void UpdateButtons()
      {
         bool deleteEnabled = lstRepos.SelectedIndex != -1;
         bool moveUpEnabled = lstRepos.SelectedIndex > 0;
         bool moveDownEnabled = lstRepos.SelectedIndex != -1 && lstRepos.SelectedIndex < lstRepos.Items.Count - 1;

         cmdRepoUp.Enabled = moveUpEnabled;
         cmdRepoDown.Enabled = moveDownEnabled;
         cmdRepoDelete.Enabled = deleteEnabled;

         pnlRepoInfo.Visible = lstRepos.SelectedIndex != -1;
      }

      private void lstRepos_SelectedIndexChanged(object sender, EventArgs e)
      {
         UpdateButtons();
      }

      private void cmdRepoDelete_Click(object sender, EventArgs e)
      {
         if (lstRepos.SelectedIndex != -1)
         {
            _rr.RemoveAt(lstRepos.SelectedIndex);
         }
      }

      private void cmdRepoUp_Click(object sender, EventArgs e)
      {
         int idx = lstRepos.SelectedIndex;

         var swap = _rr[idx - 1];
         _rr.RemoveAt(idx - 1);
         _rr.Insert(idx, swap);
         lstRepos.SelectedIndex = idx - 1;

         UpdateButtons();
      }

      private void cmdRepoDown_Click(object sender, EventArgs e)
      {
         int idx = lstRepos.SelectedIndex;

         var swap = _rr[idx + 1];
         _rr.RemoveAt(idx + 1);
         _rr.Insert(idx, swap);
         lstRepos.SelectedIndex = idx + 1;

         UpdateButtons();
      }
   }
}
