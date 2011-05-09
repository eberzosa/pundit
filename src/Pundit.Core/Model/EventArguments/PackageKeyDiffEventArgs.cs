using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Model.EventArguments
{
   public class PackageKeyDiffEventArgs : EventArgs
   {
      public PackageKeyDiffEventArgs(PackageKeyDiff key, bool succeeded)
      {
         this.PackageKeyDiff = key;
         this.Succeeded = succeeded;
      }

      public PackageKeyDiff PackageKeyDiff { get; set; }

      public bool Succeeded { get; set; }
   }
}
