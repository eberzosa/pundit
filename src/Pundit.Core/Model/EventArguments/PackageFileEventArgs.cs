using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Model.EventArguments
{
   public class PackageFileEventArgs : EventArgs
   {
      public string FileName { get; set; }
      public long FileSize { get; set; }

      public PackageFileEventArgs(string fileName, long fileSize)
      {
         FileName = fileName;
         FileSize = fileSize;
      }
   }
}
