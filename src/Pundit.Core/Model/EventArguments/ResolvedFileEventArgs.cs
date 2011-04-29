using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Model.EventArguments
{
   public class ResolvedFileEventArgs : EventArgs
   {
      public PackageFileKind Kind { get; set; }
      public string PackageId { get; set; }
      public BuildConfiguration Configuration { get; set; }
      public string FileName { get; set; }

      public ResolvedFileEventArgs(string packageId, PackageFileKind kind, BuildConfiguration configuration, string fileName)
      {
         PackageId = packageId;
         Kind = kind;
         Configuration = configuration;
         FileName = fileName;
      }
   }
}
