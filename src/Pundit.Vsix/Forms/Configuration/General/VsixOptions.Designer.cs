namespace Pundit.Vsix.Forms.Configuration.General
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
         this.label3 = new System.Windows.Forms.Label();
         this.rbUpdateJustNotify = new System.Windows.Forms.RadioButton();
         this.rbUpdateBackground = new System.Windows.Forms.RadioButton();
         ((System.ComponentModel.ISupportInitialize)(this.numPingInterval)).BeginInit();
         this.pnlPing.SuspendLayout();
         this.SuspendLayout();
         // 
         // chkDoPing
         // 
         this.chkDoPing.AutoSize = true;
         this.chkDoPing.Location = new System.Drawing.Point(13, 22);
         this.chkDoPing.Name = "chkDoPing";
         this.chkDoPing.Size = new System.Drawing.Size(221, 17);
         this.chkDoPing.TabIndex = 0;
         this.chkDoPing.Text = "check for solution dependencies updates";
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
         this.numPingInterval.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
         this.numPingInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.numPingInterval.Name = "numPingInterval";
         this.numPingInterval.Size = new System.Drawing.Size(67, 20);
         this.numPingInterval.TabIndex = 2;
         this.numPingInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
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
         this.pnlPing.Controls.Add(this.rbUpdateBackground);
         this.pnlPing.Controls.Add(this.rbUpdateJustNotify);
         this.pnlPing.Controls.Add(this.label3);
         this.pnlPing.Controls.Add(this.cmbPingMeasure);
         this.pnlPing.Controls.Add(this.label1);
         this.pnlPing.Controls.Add(this.numPingInterval);
         this.pnlPing.Enabled = false;
         this.pnlPing.Location = new System.Drawing.Point(25, 40);
         this.pnlPing.Name = "pnlPing";
         this.pnlPing.Size = new System.Drawing.Size(364, 92);
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
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(7, 27);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(127, 13);
         this.label3.TabIndex = 4;
         this.label3.Text = "when an update is found:";
         // 
         // rbUpdateJustNotify
         // 
         this.rbUpdateJustNotify.AutoSize = true;
         this.rbUpdateJustNotify.Checked = true;
         this.rbUpdateJustNotify.Location = new System.Drawing.Point(19, 43);
         this.rbUpdateJustNotify.Name = "rbUpdateJustNotify";
         this.rbUpdateJustNotify.Size = new System.Drawing.Size(86, 17);
         this.rbUpdateJustNotify.TabIndex = 5;
         this.rbUpdateJustNotify.TabStop = true;
         this.rbUpdateJustNotify.Text = "just notify me";
         this.rbUpdateJustNotify.UseVisualStyleBackColor = true;
         // 
         // rbUpdateBackground
         // 
         this.rbUpdateBackground.AutoSize = true;
         this.rbUpdateBackground.Location = new System.Drawing.Point(19, 62);
         this.rbUpdateBackground.Name = "rbUpdateBackground";
         this.rbUpdateBackground.Size = new System.Drawing.Size(173, 17);
         this.rbUpdateBackground.TabIndex = 6;
         this.rbUpdateBackground.Text = "apply silently in the background";
         this.rbUpdateBackground.UseVisualStyleBackColor = true;
         // 
         // VsixOptions
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
      private System.Windows.Forms.RadioButton rbUpdateBackground;
      private System.Windows.Forms.RadioButton rbUpdateJustNotify;
      private System.Windows.Forms.Label label3;
   }
}
