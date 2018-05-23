﻿using System.Xml.Serialization;

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
      public PackageFileKind FileKind { get; set; }

      /// <summary>
      /// File pattern, relative to the location of the package manifest file.
      /// Can be relative or absolute. Can include file masks.
      /// </summary>
      public string Include { get; set; }

      /// <summary>
      /// File pattern, relative to the location of the package manifest file.
      /// Can be relative or absolute. Can include file masks.
      /// </summary>
      public string Exclude { get; set; }

      /// <summary>
      /// Ignore directory structure of source files, copy all files into a single directory.
      /// The default is <see langword="false"/>
      /// </summary>
      public bool Flatten { get; set; }

      /// <summary>
      /// Base directory of the source files to be copied. The default is null which 
      /// means destination folder will replicate directory structure from the
      /// beginning of the project file.
      /// </summary>
      public string BaseDirectory { get; set; }

      /// <summary>
      /// Target directory to copy the source files to. By default they will be copied to root.
      /// Ignored for binary files.
      /// </summary>
      public string TargetDirectory { get; set; }

      /// <summary>
      /// Copy empty directories. The default is <see langword="true"/>
      /// </summary>
      public bool IncludeEmptyDirs { get; set; }

      /// <summary>
      /// Configuration name (debug or release). Release is default.
      /// </summary>
      public BuildConfiguration Configuration { get; set; }

      public SourceFiles()
      {
         this.IncludeEmptyDirs = true;
      }

      public SourceFiles(string include, PackageFileKind kind = PackageFileKind.Binary) : this()
      {
         this.Include = include;
         this.FileKind = kind;
      }
   }
}