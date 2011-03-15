using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NGem.Core.Model
{
   public enum PackageFileKind
   {
      /// <summary>
      /// Placed in project-root/lib
      /// </summary>
      [XmlEnum("bin")]
      Binary,

      /// <summary>
      /// Placed in project-root/include/package-id
      /// </summary>
      [XmlEnum("include")]
      Include,

      /// <summary>
      /// Placed in project-root/tools/package-id
      /// </summary>
      [XmlEnum("tools")]
      Tools,

      /// <summary>
      /// placed in project-root/other/package-id
      /// </summary>
      [XmlEnum("other")]
      Other
   }

   public class PackageFiles
   {
      /// <summary>
      /// Well-known file type
      /// </summary>
      [XmlAttribute("kind")]
      public PackageFileKind FileKind { get; set; }

      /// <summary>
      /// Source to the files, relative to the location of the package manifest file.
      /// Can be relative or absolute. Can include file masks.
      /// </summary>
      [XmlAttribute("src")]
      public string Source { get; set; }

      public PackageFiles()
      {
         
      }

      public PackageFiles(string source, PackageFileKind kind = PackageFileKind.Binary)
      {
         this.Source = source;
         this.FileKind = kind;
      }
   }

   /// <summary>
   /// Package definition existing only at the time of development
   /// </summary>
   [XmlRoot("package")]
   public class DevPackage : Package
   {
      private List<PackageFiles> _files = new List<PackageFiles>();

      [XmlArray("files")]
      [XmlArrayItem("file")]
      public List<PackageFiles> Files
      {
         get { return _files; }
         set { _files = new List<PackageFiles>(value);}
      }

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

         if(Files.Count == 0)
            ex.AddError("Files", "package requires files to be included");

         if (ex.HasErrors)
            throw ex;
      }
   }
}
