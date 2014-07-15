using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   /// <summary>
   /// Files that represent a part of the compiled package.
   /// </summary>
   public class SourceFiles
   {
      /// <summary>
      /// Well-known file type. Default is <see cref="PackageFileKind.Binary"/>
      /// </summary>
      [XmlAttribute("kind")]
      public PackageFileKind FileKind { get; set; }

      /// <summary>
      /// File pattern, relative to the location of the package manifest file.
      /// Can be relative or absolute. Can include file masks.
      /// </summary>
      [XmlAttribute("include")]
      public string Include { get; set; }

      /// <summary>
      /// File pattern, relative to the location of the package manifest file.
      /// Can be relative or absolute. Can include file masks.
      /// </summary>
      [XmlAttribute("exclude")]
      public string Exclude { get; set; }

      /// <summary>
      /// Ignore directory structure of source files, copy all files into a single directory.
      /// The default is <see langword="false"/>
      /// </summary>
      [XmlAttribute("flatten")]
      public bool Flatten { get; set; }

      /// <summary>
      /// Base directory of the source files to be copied. The default is null which 
      /// means destination folder will replicate directory structure from the
      /// beginning of the project file.
      /// </summary>
      [XmlAttribute("basedir")]
      public string BaseDirectory { get; set; }

      /// <summary>
      /// Target directory to copy the source files to. By default they will be copied to root.
      /// Ignored for binary files.
      /// </summary>
      [XmlAttribute("targetdir")]
      public string TargetDirectory { get; set; }

      /// <summary>
      /// Copy empty directories. The default is <see langword="true"/>
      /// </summary>
      [XmlAttribute("includeemptydirs")]
      public bool IncludeEmptyDirs { get; set; }

      /// <summary>
      /// Configuration name (debug or release). Release is default.
      /// </summary>
      [XmlAttribute("configuration")]
      public BuildConfiguration Configuration { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public SourceFiles()
      {
         this.IncludeEmptyDirs = true;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="baseDirectory"></param>
      /// <param name="include"></param>
      /// <param name="kind"></param>
      public SourceFiles(string baseDirectory, string include, PackageFileKind kind = PackageFileKind.Binary) : this()
      {
         this.BaseDirectory = baseDirectory;
         this.Include = include;
         this.FileKind = kind;
      }
   }
}