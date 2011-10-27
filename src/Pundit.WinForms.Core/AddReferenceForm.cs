using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pundit.WinForms.Core
{
   public partial class AddReferenceForm : Form
   {
      private readonly DirectoryInfo _manifestDirectory;

      public AddReferenceForm(DirectoryInfo manifestDirectory)
      {
         _manifestDirectory = manifestDirectory;

         InitializeComponent();

         txtRoot.Text = _manifestDirectory.FullName;
      }
   }
}
