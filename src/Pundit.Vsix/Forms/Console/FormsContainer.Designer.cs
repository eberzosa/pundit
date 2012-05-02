namespace Pundit.Vsix.Forms.Console
{
   partial class FormsContainer
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
         this.txtRich = new System.Windows.Forms.RichTextBox();
         this.SuspendLayout();
         // 
         // txtRich
         // 
         this.txtRich.BorderStyle = System.Windows.Forms.BorderStyle.None;
         this.txtRich.Dock = System.Windows.Forms.DockStyle.Fill;
         this.txtRich.Location = new System.Drawing.Point(0, 0);
         this.txtRich.Name = "txtRich";
         this.txtRich.Size = new System.Drawing.Size(600, 233);
         this.txtRich.TabIndex = 0;
         this.txtRich.Text = "";
         // 
         // FormsContainer
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.txtRich);
         this.Name = "FormsContainer";
         this.Size = new System.Drawing.Size(600, 233);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.RichTextBox txtRich;
   }
}
