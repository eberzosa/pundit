namespace Pundit.Vsix.Forms
{
   partial class Integration
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
         this.chkIntegrateIntoTortoise = new System.Windows.Forms.CheckBox();
         this.chkTortoiseWatchUpdate = new System.Windows.Forms.CheckBox();
         this.chkVSXSD = new System.Windows.Forms.CheckBox();
         this.SuspendLayout();
         // 
         // chkIntegrateIntoTortoise
         // 
         this.chkIntegrateIntoTortoise.AutoSize = true;
         this.chkIntegrateIntoTortoise.Location = new System.Drawing.Point(4, 4);
         this.chkIntegrateIntoTortoise.Name = "chkIntegrateIntoTortoise";
         this.chkIntegrateIntoTortoise.Size = new System.Drawing.Size(143, 17);
         this.chkIntegrateIntoTortoise.TabIndex = 0;
         this.chkIntegrateIntoTortoise.Text = "integrate into Subversion";
         this.chkIntegrateIntoTortoise.UseVisualStyleBackColor = true;
         // 
         // chkTortoiseWatchUpdate
         // 
         this.chkTortoiseWatchUpdate.AutoSize = true;
         this.chkTortoiseWatchUpdate.Location = new System.Drawing.Point(20, 28);
         this.chkTortoiseWatchUpdate.Name = "chkTortoiseWatchUpdate";
         this.chkTortoiseWatchUpdate.Size = new System.Drawing.Size(294, 17);
         this.chkTortoiseWatchUpdate.TabIndex = 1;
         this.chkTortoiseWatchUpdate.Text = "refresh Pundit dependencies when I update source code";
         this.chkTortoiseWatchUpdate.UseVisualStyleBackColor = true;
         // 
         // chkVSXSD
         // 
         this.chkVSXSD.AutoSize = true;
         this.chkVSXSD.Location = new System.Drawing.Point(4, 52);
         this.chkVSXSD.Name = "chkVSXSD";
         this.chkVSXSD.Size = new System.Drawing.Size(229, 17);
         this.chkVSXSD.TabIndex = 2;
         this.chkVSXSD.Text = "add IntelliSense for pundit.xml manifest files";
         this.chkVSXSD.UseVisualStyleBackColor = true;
         // 
         // Integration
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.chkVSXSD);
         this.Controls.Add(this.chkTortoiseWatchUpdate);
         this.Controls.Add(this.chkIntegrateIntoTortoise);
         this.Name = "Integration";
         this.Size = new System.Drawing.Size(647, 302);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.CheckBox chkIntegrateIntoTortoise;
      private System.Windows.Forms.CheckBox chkTortoiseWatchUpdate;
      private System.Windows.Forms.CheckBox chkVSXSD;
   }
}
