using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EBerzosa.Pundit.Core.Model.Package;

namespace Pundit.Core.Model.EventArguments
{
   public class PackageKeyEventArgs : EventArgs
   {
      public PackageKeyEventArgs(PackageKey key, bool succeeded)
      {
         this.PackageKey = key;
         this.Succeeded = succeeded;
      }

      public PackageKey PackageKey { get; set; }

      public bool Succeeded { get; set; }
   }
}
