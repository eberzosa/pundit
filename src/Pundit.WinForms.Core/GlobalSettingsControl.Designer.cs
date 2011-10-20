namespace Pundit.WinForms.Core
{
   partial class GlobalSettingsControl
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         this.label1 = new System.Windows.Forms.Label();
         this.txtLocalRepoPath = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.txtUsedSpace = new System.Windows.Forms.TextBox();
         this.pnlRepoInfo = new System.Windows.Forms.Panel();
         this.cbIsEnabled = new System.Windows.Forms.CheckBox();
         this.cbRepoPublish = new System.Windows.Forms.CheckBox();
         this.txtRepoUri = new System.Windows.Forms.TextBox();
         this.txtRepoName = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.cmdRepoDelete = new System.Windows.Forms.Button();
         this.cmdRepoDown = new System.Windows.Forms.Button();
         this.cmdRepoUp = new System.Windows.Forms.Button();
         this.cmdAddRepo = new System.Windows.Forms.Button();
         this.lstRepos = new System.Windows.Forms.ListBox();
         this.cmdNavigateToLocalRepoFolder = new System.Windows.Forms.Button();
         this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
         this.cmdClearCache = new System.Windows.Forms.Button();
         this.label5 = new System.Windows.Forms.Label();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.groupBox3 = new System.Windows.Forms.GroupBox();
         this.label6 = new System.Windows.Forms.Label();
         this.pnlRepoInfo.SuspendLayout();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(14, 25);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(32, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "Path:";
         // 
         // txtLocalRepoPath
         // 
         this.txtLocalRepoPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.txtLocalRepoPath.Location = new System.Drawing.Point(108, 22);
         this.txtLocalRepoPath.Name = "txtLocalRepoPath";
         this.txtLocalRepoPath.ReadOnly = true;
         this.txtLocalRepoPath.Size = new System.Drawing.Size(313, 20);
         this.txtLocalRepoPath.TabIndex = 1;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(14, 51);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(88, 13);
         this.label2.TabIndex = 2;
         this.label2.Text = "Occupied space:";
         // 
         // txtUsedSpace
         // 
         this.txtUsedSpace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.txtUsedSpace.Location = new System.Drawing.Point(108, 48);
         this.txtUsedSpace.Name = "txtUsedSpace";
         this.txtUsedSpace.ReadOnly = true;
         this.txtUsedSpace.Size = new System.Drawing.Size(205, 20);
         this.txtUsedSpace.TabIndex = 3;
         // 
         // pnlRepoInfo
         // 
         this.pnlRepoInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.pnlRepoInfo.Controls.Add(this.cbIsEnabled);
         this.pnlRepoInfo.Controls.Add(this.cbRepoPublish);
         this.pnlRepoInfo.Controls.Add(this.txtRepoUri);
         this.pnlRepoInfo.Controls.Add(this.txtRepoName);
         this.pnlRepoInfo.Controls.Add(this.label4);
         this.pnlRepoInfo.Controls.Add(this.label3);
         this.pnlRepoInfo.Location = new System.Drawing.Point(148, 94);
         this.pnlRepoInfo.Name = "pnlRepoInfo";
         this.pnlRepoInfo.Size = new System.Drawing.Size(314, 113);
         this.pnlRepoInfo.TabIndex = 5;
         // 
         // cbIsEnabled
         // 
         this.cbIsEnabled.AutoSize = true;
         this.cbIsEnabled.Location = new System.Drawing.Point(38, 83);
         this.cbIsEnabled.Name = "cbIsEnabled";
         this.cbIsEnabled.Size = new System.Drawing.Size(64, 17);
         this.cbIsEnabled.TabIndex = 5;
         this.cbIsEnabled.Text = "enabled";
         this.cbIsEnabled.UseVisualStyleBackColor = true;
         // 
         // cbRepoPublish
         // 
         this.cbRepoPublish.AutoSize = true;
         this.cbRepoPublish.Location = new System.Drawing.Point(38, 59);
         this.cbRepoPublish.Name = "cbRepoPublish";
         this.cbRepoPublish.Size = new System.Drawing.Size(138, 17);
         this.cbRepoPublish.TabIndex = 4;
         this.cbRepoPublish.Text = "publish to this repository";
         this.cbRepoPublish.UseVisualStyleBackColor = true;
         // 
         // txtRepoUri
         // 
         this.txtRepoUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.txtRepoUri.Location = new System.Drawing.Point(38, 33);
         this.txtRepoUri.Name = "txtRepoUri";
         this.txtRepoUri.Size = new System.Drawing.Size(253, 20);
         this.txtRepoUri.TabIndex = 3;
         // 
         // txtRepoName
         // 
         this.txtRepoName.Location = new System.Drawing.Point(38, 7);
         this.txtRepoName.Name = "txtRepoName";
         this.txtRepoName.Size = new System.Drawing.Size(146, 20);
         this.txtRepoName.TabIndex = 2;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(8, 36);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(29, 13);
         this.label4.TabIndex = 1;
         this.label4.Text = "URI:";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(-1, 10);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(38, 13);
         this.label3.TabIndex = 0;
         this.label3.Text = "Name:";
         // 
         // cmdRepoDelete
         // 
         this.cmdRepoDelete.Enabled = false;
         this.cmdRepoDelete.Image = global::Pundit.WinForms.Core.Images.delete;
         this.cmdRepoDelete.Location = new System.Drawing.Point(118, 193);
         this.cmdRepoDelete.Name = "cmdRepoDelete";
         this.cmdRepoDelete.Size = new System.Drawing.Size(23, 23);
         this.cmdRepoDelete.TabIndex = 4;
         this.toolTip1.SetToolTip(this.cmdRepoDelete, "delete selected repository");
         this.cmdRepoDelete.UseVisualStyleBackColor = true;
         this.cmdRepoDelete.Click += new System.EventHandler(this.cmdRepoDelete_Click);
         // 
         // cmdRepoDown
         // 
         this.cmdRepoDown.Enabled = false;
         this.cmdRepoDown.Image = global::Pundit.WinForms.Core.Images.arrow_down;
         this.cmdRepoDown.Location = new System.Drawing.Point(118, 163);
         this.cmdRepoDown.Name = "cmdRepoDown";
         this.cmdRepoDown.Size = new System.Drawing.Size(23, 23);
         this.cmdRepoDown.TabIndex = 3;
         this.toolTip1.SetToolTip(this.cmdRepoDown, "change priority moving down");
         this.cmdRepoDown.UseVisualStyleBackColor = true;
         this.cmdRepoDown.Click += new System.EventHandler(this.cmdRepoDown_Click);
         // 
         // cmdRepoUp
         // 
         this.cmdRepoUp.Enabled = false;
         this.cmdRepoUp.Image = global::Pundit.WinForms.Core.Images.arrow_up;
         this.cmdRepoUp.Location = new System.Drawing.Point(118, 133);
         this.cmdRepoUp.Name = "cmdRepoUp";
         this.cmdRepoUp.Size = new System.Drawing.Size(23, 23);
         this.cmdRepoUp.TabIndex = 2;
         this.toolTip1.SetToolTip(this.cmdRepoUp, "change priority moving up");
         this.cmdRepoUp.UseVisualStyleBackColor = true;
         this.cmdRepoUp.Click += new System.EventHandler(this.cmdRepoUp_Click);
         // 
         // cmdAddRepo
         // 
         this.cmdAddRepo.Image = global::Pundit.WinForms.Core.Images.add;
         this.cmdAddRepo.Location = new System.Drawing.Point(118, 103);
         this.cmdAddRepo.Name = "cmdAddRepo";
         this.cmdAddRepo.Size = new System.Drawing.Size(23, 23);
         this.cmdAddRepo.TabIndex = 1;
         this.toolTip1.SetToolTip(this.cmdAddRepo, "add repository");
         this.cmdAddRepo.UseVisualStyleBackColor = true;
         this.cmdAddRepo.Click += new System.EventHandler(this.cmdAddRepo_Click);
         // 
         // lstRepos
         // 
         this.lstRepos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)));
         this.lstRepos.FormattingEnabled = true;
         this.lstRepos.Location = new System.Drawing.Point(17, 103);
         this.lstRepos.Name = "lstRepos";
         this.lstRepos.Size = new System.Drawing.Size(96, 186);
         this.lstRepos.TabIndex = 0;
         this.lstRepos.SelectedIndexChanged += new System.EventHandler(this.lstRepos_SelectedIndexChanged);
         // 
         // cmdNavigateToLocalRepoFolder
         // 
         this.cmdNavigateToLocalRepoFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdNavigateToLocalRepoFolder.Location = new System.Drawing.Point(426, 20);
         this.cmdNavigateToLocalRepoFolder.Name = "cmdNavigateToLocalRepoFolder";
         this.cmdNavigateToLocalRepoFolder.Size = new System.Drawing.Size(31, 23);
         this.cmdNavigateToLocalRepoFolder.TabIndex = 7;
         this.cmdNavigateToLocalRepoFolder.Text = ">";
         this.toolTip1.SetToolTip(this.cmdNavigateToLocalRepoFolder, "open folder in windows explorer...");
         this.cmdNavigateToLocalRepoFolder.UseVisualStyleBackColor = true;
         this.cmdNavigateToLocalRepoFolder.Click += new System.EventHandler(this.cmdNavigateToLocalRepoFolder_Click);
         // 
         // cmdClearCache
         // 
         this.cmdClearCache.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdClearCache.Location = new System.Drawing.Point(319, 46);
         this.cmdClearCache.Name = "cmdClearCache";
         this.cmdClearCache.Size = new System.Drawing.Size(75, 23);
         this.cmdClearCache.TabIndex = 8;
         this.cmdClearCache.Text = "clear cache";
         this.cmdClearCache.UseVisualStyleBackColor = true;
         this.cmdClearCache.Click += new System.EventHandler(this.cmdClearCache_Click);
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(-3, 4);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(33, 13);
         this.label5.TabIndex = 9;
         this.label5.Text = "Local";
         // 
         // groupBox2
         // 
         this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.groupBox2.Location = new System.Drawing.Point(37, 10);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(420, 3);
         this.groupBox2.TabIndex = 10;
         this.groupBox2.TabStop = false;
         // 
         // groupBox3
         // 
         this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.groupBox3.Location = new System.Drawing.Point(47, 84);
         this.groupBox3.Name = "groupBox3";
         this.groupBox3.Size = new System.Drawing.Size(409, 3);
         this.groupBox3.TabIndex = 12;
         this.groupBox3.TabStop = false;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(-3, 79);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(44, 13);
         this.label6.TabIndex = 11;
         this.label6.Text = "Remote";
         // 
         // GlobalSettingsControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.pnlRepoInfo);
         this.Controls.Add(this.groupBox3);
         this.Controls.Add(this.cmdRepoDelete);
         this.Controls.Add(this.label6);
         this.Controls.Add(this.cmdRepoDown);
         this.Controls.Add(this.groupBox2);
         this.Controls.Add(this.cmdRepoUp);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.cmdAddRepo);
         this.Controls.Add(this.cmdClearCache);
         this.Controls.Add(this.lstRepos);
         this.Controls.Add(this.cmdNavigateToLocalRepoFolder);
         this.Controls.Add(this.txtUsedSpace);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.txtLocalRepoPath);
         this.Controls.Add(this.label1);
         this.Name = "GlobalSettingsControl";
         this.Size = new System.Drawing.Size(472, 311);
         this.pnlRepoInfo.ResumeLayout(false);
         this.pnlRepoInfo.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox txtLocalRepoPath;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox txtUsedSpace;
      private System.Windows.Forms.Button cmdRepoDelete;
      private System.Windows.Forms.Button cmdRepoDown;
      private System.Windows.Forms.Button cmdRepoUp;
      private System.Windows.Forms.Button cmdAddRepo;
      private System.Windows.Forms.ListBox lstRepos;
      private System.Windows.Forms.Button cmdNavigateToLocalRepoFolder;
      private System.Windows.Forms.ToolTip toolTip1;
      private System.Windows.Forms.Panel pnlRepoInfo;
      private System.Windows.Forms.CheckBox cbRepoPublish;
      private System.Windows.Forms.TextBox txtRepoUri;
      private System.Windows.Forms.TextBox txtRepoName;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Button cmdClearCache;
      private System.Windows.Forms.CheckBox cbIsEnabled;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.GroupBox groupBox3;
      private System.Windows.Forms.Label label6;
   }
}