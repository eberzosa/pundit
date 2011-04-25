namespace Pundit.WinForms.Core
{
   partial class PackageSearchForm
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
         this.gridResult = new System.Windows.Forms.DataGridView();
         this.txtText = new System.Windows.Forms.TextBox();
         this.cmdClose = new System.Windows.Forms.Button();
         ((System.ComponentModel.ISupportInitialize)(this.gridResult)).BeginInit();
         this.SuspendLayout();
         // 
         // gridResult
         // 
         this.gridResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.gridResult.Location = new System.Drawing.Point(3, 29);
         this.gridResult.Name = "gridResult";
         this.gridResult.Size = new System.Drawing.Size(546, 290);
         this.gridResult.TabIndex = 0;
         // 
         // txtText
         // 
         this.txtText.Location = new System.Drawing.Point(3, 3);
         this.txtText.Name = "txtText";
         this.txtText.Size = new System.Drawing.Size(546, 20);
         this.txtText.TabIndex = 1;
         this.txtText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtText_KeyUp);
         // 
         // cmdClose
         // 
         this.cmdClose.Location = new System.Drawing.Point(474, 325);
         this.cmdClose.Name = "cmdClose";
         this.cmdClose.Size = new System.Drawing.Size(75, 23);
         this.cmdClose.TabIndex = 2;
         this.cmdClose.Text = "&Close";
         this.cmdClose.UseVisualStyleBackColor = true;
         // 
         // PackageSearchForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(561, 355);
         this.Controls.Add(this.cmdClose);
         this.Controls.Add(this.txtText);
         this.Controls.Add(this.gridResult);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
         this.Name = "PackageSearchForm";
         this.Text = "Search Package";
         ((System.ComponentModel.ISupportInitialize)(this.gridResult)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.DataGridView gridResult;
      private System.Windows.Forms.TextBox txtText;
      private System.Windows.Forms.Button cmdClose;
   }
}