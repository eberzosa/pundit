namespace Pundit.WinForms.Core
{
   partial class EditDependenciesForm
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditDependenciesForm));
         this.ucD = new Pundit.WinForms.Core.PackageDependencies();
         this.cmdOk = new System.Windows.Forms.Button();
         this.cmdCancel = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // ucD
         // 
         this.ucD.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.ucD.Dependencies = ((System.Collections.Generic.IEnumerable<Pundit.Core.Model.PackageDependency>)(resources.GetObject("ucD.Dependencies")));
         this.ucD.Location = new System.Drawing.Point(2, 3);
         this.ucD.Name = "ucD";
         this.ucD.Size = new System.Drawing.Size(786, 262);
         this.ucD.TabIndex = 0;
         // 
         // cmdOk
         // 
         this.cmdOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.cmdOk.Location = new System.Drawing.Point(619, 271);
         this.cmdOk.Name = "cmdOk";
         this.cmdOk.Size = new System.Drawing.Size(75, 23);
         this.cmdOk.TabIndex = 1;
         this.cmdOk.Text = "&OK";
         this.cmdOk.UseVisualStyleBackColor = true;
         this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
         // 
         // cmdCancel
         // 
         this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.cmdCancel.Location = new System.Drawing.Point(700, 271);
         this.cmdCancel.Name = "cmdCancel";
         this.cmdCancel.Size = new System.Drawing.Size(75, 23);
         this.cmdCancel.TabIndex = 2;
         this.cmdCancel.Text = "&Cancel";
         this.cmdCancel.UseVisualStyleBackColor = true;
         this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
         // 
         // EditDependenciesForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(787, 311);
         this.Controls.Add(this.cmdCancel);
         this.Controls.Add(this.cmdOk);
         this.Controls.Add(this.ucD);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
         this.Name = "EditDependenciesForm";
         this.Text = "Solution dependencies";
         this.ResumeLayout(false);

      }

      #endregion

      private PackageDependencies ucD;
      private System.Windows.Forms.Button cmdOk;
      private System.Windows.Forms.Button cmdCancel;
   }
}