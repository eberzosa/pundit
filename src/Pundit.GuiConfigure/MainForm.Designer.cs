namespace Pundit.WinForms.App
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
         this.statusStrip1 = new System.Windows.Forms.StatusStrip();
         this.cmdSave = new System.Windows.Forms.Button();
         this.cmdCancel = new System.Windows.Forms.Button();
         this.cmdMetadata = new System.Windows.Forms.Button();
         this.packageDependencies1 = new Pundit.WinForms.Core.PackageDependencies();
         this.label1 = new System.Windows.Forms.Label();
         this.cmdGlobalSettings = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // statusStrip1
         // 
         this.statusStrip1.Location = new System.Drawing.Point(0, 304);
         this.statusStrip1.Name = "statusStrip1";
         this.statusStrip1.Size = new System.Drawing.Size(460, 22);
         this.statusStrip1.TabIndex = 7;
         this.statusStrip1.Text = "statusStrip1";
         // 
         // cmdSave
         // 
         this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdSave.Location = new System.Drawing.Point(289, 278);
         this.cmdSave.Name = "cmdSave";
         this.cmdSave.Size = new System.Drawing.Size(75, 23);
         this.cmdSave.TabIndex = 23;
         this.cmdSave.Text = "&Save";
         this.cmdSave.UseVisualStyleBackColor = true;
         this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
         // 
         // cmdCancel
         // 
         this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.cmdCancel.Location = new System.Drawing.Point(370, 278);
         this.cmdCancel.Name = "cmdCancel";
         this.cmdCancel.Size = new System.Drawing.Size(75, 23);
         this.cmdCancel.TabIndex = 24;
         this.cmdCancel.Text = "&Cancel";
         this.cmdCancel.UseVisualStyleBackColor = true;
         this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
         // 
         // cmdMetadata
         // 
         this.cmdMetadata.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.cmdMetadata.Location = new System.Drawing.Point(12, 278);
         this.cmdMetadata.Name = "cmdMetadata";
         this.cmdMetadata.Size = new System.Drawing.Size(75, 23);
         this.cmdMetadata.TabIndex = 26;
         this.cmdMetadata.Text = "&Metadata...";
         this.cmdMetadata.UseVisualStyleBackColor = true;
         this.cmdMetadata.Click += new System.EventHandler(this.cmdMetadata_Click);
         // 
         // packageDependencies1
         // 
         this.packageDependencies1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.packageDependencies1.Location = new System.Drawing.Point(12, 30);
         this.packageDependencies1.Name = "packageDependencies1";
         this.packageDependencies1.Size = new System.Drawing.Size(448, 242);
         this.packageDependencies1.TabIndex = 25;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(13, 11);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(65, 13);
         this.label1.TabIndex = 27;
         this.label1.Text = "References:";
         // 
         // cmdGlobalSettings
         // 
         this.cmdGlobalSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.cmdGlobalSettings.Location = new System.Drawing.Point(93, 278);
         this.cmdGlobalSettings.Name = "cmdGlobalSettings";
         this.cmdGlobalSettings.Size = new System.Drawing.Size(107, 23);
         this.cmdGlobalSettings.TabIndex = 28;
         this.cmdGlobalSettings.Text = "&Global settings...";
         this.cmdGlobalSettings.UseVisualStyleBackColor = true;
         this.cmdGlobalSettings.Click += new System.EventHandler(this.cmdGlobalSettings_Click);
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(460, 326);
         this.Controls.Add(this.cmdGlobalSettings);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.cmdMetadata);
         this.Controls.Add(this.packageDependencies1);
         this.Controls.Add(this.cmdCancel);
         this.Controls.Add(this.cmdSave);
         this.Controls.Add(this.statusStrip1);
         this.Name = "MainForm";
         this.Text = "Pundit GUI [{0}]";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.StatusStrip statusStrip1;
      private System.Windows.Forms.Button cmdSave;
      private System.Windows.Forms.Button cmdCancel;
      private WinForms.Core.PackageDependencies packageDependencies1;
      private System.Windows.Forms.Button cmdMetadata;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Button cmdGlobalSettings;
   }
}

