using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pundit.WinForms.Core
{
   public partial class PlainTextModalEditForm : Form
   {
      public PlainTextModalEditForm()
      {
         InitializeComponent();
      }

      public PlainTextModalEditForm(string title, string initialText) : this()
      {
         if(title != null) Text = title;
         if (initialText != null) textMultiText.Text = initialText;
      }

      public string EditedText
      {
         get { return textMultiText.Text; }
      }

      private void cmdSave_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      private void cmdCancel_Click(object sender, EventArgs e)
      {
         this.Close();
      }
   }
}
