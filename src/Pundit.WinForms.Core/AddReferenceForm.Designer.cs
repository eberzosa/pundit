namespace Pundit.WinForms.Core
{
   partial class AddReferenceForm
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
         System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
         System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
         System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
         this.label1 = new System.Windows.Forms.Label();
         this.txtRoot = new System.Windows.Forms.TextBox();
         this.gridReferences = new System.Windows.Forms.DataGridView();
         this.ComponentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Version = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Runtime = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Path = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.cmdOk = new System.Windows.Forms.Button();
         this.cmdCancel = new System.Windows.Forms.Button();
         ((System.ComponentModel.ISupportInitialize)(this.gridReferences)).BeginInit();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(9, 10);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(70, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "Binaries from:";
         // 
         // txtRoot
         // 
         this.txtRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.txtRoot.Location = new System.Drawing.Point(85, 7);
         this.txtRoot.Name = "txtRoot";
         this.txtRoot.ReadOnly = true;
         this.txtRoot.Size = new System.Drawing.Size(513, 20);
         this.txtRoot.TabIndex = 1;
         // 
         // gridReferences
         // 
         this.gridReferences.AllowUserToAddRows = false;
         this.gridReferences.AllowUserToDeleteRows = false;
         this.gridReferences.AllowUserToResizeRows = false;
         this.gridReferences.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.gridReferences.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
         dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
         dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
         dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
         dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
         dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
         dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
         this.gridReferences.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
         this.gridReferences.ColumnHeadersHeight = 28;
         this.gridReferences.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
         this.gridReferences.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ComponentName,
            this.Version,
            this.Runtime,
            this.Path});
         this.gridReferences.Location = new System.Drawing.Point(12, 41);
         this.gridReferences.Name = "gridReferences";
         this.gridReferences.ReadOnly = true;
         dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
         dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
         dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
         dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
         dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
         dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
         this.gridReferences.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
         this.gridReferences.RowHeadersVisible = false;
         dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
         this.gridReferences.RowsDefaultCellStyle = dataGridViewCellStyle3;
         this.gridReferences.ShowEditingIcon = false;
         this.gridReferences.Size = new System.Drawing.Size(579, 269);
         this.gridReferences.TabIndex = 2;
         // 
         // ComponentName
         // 
         this.ComponentName.HeaderText = "Component Name";
         this.ComponentName.Name = "ComponentName";
         this.ComponentName.ReadOnly = true;
         this.ComponentName.Width = 104;
         // 
         // Version
         // 
         this.Version.HeaderText = "Version";
         this.Version.Name = "Version";
         this.Version.ReadOnly = true;
         this.Version.Width = 103;
         // 
         // Runtime
         // 
         this.Runtime.HeaderText = "Runtime";
         this.Runtime.Name = "Runtime";
         this.Runtime.ReadOnly = true;
         this.Runtime.Width = 104;
         // 
         // Path
         // 
         this.Path.HeaderText = "Path";
         this.Path.Name = "Path";
         this.Path.ReadOnly = true;
         this.Path.Width = 103;
         // 
         // cmdOk
         // 
         this.cmdOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdOk.Location = new System.Drawing.Point(434, 316);
         this.cmdOk.Name = "cmdOk";
         this.cmdOk.Size = new System.Drawing.Size(75, 23);
         this.cmdOk.TabIndex = 3;
         this.cmdOk.Text = "&Ok";
         this.cmdOk.UseVisualStyleBackColor = true;
         // 
         // cmdCancel
         // 
         this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdCancel.Location = new System.Drawing.Point(515, 316);
         this.cmdCancel.Name = "cmdCancel";
         this.cmdCancel.Size = new System.Drawing.Size(75, 23);
         this.cmdCancel.TabIndex = 4;
         this.cmdCancel.Text = "&Cancel";
         this.cmdCancel.UseVisualStyleBackColor = true;
         // 
         // AddReferenceForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(603, 351);
         this.Controls.Add(this.cmdCancel);
         this.Controls.Add(this.cmdOk);
         this.Controls.Add(this.gridReferences);
         this.Controls.Add(this.txtRoot);
         this.Controls.Add(this.label1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
         this.Name = "AddReferenceForm";
         this.Text = "Add Pundit Reference...";
         ((System.ComponentModel.ISupportInitialize)(this.gridReferences)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox txtRoot;
      private System.Windows.Forms.DataGridView gridReferences;
      private System.Windows.Forms.Button cmdOk;
      private System.Windows.Forms.Button cmdCancel;
      private System.Windows.Forms.DataGridViewTextBoxColumn ComponentName;
      private System.Windows.Forms.DataGridViewTextBoxColumn Version;
      private System.Windows.Forms.DataGridViewTextBoxColumn Runtime;
      private System.Windows.Forms.DataGridViewTextBoxColumn Path;
   }
}