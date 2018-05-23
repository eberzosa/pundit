using System.Collections.Generic;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;

namespace Pundit.Core.Model
{
   /// <summary>
   /// Package definition existing only at the time of development
   /// </summary>
   public class PackageSpec : PackageManifestRoot
   {
      public List<SourceFiles> Files { get; set; } = new List<SourceFiles>();

      public override void Validate()
      {
         if (Files == null || Files.Count == 0)
            throw new InvalidPackageException("manifest has no input files");

         base.Validate();
      }
   }
}
