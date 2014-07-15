using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Model.EventArguments
{
   /// <summary>
   /// 
   /// </summary>
   public class PackageKeyDiffEventArgs : EventArgs
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="key"></param>
      /// <param name="succeeded"></param>
      public PackageKeyDiffEventArgs(PackageKeyDiff key, bool succeeded)
      {
         this.PackageKeyDiff = key;
         this.Succeeded = succeeded;
      }

      /// <summary>
      /// 
      /// </summary>
      public PackageKeyDiff PackageKeyDiff { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public bool Succeeded { get; set; }
   }
}
