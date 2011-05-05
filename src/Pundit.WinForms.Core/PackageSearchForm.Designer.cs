namespace Pundit.WinForms.Core
{
   partial class PackageSearchForm
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageSearchForm));
         this.gridResult = new System.Windows.Forms.DataGridView();
         this.PackageId = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Version = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Platform = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.txtText = new System.Windows.Forms.TextBox();
         this.cmdClose = new System.Windows.Forms.Button();
         this.cmdFind = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.gridResult)).BeginInit();
         this.SuspendLayout();
         // 
         // gridResult
         // 
         this.gridResult.AllowUserToAddRows = false;
         this.gridResult.AllowUserToDeleteRows = false;
         this.gridResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.gridResult.BackgroundColor = System.Drawing.SystemColors.ControlLight;
         this.gridResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.gridResult.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
         this.gridResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.gridResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PackageId,
            this.Version,
            this.Platform});
         this.gridResult.EnableHeadersVisualStyles = false;
         this.gridResult.Location = new System.Drawing.Point(3, 29);
         this.gridResult.Name = "gridResult";
         this.gridResult.RowHeadersVisible = false;
         this.gridResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.gridResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
         this.gridResult.Size = new System.Drawing.Size(546, 290);
         this.gridResult.TabIndex = 0;
         this.gridResult.DoubleClick += new System.EventHandler(this.gridResult_DoubleClick);
         // 
         // PackageId
         // 
         this.PackageId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
         this.PackageId.DataPropertyName = "PackageId";
         this.PackageId.HeaderText = "packageId";
         this.PackageId.Name = "PackageId";
         this.PackageId.ReadOnly = true;
         // 
         // Version
         // 
         this.Version.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
         this.Version.DataPropertyName = "Version";
         this.Version.HeaderText = "version";
         this.Version.Name = "Version";
         this.Version.ReadOnly = true;
         this.Version.Width = 64;
         // 
         // Platform
         // 
         this.Platform.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
         this.Platform.DataPropertyName = "Platform";
         this.Platform.HeaderText = "platform";
         this.Platform.Name = "Platform";
         this.Platform.ReadOnly = true;
         this.Platform.Width = 67;
         // 
         // txtText
         // 
         this.txtText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.txtText.Location = new System.Drawing.Point(3, 3);
         this.txtText.Name = "txtText";
         this.txtText.Size = new System.Drawing.Size(521, 20);
         this.txtText.TabIndex = 1;
         this.txtText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtText_KeyUp);
         // 
         // cmdClose
         // 
         this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdClose.Location = new System.Drawing.Point(474, 325);
         this.cmdClose.Name = "cmdClose";
         this.cmdClose.Size = new System.Drawing.Size(75, 23);
         this.cmdClose.TabIndex = 2;
         this.cmdClose.Text = "&Close";
         this.cmdClose.UseVisualStyleBackColor = true;
         this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
         // 
         // cmdFind
         // 
         this.cmdFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdFind.Image = ((System.Drawing.Image)(resources.GetObject("cmdFind.Image")));
         this.cmdFind.Location = new System.Drawing.Point(530, 2);
         this.cmdFind.Name = "cmdFind";
         this.cmdFind.Size = new System.Drawing.Size(23, 23);
         this.cmdFind.TabIndex = 3;
         this.cmdFind.UseVisualStyleBackColor = true;
         this.cmdFind.Click += new System.EventHandler(this.cmdFind_Click);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(0, 330);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(270, 13);
         this.label1.TabIndex = 4;
         this.label1.Text = "double-click a found package to add it to the manifest...";
         // 
         // PackageSearchForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(561, 355);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.cmdFind);
         this.Controls.Add(this.cmdClose);
         this.Controls.Add(this.txtText);
         this.Controls.Add(this.gridResult);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
         this.Name = "PackageSearchForm";
         this.Text = "Search package";
         ((System.ComponentModel.ISupportInitialize)(this.gridResult)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox txtText;
      private System.Windows.Forms.Button cmdClose;
      private System.Windows.Forms.Button cmdFind;
      private System.Windows.Forms.DataGridView gridResult;
      private System.Windows.Forms.DataGridViewTextBoxColumn PackageId;
      private System.Windows.Forms.DataGridViewTextBoxColumn Version;
      private System.Windows.Forms.DataGridViewTextBoxColumn Platform;
      private System.Windows.Forms.Label label1;
   }
}