using System;

namespace Pundit.Core.Model.EventArguments
{
   public class PackageDownloadEventArgs : EventArgs
   {
      public PackageDownloadEventArgs(PackageKey key, bool succeeded, long totalSize, long downloadedSize)
      {
         this.PackageKey = key;
         this.Succeeded = succeeded;
         this.TotalSize = totalSize;
         this.DownloadedSize = downloadedSize;
      }

      public PackageKey PackageKey { get; private set; }

      public bool Succeeded { get; private set; }

      public long TotalSize { get; private set; }

      public long DownloadedSize { get; private set; }
   }
}
