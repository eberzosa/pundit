namespace Pundit.GuiConfigure
{
   partial class MainForm
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
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.txtPackageId = new System.Windows.Forms.TextBox();
         this.statusStrip1 = new System.Windows.Forms.StatusStrip();
         this.label3 = new System.Windows.Forms.Label();
         this.cbPlatform = new System.Windows.Forms.ComboBox();
         this.txtProjectUrl = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.txtAuthor = new System.Windows.Forms.TextBox();
         this.linkLabel1 = new System.Windows.Forms.LinkLabel();
         this.linkLabel2 = new System.Windows.Forms.LinkLabel();
         this.linkLabel3 = new System.Windows.Forms.LinkLabel();
         this.gridDependencies = new System.Windows.Forms.DataGridView();
         this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Column4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
         this.label6 = new System.Windows.Forms.Label();
         this.txtVersion = new System.Windows.Forms.TextBox();
         this.cmdSave = new System.Windows.Forms.Button();
         this.cmdCancel = new System.Windows.Forms.Button();
         this.cmdTest = new System.Windows.Forms.Button();
         this.cmdAddDependency = new System.Windows.Forms.LinkLabel();
         this.linkLabel5 = new System.Windows.Forms.LinkLabel();
         ((System.ComponentModel.ISupportInitialize)(this.gridDependencies)).BeginInit();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(12, 11);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(61, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "packageId:";
         // 
         // label2
         // 
         this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(440, 11);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(44, 13);
         this.label2.TabIndex = 1;
         this.label2.Text = "version:";
         // 
         // txtPackageId
         // 
         this.txtPackageId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.txtPackageId.Location = new System.Drawing.Point(79, 8);
         this.txtPackageId.Name = "txtPackageId";
         this.txtPackageId.Size = new System.Drawing.Size(355, 20);
         this.txtPackageId.TabIndex = 2;
         // 
         // statusStrip1
         // 
         this.statusStrip1.Location = new System.Drawing.Point(0, 395);
         this.statusStrip1.Name = "statusStrip1";
         this.statusStrip1.Size = new System.Drawing.Size(603, 22);
         this.statusStrip1.TabIndex = 7;
         this.statusStrip1.Text = "statusStrip1";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(12, 37);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(47, 13);
         this.label3.TabIndex = 8;
         this.label3.Text = "platform:";
         // 
         // cbPlatform
         // 
         this.cbPlatform.FormattingEnabled = true;
         this.cbPlatform.Items.AddRange(new object[] {
            "net10",
            "net11",
            "net20",
            "net30",
            "net35",
            "net40"});
         this.cbPlatform.Location = new System.Drawing.Point(79, 33);
         this.cbPlatform.Name = "cbPlatform";
         this.cbPlatform.Size = new System.Drawing.Size(98, 21);
         this.cbPlatform.TabIndex = 9;
         // 
         // txtProjectUrl
         // 
         this.txtProjectUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.txtProjectUrl.Location = new System.Drawing.Point(253, 34);
         this.txtProjectUrl.Name = "txtProjectUrl";
         this.txtProjectUrl.Size = new System.Drawing.Size(338, 20);
         this.txtProjectUrl.TabIndex = 10;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(183, 37);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(64, 13);
         this.label4.TabIndex = 11;
         this.label4.Text = "projectURL:";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(12, 64);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(40, 13);
         this.label5.TabIndex = 12;
         this.label5.Text = "author:";
         // 
         // txtAuthor
         // 
         this.txtAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.txtAuthor.Location = new System.Drawing.Point(79, 61);
         this.txtAuthor.Name = "txtAuthor";
         this.txtAuthor.Size = new System.Drawing.Size(341, 20);
         this.txtAuthor.TabIndex = 16;
         // 
         // linkLabel1
         // 
         this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.linkLabel1.AutoSize = true;
         this.linkLabel1.Location = new System.Drawing.Point(426, 64);
         this.linkLabel1.Name = "linkLabel1";
         this.linkLabel1.Size = new System.Drawing.Size(58, 13);
         this.linkLabel1.TabIndex = 17;
         this.linkLabel1.TabStop = true;
         this.linkLabel1.Text = "description";
         // 
         // linkLabel2
         // 
         this.linkLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.linkLabel2.AutoSize = true;
         this.linkLabel2.Location = new System.Drawing.Point(485, 64);
         this.linkLabel2.Name = "linkLabel2";
         this.linkLabel2.Size = new System.Drawing.Size(70, 13);
         this.linkLabel2.TabIndex = 18;
         this.linkLabel2.TabStop = true;
         this.linkLabel2.Text = "release notes";
         // 
         // linkLabel3
         // 
         this.linkLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.linkLabel3.AutoSize = true;
         this.linkLabel3.Location = new System.Drawing.Point(558, 64);
         this.linkLabel3.Name = "linkLabel3";
         this.linkLabel3.Size = new System.Drawing.Size(40, 13);
         this.linkLabel3.TabIndex = 19;
         this.linkLabel3.TabStop = true;
         this.linkLabel3.Text = "license";
         // 
         // gridDependencies
         // 
         this.gridDependencies.AllowUserToAddRows = false;
         this.gridDependencies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.gridDependencies.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
         this.gridDependencies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.gridDependencies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
         this.gridDependencies.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
         this.gridDependencies.EnableHeadersVisualStyles = false;
         this.gridDependencies.Location = new System.Drawing.Point(12, 110);
         this.gridDependencies.Margin = new System.Windows.Forms.Padding(0);
         this.gridDependencies.Name = "gridDependencies";
         this.gridDependencies.ReadOnly = true;
         this.gridDependencies.RowHeadersVisible = false;
         this.gridDependencies.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.gridDependencies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
         this.gridDependencies.Size = new System.Drawing.Size(576, 247);
         this.gridDependencies.TabIndex = 20;
         // 
         // Column1
         // 
         this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
         this.Column1.HeaderText = "packageId";
         this.Column1.Name = "Column1";
         this.Column1.ReadOnly = true;
         // 
         // Column2
         // 
         this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
         this.Column2.HeaderText = "version";
         this.Column2.Name = "Column2";
         this.Column2.ReadOnly = true;
         // 
         // Column3
         // 
         this.Column3.HeaderText = "platform";
         this.Column3.Name = "Column3";
         this.Column3.ReadOnly = true;
         // 
         // Column4
         // 
         this.Column4.HeaderText = "devtime";
         this.Column4.Name = "Column4";
         this.Column4.ReadOnly = true;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(12, 94);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(77, 13);
         this.label6.TabIndex = 21;
         this.label6.Text = "dependencies:";
         // 
         // txtVersion
         // 
         this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.txtVersion.Location = new System.Drawing.Point(490, 8);
         this.txtVersion.Name = "txtVersion";
         this.txtVersion.Size = new System.Drawing.Size(101, 20);
         this.txtVersion.TabIndex = 22;
         // 
         // cmdSave
         // 
         this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdSave.Location = new System.Drawing.Point(432, 369);
         this.cmdSave.Name = "cmdSave";
         this.cmdSave.Size = new System.Drawing.Size(75, 23);
         this.cmdSave.TabIndex = 23;
         this.cmdSave.Text = "&Save";
         this.cmdSave.UseVisualStyleBackColor = true;
         // 
         // cmdCancel
         // 
         this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.cmdCancel.Location = new System.Drawing.Point(513, 369);
         this.cmdCancel.Name = "cmdCancel";
         this.cmdCancel.Size = new System.Drawing.Size(75, 23);
         this.cmdCancel.TabIndex = 24;
         this.cmdCancel.Text = "&Cancel";
         this.cmdCancel.UseVisualStyleBackColor = true;
         this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
         // 
         // cmdTest
         // 
         this.cmdTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.cmdTest.Location = new System.Drawing.Point(12, 369);
         this.cmdTest.Name = "cmdTest";
         this.cmdTest.Size = new System.Drawing.Size(75, 23);
         this.cmdTest.TabIndex = 25;
         this.cmdTest.Text = "&Test";
         this.cmdTest.UseVisualStyleBackColor = true;
         // 
         // cmdAddDependency
         // 
         this.cmdAddDependency.AutoSize = true;
         this.cmdAddDependency.Location = new System.Drawing.Point(96, 94);
         this.cmdAddDependency.Name = "cmdAddDependency";
         this.cmdAddDependency.Size = new System.Drawing.Size(34, 13);
         this.cmdAddDependency.TabIndex = 26;
         this.cmdAddDependency.TabStop = true;
         this.cmdAddDependency.Text = "add...";
         this.cmdAddDependency.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.cmdAddDependency_LinkClicked);
         // 
         // linkLabel5
         // 
         this.linkLabel5.AutoSize = true;
         this.linkLabel5.Enabled = false;
         this.linkLabel5.Location = new System.Drawing.Point(132, 94);
         this.linkLabel5.Name = "linkLabel5";
         this.linkLabel5.Size = new System.Drawing.Size(85, 13);
         this.linkLabel5.TabIndex = 27;
         this.linkLabel5.TabStop = true;
         this.linkLabel5.Text = "remove selected";
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.ClientSize = new System.Drawing.Size(603, 417);
         this.Controls.Add(this.linkLabel5);
         this.Controls.Add(this.cmdAddDependency);
         this.Controls.Add(this.cmdTest);
         this.Controls.Add(this.cmdCancel);
         this.Controls.Add(this.cmdSave);
         this.Controls.Add(this.txtVersion);
         this.Controls.Add(this.label6);
         this.Controls.Add(this.gridDependencies);
         this.Controls.Add(this.linkLabel3);
         this.Controls.Add(this.linkLabel2);
         this.Controls.Add(this.linkLabel1);
         this.Controls.Add(this.txtAuthor);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.txtProjectUrl);
         this.Controls.Add(this.cbPlatform);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.statusStrip1);
         this.Controls.Add(this.txtPackageId);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.Name = "MainForm";
         this.Text = "Pundit UI Helper [{0}]";
         ((System.ComponentModel.ISupportInitialize)(this.gridDependencies)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox txtPackageId;
      private System.Windows.Forms.StatusStrip statusStrip1;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.ComboBox cbPlatform;
      private System.Windows.Forms.TextBox txtProjectUrl;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.TextBox txtAuthor;
      private System.Windows.Forms.LinkLabel linkLabel1;
      private System.Windows.Forms.LinkLabel linkLabel2;
      private System.Windows.Forms.LinkLabel linkLabel3;
      private System.Windows.Forms.DataGridView gridDependencies;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.TextBox txtVersion;
      private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
      private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
      private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
      private System.Windows.Forms.DataGridViewCheckBoxColumn Column4;
      private System.Windows.Forms.Button cmdSave;
      private System.Windows.Forms.Button cmdCancel;
      private System.Windows.Forms.Button cmdTest;
      private System.Windows.Forms.LinkLabel cmdAddDependency;
      private System.Windows.Forms.LinkLabel linkLabel5;
   }
}

