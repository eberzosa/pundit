namespace Pundit.WinForms.App
{
   partial class MetadataForm
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
         this.ucMetadata = new Pundit.WinForms.Core.PackageMetadata();
         this.cmdOk = new System.Windows.Forms.Button();
         this.cmdCancel = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // ucMetadata
         // 
         this.ucMetadata.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.ucMetadata.Location = new System.Drawing.Point(-1, 2);
         this.ucMetadata.Name = "ucMetadata";
         this.ucMetadata.Package = null;
         this.ucMetadata.Size = new System.Drawing.Size(406, 233);
         this.ucMetadata.TabIndex = 0;
         this.ucMetadata.Load += new System.EventHandler(this.ucMetadata_Load);
         // 
         // cmdOk
         // 
         this.cmdOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdOk.Location = new System.Drawing.Point(234, 241);
         this.cmdOk.Name = "cmdOk";
         this.cmdOk.Size = new System.Drawing.Size(75, 23);
         this.cmdOk.TabIndex = 1;
         this.cmdOk.Text = "&Ok";
         this.cmdOk.UseVisualStyleBackColor = true;
         this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
         // 
         // cmdCancel
         // 
         this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdCancel.Location = new System.Drawing.Point(315, 241);
         this.cmdCancel.Name = "cmdCancel";
         this.cmdCancel.Size = new System.Drawing.Size(75, 23);
         this.cmdCancel.TabIndex = 2;
         this.cmdCancel.Text = "&Cancel";
         this.cmdCancel.UseVisualStyleBackColor = true;
         this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
         // 
         // MetadataForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(402, 276);
         this.Controls.Add(this.cmdCancel);
         this.Controls.Add(this.cmdOk);
         this.Controls.Add(this.ucMetadata);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
         this.Name = "MetadataForm";
         this.Text = "Package Metadata";
         this.ResumeLayout(false);

      }

      #endregion

      private Core.PackageMetadata ucMetadata;
      private System.Windows.Forms.Button cmdOk;
      private System.Windows.Forms.Button cmdCancel;
   }
}