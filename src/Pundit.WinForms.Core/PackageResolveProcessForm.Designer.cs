namespace Pundit.WinForms.Core
{
   partial class PackageResolveProcessForm
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
         this.lblProgress = new System.Windows.Forms.Label();
         this.cmdOk = new System.Windows.Forms.Button();
         this.cmdCancel = new System.Windows.Forms.Button();
         this.gridPackages = new System.Windows.Forms.DataGridView();
         this.imgProgress = new System.Windows.Forms.PictureBox();
         this.StatusImage = new System.Windows.Forms.DataGridViewImageColumn();
         this.PackageId = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Version = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
         ((System.ComponentModel.ISupportInitialize)(this.gridPackages)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.imgProgress)).BeginInit();
         this.SuspendLayout();
         // 
         // lblProgress
         // 
         this.lblProgress.AutoSize = true;
         this.lblProgress.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblProgress.Location = new System.Drawing.Point(50, 18);
         this.lblProgress.Name = "lblProgress";
         this.lblProgress.Size = new System.Drawing.Size(122, 16);
         this.lblProgress.TabIndex = 1;
         this.lblProgress.Text = "pushing electrons...";
         // 
         // cmdOk
         // 
         this.cmdOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.cmdOk.Enabled = false;
         this.cmdOk.Location = new System.Drawing.Point(405, 309);
         this.cmdOk.Name = "cmdOk";
         this.cmdOk.Size = new System.Drawing.Size(75, 23);
         this.cmdOk.TabIndex = 2;
         this.cmdOk.Text = "&OK";
         this.cmdOk.UseVisualStyleBackColor = true;
         // 
         // cmdCancel
         // 
         this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.cmdCancel.Location = new System.Drawing.Point(486, 309);
         this.cmdCancel.Name = "cmdCancel";
         this.cmdCancel.Size = new System.Drawing.Size(75, 23);
         this.cmdCancel.TabIndex = 3;
         this.cmdCancel.Text = "&Cancel";
         this.cmdCancel.UseVisualStyleBackColor = true;
         this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
         // 
         // gridPackages
         // 
         this.gridPackages.AllowUserToAddRows = false;
         this.gridPackages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.gridPackages.BackgroundColor = System.Drawing.SystemColors.Control;
         this.gridPackages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.gridPackages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StatusImage,
            this.PackageId,
            this.Version,
            this.Status});
         this.gridPackages.Location = new System.Drawing.Point(12, 49);
         this.gridPackages.Name = "gridPackages";
         this.gridPackages.ReadOnly = true;
         this.gridPackages.RowHeadersVisible = false;
         this.gridPackages.Size = new System.Drawing.Size(549, 254);
         this.gridPackages.TabIndex = 4;
         // 
         // imgProgress
         // 
         this.imgProgress.Image = global::Pundit.WinForms.Core.Images.ajax_loader;
         this.imgProgress.Location = new System.Drawing.Point(12, 12);
         this.imgProgress.Name = "imgProgress";
         this.imgProgress.Size = new System.Drawing.Size(31, 31);
         this.imgProgress.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
         this.imgProgress.TabIndex = 0;
         this.imgProgress.TabStop = false;
         // 
         // StatusImage
         // 
         this.StatusImage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
         this.StatusImage.HeaderText = "";
         this.StatusImage.Name = "StatusImage";
         this.StatusImage.ReadOnly = true;
         this.StatusImage.Width = 7;
         // 
         // PackageId
         // 
         this.PackageId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
         this.PackageId.HeaderText = "Package ID";
         this.PackageId.Name = "PackageId";
         this.PackageId.ReadOnly = true;
         // 
         // Version
         // 
         this.Version.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
         this.Version.HeaderText = "Version";
         this.Version.Name = "Version";
         this.Version.ReadOnly = true;
         this.Version.Width = 71;
         // 
         // Status
         // 
         this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
         this.Status.HeaderText = "Status";
         this.Status.Name = "Status";
         this.Status.ReadOnly = true;
         this.Status.Width = 66;
         // 
         // PackageResolveProcessForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(573, 344);
         this.Controls.Add(this.gridPackages);
         this.Controls.Add(this.cmdCancel);
         this.Controls.Add(this.cmdOk);
         this.Controls.Add(this.lblProgress);
         this.Controls.Add(this.imgProgress);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
         this.Name = "PackageResolveProcessForm";
         this.Text = "Dependency resolution";
         ((System.ComponentModel.ISupportInitialize)(this.gridPackages)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.imgProgress)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.PictureBox imgProgress;
      private System.Windows.Forms.Label lblProgress;
      private System.Windows.Forms.Button cmdOk;
      private System.Windows.Forms.Button cmdCancel;
      private System.Windows.Forms.DataGridView gridPackages;
      private System.Windows.Forms.DataGridViewImageColumn StatusImage;
      private System.Windows.Forms.DataGridViewTextBoxColumn PackageId;
      private System.Windows.Forms.DataGridViewTextBoxColumn Version;
      private System.Windows.Forms.DataGridViewTextBoxColumn Status;
   }
}