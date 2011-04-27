namespace Pundit.WinForms.Core
{
   partial class GlobalSettingsForm
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobalSettingsForm));
         this.label1 = new System.Windows.Forms.Label();
         this.txtLocalRepoPath = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.txtUsedSpace = new System.Windows.Forms.TextBox();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.cmdRepoDelete = new System.Windows.Forms.Button();
         this.lstRepos = new System.Windows.Forms.ListBox();
         this.cmdOk = new System.Windows.Forms.Button();
         this.cmdCancel = new System.Windows.Forms.Button();
         this.cmdNavigateToLocalRepoFolder = new System.Windows.Forms.Button();
         this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
         this.cmdRepoDown = new System.Windows.Forms.Button();
         this.cmdRepoUp = new System.Windows.Forms.Button();
         this.cmdAddRepo = new System.Windows.Forms.Button();
         this.pnlRepoInfo = new System.Windows.Forms.Panel();
         this.label3 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.txtRepoName = new System.Windows.Forms.TextBox();
         this.txtRepoUri = new System.Windows.Forms.TextBox();
         this.cbRepoPublish = new System.Windows.Forms.CheckBox();
         this.groupBox1.SuspendLayout();
         this.pnlRepoInfo.SuspendLayout();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(4, 7);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(108, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "Local repository path:";
         // 
         // txtLocalRepoPath
         // 
         this.txtLocalRepoPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.txtLocalRepoPath.Location = new System.Drawing.Point(118, 4);
         this.txtLocalRepoPath.Name = "txtLocalRepoPath";
         this.txtLocalRepoPath.ReadOnly = true;
         this.txtLocalRepoPath.Size = new System.Drawing.Size(389, 20);
         this.txtLocalRepoPath.TabIndex = 1;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(45, 33);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(67, 13);
         this.label2.TabIndex = 2;
         this.label2.Text = "Used space:";
         // 
         // txtUsedSpace
         // 
         this.txtUsedSpace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.txtUsedSpace.Location = new System.Drawing.Point(118, 30);
         this.txtUsedSpace.Name = "txtUsedSpace";
         this.txtUsedSpace.ReadOnly = true;
         this.txtUsedSpace.Size = new System.Drawing.Size(281, 20);
         this.txtUsedSpace.TabIndex = 3;
         // 
         // groupBox1
         // 
         this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.groupBox1.Controls.Add(this.pnlRepoInfo);
         this.groupBox1.Controls.Add(this.cmdRepoDelete);
         this.groupBox1.Controls.Add(this.cmdRepoDown);
         this.groupBox1.Controls.Add(this.cmdRepoUp);
         this.groupBox1.Controls.Add(this.cmdAddRepo);
         this.groupBox1.Controls.Add(this.lstRepos);
         this.groupBox1.Location = new System.Drawing.Point(7, 57);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(537, 201);
         this.groupBox1.TabIndex = 4;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Additional repositories";
         // 
         // cmdRepoDelete
         // 
         this.cmdRepoDelete.Enabled = false;
         this.cmdRepoDelete.Image = ((System.Drawing.Image)(resources.GetObject("cmdRepoDelete.Image")));
         this.cmdRepoDelete.Location = new System.Drawing.Point(134, 110);
         this.cmdRepoDelete.Name = "cmdRepoDelete";
         this.cmdRepoDelete.Size = new System.Drawing.Size(23, 23);
         this.cmdRepoDelete.TabIndex = 4;
         this.toolTip1.SetToolTip(this.cmdRepoDelete, "delete selected repository");
         this.cmdRepoDelete.UseVisualStyleBackColor = true;
         this.cmdRepoDelete.Click += new System.EventHandler(this.cmdRepoDelete_Click);
         // 
         // lstRepos
         // 
         this.lstRepos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
         this.lstRepos.FormattingEnabled = true;
         this.lstRepos.Location = new System.Drawing.Point(7, 20);
         this.lstRepos.Name = "lstRepos";
         this.lstRepos.Size = new System.Drawing.Size(120, 173);
         this.lstRepos.TabIndex = 0;
         this.lstRepos.SelectedIndexChanged += new System.EventHandler(this.lstRepos_SelectedIndexChanged);
         // 
         // cmdOk
         // 
         this.cmdOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.cmdOk.Location = new System.Drawing.Point(387, 264);
         this.cmdOk.Name = "cmdOk";
         this.cmdOk.Size = new System.Drawing.Size(75, 23);
         this.cmdOk.TabIndex = 5;
         this.cmdOk.Text = "&OK";
         this.cmdOk.UseVisualStyleBackColor = true;
         this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
         // 
         // cmdCancel
         // 
         this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.cmdCancel.Location = new System.Drawing.Point(468, 264);
         this.cmdCancel.Name = "cmdCancel";
         this.cmdCancel.Size = new System.Drawing.Size(75, 23);
         this.cmdCancel.TabIndex = 6;
         this.cmdCancel.Text = "&Cancel";
         this.cmdCancel.UseVisualStyleBackColor = true;
         this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
         // 
         // cmdNavigateToLocalRepoFolder
         // 
         this.cmdNavigateToLocalRepoFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdNavigateToLocalRepoFolder.Location = new System.Drawing.Point(512, 2);
         this.cmdNavigateToLocalRepoFolder.Name = "cmdNavigateToLocalRepoFolder";
         this.cmdNavigateToLocalRepoFolder.Size = new System.Drawing.Size(31, 23);
         this.cmdNavigateToLocalRepoFolder.TabIndex = 7;
         this.cmdNavigateToLocalRepoFolder.Text = ">";
         this.toolTip1.SetToolTip(this.cmdNavigateToLocalRepoFolder, "open folder in windows explorer...");
         this.cmdNavigateToLocalRepoFolder.UseVisualStyleBackColor = true;
         this.cmdNavigateToLocalRepoFolder.Click += new System.EventHandler(this.cmdNavigateToLocalRepoFolder_Click);
         // 
         // cmdRepoDown
         // 
         this.cmdRepoDown.Enabled = false;
         this.cmdRepoDown.Image = global::Pundit.WinForms.Core.Properties.Resources.arrow_down;
         this.cmdRepoDown.Location = new System.Drawing.Point(134, 80);
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
         this.cmdRepoUp.Image = ((System.Drawing.Image)(resources.GetObject("cmdRepoUp.Image")));
         this.cmdRepoUp.Location = new System.Drawing.Point(134, 50);
         this.cmdRepoUp.Name = "cmdRepoUp";
         this.cmdRepoUp.Size = new System.Drawing.Size(23, 23);
         this.cmdRepoUp.TabIndex = 2;
         this.toolTip1.SetToolTip(this.cmdRepoUp, "change priority moving up");
         this.cmdRepoUp.UseVisualStyleBackColor = true;
         this.cmdRepoUp.Click += new System.EventHandler(this.cmdRepoUp_Click);
         // 
         // cmdAddRepo
         // 
         this.cmdAddRepo.Image = ((System.Drawing.Image)(resources.GetObject("cmdAddRepo.Image")));
         this.cmdAddRepo.Location = new System.Drawing.Point(134, 20);
         this.cmdAddRepo.Name = "cmdAddRepo";
         this.cmdAddRepo.Size = new System.Drawing.Size(23, 23);
         this.cmdAddRepo.TabIndex = 1;
         this.toolTip1.SetToolTip(this.cmdAddRepo, "add repository");
         this.cmdAddRepo.UseVisualStyleBackColor = true;
         this.cmdAddRepo.Click += new System.EventHandler(this.cmdAddRepo_Click);
         // 
         // pnlRepoInfo
         // 
         this.pnlRepoInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.pnlRepoInfo.Controls.Add(this.cbRepoPublish);
         this.pnlRepoInfo.Controls.Add(this.txtRepoUri);
         this.pnlRepoInfo.Controls.Add(this.txtRepoName);
         this.pnlRepoInfo.Controls.Add(this.label4);
         this.pnlRepoInfo.Controls.Add(this.label3);
         this.pnlRepoInfo.Location = new System.Drawing.Point(163, 19);
         this.pnlRepoInfo.Name = "pnlRepoInfo";
         this.pnlRepoInfo.Size = new System.Drawing.Size(366, 174);
         this.pnlRepoInfo.TabIndex = 5;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(18, 11);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(38, 13);
         this.label3.TabIndex = 0;
         this.label3.Text = "Name:";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(27, 37);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(29, 13);
         this.label4.TabIndex = 1;
         this.label4.Text = "URI:";
         // 
         // txtRepoName
         // 
         this.txtRepoName.Location = new System.Drawing.Point(62, 8);
         this.txtRepoName.Name = "txtRepoName";
         this.txtRepoName.Size = new System.Drawing.Size(146, 20);
         this.txtRepoName.TabIndex = 2;
         // 
         // txtRepoUri
         // 
         this.txtRepoUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.txtRepoUri.Location = new System.Drawing.Point(62, 34);
         this.txtRepoUri.Name = "txtRepoUri";
         this.txtRepoUri.Size = new System.Drawing.Size(301, 20);
         this.txtRepoUri.TabIndex = 3;
         // 
         // cbRepoPublish
         // 
         this.cbRepoPublish.AutoSize = true;
         this.cbRepoPublish.Location = new System.Drawing.Point(62, 60);
         this.cbRepoPublish.Name = "cbRepoPublish";
         this.cbRepoPublish.Size = new System.Drawing.Size(138, 17);
         this.cbRepoPublish.TabIndex = 4;
         this.cbRepoPublish.Text = "publish to this repository";
         this.cbRepoPublish.UseVisualStyleBackColor = true;
         // 
         // GlobalOptionsForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(548, 299);
         this.Controls.Add(this.cmdNavigateToLocalRepoFolder);
         this.Controls.Add(this.cmdCancel);
         this.Controls.Add(this.cmdOk);
         this.Controls.Add(this.groupBox1);
         this.Controls.Add(this.txtUsedSpace);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.txtLocalRepoPath);
         this.Controls.Add(this.label1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
         this.Name = "GlobalOptionsForm";
         this.Text = "Pundit Global Settings";
         this.groupBox1.ResumeLayout(false);
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
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.Button cmdRepoDelete;
      private System.Windows.Forms.Button cmdRepoDown;
      private System.Windows.Forms.Button cmdRepoUp;
      private System.Windows.Forms.Button cmdAddRepo;
      private System.Windows.Forms.ListBox lstRepos;
      private System.Windows.Forms.Button cmdOk;
      private System.Windows.Forms.Button cmdCancel;
      private System.Windows.Forms.Button cmdNavigateToLocalRepoFolder;
      private System.Windows.Forms.ToolTip toolTip1;
      private System.Windows.Forms.Panel pnlRepoInfo;
      private System.Windows.Forms.CheckBox cbRepoPublish;
      private System.Windows.Forms.TextBox txtRepoUri;
      private System.Windows.Forms.TextBox txtRepoName;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label3;
   }
}