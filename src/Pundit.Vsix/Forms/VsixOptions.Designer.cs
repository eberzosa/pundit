namespace Pundit.Vsix.Forms
{
   partial class VsixOptions
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
         this.chkDoPing = new System.Windows.Forms.CheckBox();
         this.label1 = new System.Windows.Forms.Label();
         this.numPingInterval = new System.Windows.Forms.NumericUpDown();
         this.cmbPingMeasure = new System.Windows.Forms.ComboBox();
         this.pnlPing = new System.Windows.Forms.Panel();
         this.label2 = new System.Windows.Forms.Label();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.lblVersion = new System.Windows.Forms.Label();
         this.lnkLicense = new System.Windows.Forms.LinkLabel();
         this.lnkDocs = new System.Windows.Forms.LinkLabel();
         ((System.ComponentModel.ISupportInitialize)(this.numPingInterval)).BeginInit();
         this.pnlPing.SuspendLayout();
         this.SuspendLayout();
         // 
         // chkDoPing
         // 
         this.chkDoPing.AutoSize = true;
         this.chkDoPing.Location = new System.Drawing.Point(13, 22);
         this.chkDoPing.Name = "chkDoPing";
         this.chkDoPing.Size = new System.Drawing.Size(228, 17);
         this.chkDoPing.TabIndex = 0;
         this.chkDoPing.Text = "check for solution\'s dependencies updates";
         this.chkDoPing.UseVisualStyleBackColor = true;
         this.chkDoPing.CheckedChanged += new System.EventHandler(this.chkDoPing_CheckedChanged);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(7, 5);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(33, 13);
         this.label1.TabIndex = 1;
         this.label1.Text = "every";
         // 
         // numPingInterval
         // 
         this.numPingInterval.Location = new System.Drawing.Point(46, 3);
         this.numPingInterval.Name = "numPingInterval";
         this.numPingInterval.Size = new System.Drawing.Size(67, 20);
         this.numPingInterval.TabIndex = 2;
         // 
         // cmbPingMeasure
         // 
         this.cmbPingMeasure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbPingMeasure.FormattingEnabled = true;
         this.cmbPingMeasure.Items.AddRange(new object[] {
            "minute(s)",
            "hour(s)",
            "day(s)"});
         this.cmbPingMeasure.Location = new System.Drawing.Point(120, 3);
         this.cmbPingMeasure.Name = "cmbPingMeasure";
         this.cmbPingMeasure.Size = new System.Drawing.Size(99, 21);
         this.cmbPingMeasure.TabIndex = 3;
         // 
         // pnlPing
         // 
         this.pnlPing.Controls.Add(this.cmbPingMeasure);
         this.pnlPing.Controls.Add(this.label1);
         this.pnlPing.Controls.Add(this.numPingInterval);
         this.pnlPing.Enabled = false;
         this.pnlPing.Location = new System.Drawing.Point(25, 40);
         this.pnlPing.Name = "pnlPing";
         this.pnlPing.Size = new System.Drawing.Size(364, 38);
         this.pnlPing.TabIndex = 4;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(-3, 5);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(65, 13);
         this.label2.TabIndex = 5;
         this.label2.Text = "Notifications";
         // 
         // groupBox1
         // 
         this.groupBox1.Location = new System.Drawing.Point(63, 11);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(440, 3);
         this.groupBox1.TabIndex = 6;
         this.groupBox1.TabStop = false;
         // 
         // lblVersion
         // 
         this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.lblVersion.AutoSize = true;
         this.lblVersion.Enabled = false;
         this.lblVersion.ForeColor = System.Drawing.SystemColors.ActiveCaption;
         this.lblVersion.Location = new System.Drawing.Point(0, 269);
         this.lblVersion.Name = "lblVersion";
         this.lblVersion.Size = new System.Drawing.Size(151, 13);
         this.lblVersion.TabIndex = 7;
         this.lblVersion.Text = "Pundit {0} open-source project";
         // 
         // lnkLicense
         // 
         this.lnkLicense.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.lnkLicense.AutoSize = true;
         this.lnkLicense.Location = new System.Drawing.Point(471, 269);
         this.lnkLicense.Name = "lnkLicense";
         this.lnkLicense.Size = new System.Drawing.Size(40, 13);
         this.lnkLicense.TabIndex = 8;
         this.lnkLicense.TabStop = true;
         this.lnkLicense.Text = "license";
         this.lnkLicense.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLicense_LinkClicked);
         // 
         // lnkDocs
         // 
         this.lnkDocs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.lnkDocs.AutoSize = true;
         this.lnkDocs.Location = new System.Drawing.Point(394, 269);
         this.lnkDocs.Name = "lnkDocs";
         this.lnkDocs.Size = new System.Drawing.Size(77, 13);
         this.lnkDocs.TabIndex = 9;
         this.lnkDocs.TabStop = true;
         this.lnkDocs.Text = "documentation";
         this.lnkDocs.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDocs_LinkClicked);
         // 
         // VsixOptions
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.lnkDocs);
         this.Controls.Add(this.lnkLicense);
         this.Controls.Add(this.lblVersion);
         this.Controls.Add(this.groupBox1);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.pnlPing);
         this.Controls.Add(this.chkDoPing);
         this.Name = "VsixOptions";
         this.Size = new System.Drawing.Size(508, 283);
         ((System.ComponentModel.ISupportInitialize)(this.numPingInterval)).EndInit();
         this.pnlPing.ResumeLayout(false);
         this.pnlPing.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.CheckBox chkDoPing;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.NumericUpDown numPingInterval;
      private System.Windows.Forms.ComboBox cmbPingMeasure;
      private System.Windows.Forms.Panel pnlPing;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.Label lblVersion;
      private System.Windows.Forms.LinkLabel lnkLicense;
      private System.Windows.Forms.LinkLabel lnkDocs;
   }
}
