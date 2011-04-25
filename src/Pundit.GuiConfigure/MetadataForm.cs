using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pundit.Core.Model;

namespace Pundit.WinForms.App
{
   public partial class MetadataForm : Form
   {
      public MetadataForm()
      {
         InitializeComponent();
      }

      public MetadataForm(Package package) : this()
      {
         ucMetadata.Package = package;
         ucMetadata.ShowPackage();
      }

      private void cmdOk_Click(object sender, EventArgs e)
      {
         string author = ucMetadata.Package.Author;

         Close();
      }

      private void cmdCancel_Click(object sender, EventArgs e)
      {
         Close();
      }

      private void ucMetadata_Load(object sender, EventArgs e)
      {

      }
   }
}
