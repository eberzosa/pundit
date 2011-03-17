using System.IO;
using System.Xml.Serialization;

namespace NGem.Core.Model
{
   public class SourceFiles
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

      public SourceFiles()
      {
         
      }

      public SourceFiles(string source, PackageFileKind kind = PackageFileKind.Binary)
      {
         this.Source = source;
         this.FileKind = kind;
      }

      public string[] Resolve(string manifestRoot)
      {
         string absolutePath = Path.IsPathRooted(Source) ? Source : Path.Combine(manifestRoot, Source);
         string absoluteDir = Path.GetDirectoryName(absolutePath);

         if(!Directory.Exists(absoluteDir))
            throw new DirectoryNotFoundException("directory [" + absoluteDir + "] not found");

         return Directory.GetFiles(absoluteDir, Path.GetFileName(absolutePath));
      }
   }
}