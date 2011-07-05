namespace Pundit.WinForms.Core
{
   partial class PackageDependencies
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

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageDependencies));
         this.gridDependencies = new System.Windows.Forms.DataGridView();
         this.cmdAdd = new System.Windows.Forms.Button();
         this.cmdRemove = new System.Windows.Forms.Button();
         this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Version = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Platform = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Scope = new System.Windows.Forms.DataGridViewComboBoxColumn();
         this.PlatformFolder = new System.Windows.Forms.DataGridViewCheckBoxColumn();
         ((System.ComponentModel.ISupportInitialize)(this.gridDependencies)).BeginInit();
         this.SuspendLayout();
         // 
         // gridDependencies
         // 
         this.gridDependencies.AllowUserToAddRows = false;
         this.gridDependencies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.gridDependencies.BackgroundColor = System.Drawing.SystemColors.ControlLight;
         this.gridDependencies.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.gridDependencies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.gridDependencies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Version,
            this.Platform,
            this.Scope,
            this.PlatformFolder});
         this.gridDependencies.Location = new System.Drawing.Point(0, 0);
         this.gridDependencies.Margin = new System.Windows.Forms.Padding(0);
         this.gridDependencies.MultiSelect = false;
         this.gridDependencies.Name = "gridDependencies";
         this.gridDependencies.RowHeadersVisible = false;
         this.gridDependencies.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.gridDependencies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
         this.gridDependencies.Size = new System.Drawing.Size(530, 207);
         this.gridDependencies.TabIndex = 28;
         this.gridDependencies.SelectionChanged += new System.EventHandler(this.gridDependencies_SelectionChanged);
         // 
         // cmdAdd
         // 
         this.cmdAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdAdd.Image = ((System.Drawing.Image)(resources.GetObject("cmdAdd.Image")));
         this.cmdAdd.Location = new System.Drawing.Point(533, 3);
         this.cmdAdd.Name = "cmdAdd";
         this.cmdAdd.Size = new System.Drawing.Size(23, 23);
         this.cmdAdd.TabIndex = 29;
         this.cmdAdd.UseVisualStyleBackColor = true;
         this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
         // 
         // cmdRemove
         // 
         this.cmdRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdRemove.Enabled = false;
         this.cmdRemove.Image = ((System.Drawing.Image)(resources.GetObject("cmdRemove.Image")));
         this.cmdRemove.Location = new System.Drawing.Point(533, 32);
         this.cmdRemove.Name = "cmdRemove";
         this.cmdRemove.Size = new System.Drawing.Size(23, 23);
         this.cmdRemove.TabIndex = 30;
         this.cmdRemove.UseVisualStyleBackColor = true;
         this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
         // 
         // Column1
         // 
         this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
         this.Column1.DataPropertyName = "PackageId";
         this.Column1.HeaderText = "Package ID";
         this.Column1.Name = "Column1";
         // 
         // Version
         // 
         this.Version.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
         this.Version.DataPropertyName = "VersionPattern";
         this.Version.HeaderText = "Version";
         this.Version.Name = "Version";
         this.Version.Width = 65;
         // 
         // Platform
         // 
         this.Platform.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
         this.Platform.DataPropertyName = "Platform";
         this.Platform.HeaderText = "Platform";
         this.Platform.Name = "Platform";
         this.Platform.ReadOnly = true;
         this.Platform.Width = 68;
         // 
         // Scope
         // 
         this.Scope.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
         this.Scope.DataPropertyName = "Scope";
         this.Scope.HeaderText = "Scope";
         this.Scope.MinimumWidth = 100;
         this.Scope.Name = "Scope";
         // 
         // PlatformFolder
         // 
         this.PlatformFolder.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
         this.PlatformFolder.DataPropertyName = "CreatePlatformFolder";
         this.PlatformFolder.HeaderText = "Platform Subfolder";
         this.PlatformFolder.Name = "PlatformFolder";
         this.PlatformFolder.Width = 97;
         // 
         // PackageDependencies
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.cmdRemove);
         this.Controls.Add(this.cmdAdd);
         this.Controls.Add(this.gridDependencies);
         this.Name = "PackageDependencies";
         this.Size = new System.Drawing.Size(560, 207);
         ((System.ComponentModel.ISupportInitialize)(this.gridDependencies)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.DataGridView gridDependencies;
      private System.Windows.Forms.Button cmdAdd;
      private System.Windows.Forms.Button cmdRemove;
      private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
      private System.Windows.Forms.DataGridViewTextBoxColumn Version;
      private System.Windows.Forms.DataGridViewTextBoxColumn Platform;
      private System.Windows.Forms.DataGridViewComboBoxColumn Scope;
      private System.Windows.Forms.DataGridViewCheckBoxColumn PlatformFolder;
   }
}
