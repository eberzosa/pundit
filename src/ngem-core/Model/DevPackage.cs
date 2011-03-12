using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NGem.Core.Model
{
   public enum PackageFileKind
   {
      [XmlEnum("bin")]
      Binary,

      [XmlEnum("src")]
      SourceCode,

      [XmlEnum("res")]
      Resource,

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

      /// <summary>
      /// Profile name (optional). When providing the library for different platforms
      /// or runtimes, can identify specific profile the client can choose from.
      /// Known profiles recommended to use when possible:
      /// 
      /// NET10
      /// NET11
      /// NET20
      /// NET30
      /// NET35
      /// NET40
      /// WINx86 - native windows x86 binary
      /// WINx64 - native windows Intel x64 binary
      /// 
      /// </summary>
      [XmlAttribute("profile")]
      public string Profile { get; set; }

      public PackageFiles()
      {
         
      }

      public PackageFiles(string source, PackageFileKind kind = PackageFileKind.Binary, string profile = null)
      {
         this.Source = source;
         this.FileKind = kind;
         this.Profile = profile;
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
