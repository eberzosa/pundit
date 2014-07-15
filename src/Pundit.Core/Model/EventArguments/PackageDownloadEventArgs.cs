using System;

namespace Pundit.Core.Model.EventArguments
{
   public class PackageDownloadEventArgs : EventArgs
   {
      public PackageDownloadEventArgs(PackageKey key, bool succeeded, long totalSize, long downloadedSize, long avgSpeed)
      {
         this.PackageKey = key;
         this.Succeeded = succeeded;
         this.TotalSize = totalSize;
         this.DownloadedSize = downloadedSize;
         this.AvgSpeed = avgSpeed;
      }

      public PackageKey PackageKey { get; private set; }

      public bool Succeeded { get; private set; }

      public long TotalSize { get; private set; }

      public long DownloadedSize { get; private set; }

      /// <summary>
      /// Average speed in bytes per second
      /// </summary>
      public long AvgSpeed { get; private set; }

      public string FailureReason { get; private set; }
   }
}
