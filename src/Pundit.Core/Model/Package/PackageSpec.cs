using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   /// <summary>
   /// Package definition existing only at the time of development
   /// </summary>
   public class PackageSpec : PackageManifest
   {
      public List<SourceFiles> Files { get; set; } = new List<SourceFiles>();

      public override void Validate()
      {
         InvalidPackageException ex;

         try
         {
            base.Validate();

            ex = new InvalidPackageException();
         }
         catch(InvalidPackageException ex1)
         {
            ex = ex1;
         }

         //if(Files.Count == 0)
         //   ex.AddError("Files", "package requires files to be included");

         if (ex.HasErrors)
            throw ex;
      }
   }
}
