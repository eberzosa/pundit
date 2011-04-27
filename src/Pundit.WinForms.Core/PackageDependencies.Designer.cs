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
         this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Column4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
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
         this.gridDependencies.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
         this.gridDependencies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.gridDependencies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
         this.gridDependencies.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
         this.gridDependencies.EnableHeadersVisualStyles = false;
         this.gridDependencies.Location = new System.Drawing.Point(0, 0);
         this.gridDependencies.Margin = new System.Windows.Forms.Padding(0);
         this.gridDependencies.Name = "gridDependencies";
         this.gridDependencies.ReadOnly = true;
         this.gridDependencies.RowHeadersVisible = false;
         this.gridDependencies.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.gridDependencies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
         this.gridDependencies.Size = new System.Drawing.Size(530, 207);
         this.gridDependencies.TabIndex = 28;
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
         this.cmdRemove.Image = ((System.Drawing.Image)(resources.GetObject("cmdRemove.Image")));
         this.cmdRemove.Location = new System.Drawing.Point(533, 32);
         this.cmdRemove.Name = "cmdRemove";
         this.cmdRemove.Size = new System.Drawing.Size(23, 23);
         this.cmdRemove.TabIndex = 30;
         this.cmdRemove.UseVisualStyleBackColor = true;
         // 
         // Column1
         // 
         this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
         this.Column1.DataPropertyName = "PackageId";
         this.Column1.HeaderText = "packageId";
         this.Column1.Name = "Column1";
         this.Column1.ReadOnly = true;
         // 
         // Column2
         // 
         this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
         this.Column2.DataPropertyName = "Version";
         this.Column2.HeaderText = "version";
         this.Column2.Name = "Column2";
         this.Column2.ReadOnly = true;
         this.Column2.Width = 64;
         // 
         // Column3
         // 
         this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
         this.Column3.DataPropertyName = "Platform";
         this.Column3.HeaderText = "platform";
         this.Column3.Name = "Column3";
         this.Column3.ReadOnly = true;
         this.Column3.Width = 67;
         // 
         // Column4
         // 
         this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
         this.Column4.DataPropertyName = "DevTime";
         this.Column4.HeaderText = "devtime";
         this.Column4.Name = "Column4";
         this.Column4.ReadOnly = true;
         this.Column4.Width = 48;
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
      private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
      private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
      private System.Windows.Forms.DataGridViewCheckBoxColumn Column4;
   }
}
