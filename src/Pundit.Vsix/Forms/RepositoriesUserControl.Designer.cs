namespace Pundit.Vsix.Forms
{
   partial class RepositoriesUserControl
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
         this.globalSettingsControl1 = new Pundit.WinForms.Core.GlobalSettingsControl();
         this.SuspendLayout();
         // 
         // globalSettingsControl1
         // 
         this.globalSettingsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.globalSettingsControl1.Location = new System.Drawing.Point(0, 0);
         this.globalSettingsControl1.Name = "globalSettingsControl1";
         this.globalSettingsControl1.Size = new System.Drawing.Size(621, 276);
         this.globalSettingsControl1.TabIndex = 0;
         // 
         // RepositoriesUserControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.globalSettingsControl1);
         this.Name = "RepositoriesUserControl";
         this.Size = new System.Drawing.Size(621, 276);
         this.ResumeLayout(false);

      }

      #endregion

      private WinForms.Core.GlobalSettingsControl globalSettingsControl1;

   }
}
