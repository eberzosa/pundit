using System;

namespace Pundit.Core.Model.EventArguments
{
   /// <summary>
   /// File packaging event args
   /// </summary>
   public class PackageFileEventArgs : EventArgs
   {
      /// <summary>
      /// Gets this file name
      /// </summary>
      public string FileName { get; private set; }

      /// <summary>
      /// Gets this file uncompressed size or compressed size depending when an event was thrown
      /// </summary>
      public long FileSize { get; private set; }

      /// <summary>
      /// Gets the file processing index in this batch
      /// </summary>
      public int FileIndex { get; private set; }

      /// <summary>
      /// Gets the total number of files in this batch
      /// </summary>
      public int FilesTotal { get; private set; }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="fileName"></param>
      /// <param name="fileSize"></param>
      /// <param name="fileIndex"></param>
      /// <param name="filesTotal"></param>
      public PackageFileEventArgs(string fileName, long fileSize, int fileIndex, int filesTotal)
      {
         FileName = fileName;
         FileSize = fileSize;
         FileIndex = fileIndex;
         FilesTotal = filesTotal;
      }
   }
}
