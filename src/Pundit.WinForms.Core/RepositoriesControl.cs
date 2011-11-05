﻿using System;
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
   public partial class RepositoriesControl : UserControl
   {
      private BindingList<RegisteredRepository> _rr;

      public RepositoriesControl()
      {
         InitializeComponent();

         UpdateOccupiedSpace();

         _rr = new BindingList<RegisteredRepository>(new List<RegisteredRepository>(
            LocalConfiguration.Registered.RepositoriesArray
            ));
         lstRepos.DataSource = _rr;
         lstRepos.DisplayMember = "Name";
         txtRepoName.DataBindings.Add("Text", _rr, "Name");
         txtRepoUri.DataBindings.Add("Text", _rr, "Uri");
         cbRepoPublish.DataBindings.Add("Checked", _rr, "UseForPublishing");
         cbIsEnabled.DataBindings.Add("Checked", _rr, "IsEnabled");
      }

      private void UpdateOccupiedSpace()
      {
         txtLocalRepoPath.Text = LocalConfiguration.GlobalRootPath;
         txtUsedSpace.Text = PathUtils.FileSizeToString(LocalConfiguration.OccupiedSpace);
      }

      public void Save()
      {
         SaveRepositories();
      }

      private void SaveRepositories()
      {
         LocalConfiguration.Registered.RepositoriesArray = _rr.ToArray();

         using(Stream s = File.Create(LocalConfiguration.GlobalSettingsFilePath))
         {
            LocalConfiguration.Registered.SaveTo(s);
         }
      }

      private void cmdNavigateToLocalRepoFolder_Click(object sender, EventArgs e)
      {
         Process.Start("explorer.exe", txtLocalRepoPath.Text);
      }

      private void cmdAddRepo_Click(object sender, EventArgs e)
      {
         var rr = new RegisteredRepository("new repository");
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

      private void cmdClearCache_Click(object sender, EventArgs e)
      {
         if (DialogResult.Yes == Alert.AskYesNo(Strings.Alert_PurgeCache))
         {
            foreach (string file in Directory.GetFiles(LocalConfiguration.GlobalRootFilePath))
            {
               try
               {
                  File.Delete(file);
               }
               catch
               {
               }
            }
         }
      }
   }
}
