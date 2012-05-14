namespace Pundit.Vsix.Forms
{
   partial class PageFooter
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
         this.lblVersion = new System.Windows.Forms.Label();
         this.lnkDocs = new System.Windows.Forms.LinkLabel();
         this.lnkLicense = new System.Windows.Forms.LinkLabel();
         this.SuspendLayout();
         // 
         // lblVersion
         // 
         this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.lblVersion.AutoSize = true;
         this.lblVersion.Enabled = false;
         this.lblVersion.ForeColor = System.Drawing.SystemColors.ActiveCaption;
         this.lblVersion.Location = new System.Drawing.Point(2, 7);
         this.lblVersion.Name = "lblVersion";
         this.lblVersion.Size = new System.Drawing.Size(151, 13);
         this.lblVersion.TabIndex = 8;
         this.lblVersion.Text = "Pundit {0} open-source project";
         // 
         // lnkDocs
         // 
         this.lnkDocs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.lnkDocs.AutoSize = true;
         this.lnkDocs.Location = new System.Drawing.Point(385, 7);
         this.lnkDocs.Name = "lnkDocs";
         this.lnkDocs.Size = new System.Drawing.Size(77, 13);
         this.lnkDocs.TabIndex = 11;
         this.lnkDocs.TabStop = true;
         this.lnkDocs.Text = "documentation";
         this.lnkDocs.Click += new System.EventHandler(this.lnkDocs_Click);
         // 
         // lnkLicense
         // 
         this.lnkLicense.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.lnkLicense.AutoSize = true;
         this.lnkLicense.Location = new System.Drawing.Point(462, 7);
         this.lnkLicense.Name = "lnkLicense";
         this.lnkLicense.Size = new System.Drawing.Size(40, 13);
         this.lnkLicense.TabIndex = 10;
         this.lnkLicense.TabStop = true;
         this.lnkLicense.Text = "license";
         this.lnkLicense.Click += new System.EventHandler(this.lnkLicense_Click);
         // 
         // PageFooter
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.lnkDocs);
         this.Controls.Add(this.lnkLicense);
         this.Controls.Add(this.lblVersion);
         this.Name = "PageFooter";
         this.Size = new System.Drawing.Size(505, 24);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label lblVersion;
      private System.Windows.Forms.LinkLabel lnkDocs;
      private System.Windows.Forms.LinkLabel lnkLicense;
   }
}
