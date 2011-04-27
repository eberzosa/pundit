namespace Pundit.WinForms.Core
{
   partial class PackageFiles
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
         this.cbConfiguration = new System.Windows.Forms.ComboBox();
         this.label1 = new System.Windows.Forms.Label();
         this.gridFiles = new System.Windows.Forms.DataGridView();
         this.cmdTest = new System.Windows.Forms.Button();
         ((System.ComponentModel.ISupportInitialize)(this.gridFiles)).BeginInit();
         this.SuspendLayout();
         // 
         // cbConfiguration
         // 
         this.cbConfiguration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cbConfiguration.FormattingEnabled = true;
         this.cbConfiguration.Items.AddRange(new object[] {
            "<All>",
            "Debug",
            "Release"});
         this.cbConfiguration.Location = new System.Drawing.Point(77, 2);
         this.cbConfiguration.Name = "cbConfiguration";
         this.cbConfiguration.Size = new System.Drawing.Size(171, 21);
         this.cbConfiguration.TabIndex = 0;
         this.cbConfiguration.SelectedIndexChanged += new System.EventHandler(this.cbConfiguration_SelectedIndexChanged);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(-1, 5);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(72, 13);
         this.label1.TabIndex = 1;
         this.label1.Text = "Configuration:";
         // 
         // gridFiles
         // 
         this.gridFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.gridFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.gridFiles.Location = new System.Drawing.Point(0, 29);
         this.gridFiles.Name = "gridFiles";
         this.gridFiles.Size = new System.Drawing.Size(523, 233);
         this.gridFiles.TabIndex = 2;
         // 
         // cmdTest
         // 
         this.cmdTest.Location = new System.Drawing.Point(254, 0);
         this.cmdTest.Name = "cmdTest";
         this.cmdTest.Size = new System.Drawing.Size(75, 23);
         this.cmdTest.TabIndex = 3;
         this.cmdTest.Text = "&Test...";
         this.cmdTest.UseVisualStyleBackColor = true;
         // 
         // PackageFiles
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.cmdTest);
         this.Controls.Add(this.gridFiles);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.cbConfiguration);
         this.Name = "PackageFiles";
         this.Size = new System.Drawing.Size(526, 265);
         ((System.ComponentModel.ISupportInitialize)(this.gridFiles)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ComboBox cbConfiguration;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.DataGridView gridFiles;
      private System.Windows.Forms.Button cmdTest;
   }
}
