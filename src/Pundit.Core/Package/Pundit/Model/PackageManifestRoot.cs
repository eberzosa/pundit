using System;
using System.Diagnostics;

namespace EBerzosa.Pundit.Core.Model.Package
{
   [DebuggerDisplay("{PackageId} [{Version.ToString()}] [{Framework?.GetShortFolderName()}]")]
   public class PackageManifestRoot : PackageManifest
   {
      public NuGet.Frameworks.NuGetFramework Framework { get; set; }

      public override void Validate()
      {
         base.Validate();

         if (Framework.IsAny || Framework.IsUnsupported)
            throw new NotSupportedException("The framework cannot be any or unsupported, current: " + Framework);
      }
   }
}