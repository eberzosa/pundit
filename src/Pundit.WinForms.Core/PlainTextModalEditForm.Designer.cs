namespace Pundit.WinForms.Core
{
   partial class PlainTextModalEditForm
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
         this.textMultiText = new System.Windows.Forms.TextBox();
         this.cmdSave = new System.Windows.Forms.Button();
         this.cmdCancel = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // textMultiText
         // 
         this.textMultiText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.textMultiText.Location = new System.Drawing.Point(1, 4);
         this.textMultiText.Multiline = true;
         this.textMultiText.Name = "textMultiText";
         this.textMultiText.Size = new System.Drawing.Size(462, 315);
         this.textMultiText.TabIndex = 0;
         // 
         // cmdSave
         // 
         this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdSave.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.cmdSave.Location = new System.Drawing.Point(307, 325);
         this.cmdSave.Name = "cmdSave";
         this.cmdSave.Size = new System.Drawing.Size(75, 23);
         this.cmdSave.TabIndex = 1;
         this.cmdSave.Text = "&Save";
         this.cmdSave.UseVisualStyleBackColor = true;
         this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
         // 
         // cmdCancel
         // 
         this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.cmdCancel.Location = new System.Drawing.Point(388, 325);
         this.cmdCancel.Name = "cmdCancel";
         this.cmdCancel.Size = new System.Drawing.Size(75, 23);
         this.cmdCancel.TabIndex = 2;
         this.cmdCancel.Text = "&Cancel";
         this.cmdCancel.UseVisualStyleBackColor = true;
         this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
         // 
         // PlainTextModalEditForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(466, 350);
         this.Controls.Add(this.cmdCancel);
         this.Controls.Add(this.cmdSave);
         this.Controls.Add(this.textMultiText);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
         this.Name = "PlainTextModalEditForm";
         this.Text = "PlainTextModalEditForm";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox textMultiText;
      private System.Windows.Forms.Button cmdSave;
      private System.Windows.Forms.Button cmdCancel;
   }
}