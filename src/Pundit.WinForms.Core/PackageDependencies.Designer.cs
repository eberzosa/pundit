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
         this.gridDependencies = new System.Windows.Forms.DataGridView();
         this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Column4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
         this.button1 = new System.Windows.Forms.Button();
         this.button2 = new System.Windows.Forms.Button();
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
         // button1
         // 
         this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.button1.Location = new System.Drawing.Point(533, 3);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(23, 23);
         this.button1.TabIndex = 29;
         this.button1.Text = "+";
         this.button1.UseVisualStyleBackColor = true;
         // 
         // button2
         // 
         this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.button2.Location = new System.Drawing.Point(533, 32);
         this.button2.Name = "button2";
         this.button2.Size = new System.Drawing.Size(23, 23);
         this.button2.TabIndex = 30;
         this.button2.Text = "-";
         this.button2.UseVisualStyleBackColor = true;
         // 
         // PackageDependencies
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.button2);
         this.Controls.Add(this.button1);
         this.Controls.Add(this.gridDependencies);
         this.Name = "PackageDependencies";
         this.Size = new System.Drawing.Size(560, 207);
         ((System.ComponentModel.ISupportInitialize)(this.gridDependencies)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.DataGridView gridDependencies;
      private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
      private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
      private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
      private System.Windows.Forms.DataGridViewCheckBoxColumn Column4;
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.Button button2;
   }
}
