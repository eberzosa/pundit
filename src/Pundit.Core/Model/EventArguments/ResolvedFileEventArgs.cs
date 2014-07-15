using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Model.EventArguments
{
   /// <summary>
   /// 
   /// </summary>
   public class ResolvedFileEventArgs : EventArgs
   {
      /// <summary>
      /// 
      /// </summary>
      public PackageFileKind Kind { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public string PackageId { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public BuildConfiguration Configuration { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public string FileName { get; set; }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="packageId"></param>
      /// <param name="kind"></param>
      /// <param name="configuration"></param>
      /// <param name="fileName"></param>
      public ResolvedFileEventArgs(string packageId, PackageFileKind kind, BuildConfiguration configuration, string fileName)
      {
         PackageId = packageId;
         Kind = kind;
         Configuration = configuration;
         FileName = fileName;
      }
   }
}
