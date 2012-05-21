namespace Pundit.WinForms.Core.Forms
{
   partial class AddManifestForm
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
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.txtPackageId = new System.Windows.Forms.TextBox();
         this.cbPlatform = new System.Windows.Forms.ComboBox();
         this.cmdOk = new System.Windows.Forms.Button();
         this.cmdCancel = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(14, 8);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(61, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "packageId:";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(28, 35);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(47, 13);
         this.label2.TabIndex = 1;
         this.label2.Text = "platform:";
         // 
         // txtPackageId
         // 
         this.txtPackageId.Location = new System.Drawing.Point(81, 5);
         this.txtPackageId.Name = "txtPackageId";
         this.txtPackageId.Size = new System.Drawing.Size(170, 20);
         this.txtPackageId.TabIndex = 2;
         this.txtPackageId.TextChanged += new System.EventHandler(this.PackageIdTextChanged);
         this.txtPackageId.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPackageId_KeyUp);
         // 
         // cbPlatform
         // 
         this.cbPlatform.FormattingEnabled = true;
         this.cbPlatform.Items.AddRange(new object[] {
            "noarch",
            "net20",
            "net35",
            "net40"});
         this.cbPlatform.Location = new System.Drawing.Point(81, 32);
         this.cbPlatform.Name = "cbPlatform";
         this.cbPlatform.Size = new System.Drawing.Size(170, 21);
         this.cbPlatform.TabIndex = 3;
         this.cbPlatform.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbPlatform_KeyUp);
         // 
         // cmdOk
         // 
         this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.cmdOk.Enabled = false;
         this.cmdOk.Location = new System.Drawing.Point(95, 68);
         this.cmdOk.Name = "cmdOk";
         this.cmdOk.Size = new System.Drawing.Size(75, 23);
         this.cmdOk.TabIndex = 4;
         this.cmdOk.Text = "&OK";
         this.cmdOk.UseVisualStyleBackColor = true;
         // 
         // cmdCancel
         // 
         this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.cmdCancel.Location = new System.Drawing.Point(176, 68);
         this.cmdCancel.Name = "cmdCancel";
         this.cmdCancel.Size = new System.Drawing.Size(75, 23);
         this.cmdCancel.TabIndex = 5;
         this.cmdCancel.Text = "&Cancel";
         this.cmdCancel.UseVisualStyleBackColor = true;
         // 
         // AddManifestForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(263, 98);
         this.Controls.Add(this.cmdCancel);
         this.Controls.Add(this.cmdOk);
         this.Controls.Add(this.cbPlatform);
         this.Controls.Add(this.txtPackageId);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
         this.Name = "AddManifestForm";
         this.Text = "Create Pundit Manifest";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox txtPackageId;
      private System.Windows.Forms.ComboBox cbPlatform;
      private System.Windows.Forms.Button cmdOk;
      private System.Windows.Forms.Button cmdCancel;
   }
}