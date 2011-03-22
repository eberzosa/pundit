using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using NAnt.Core;
using Pundit.Core.Utils;

namespace Pundit.Core.Model
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
      [XmlAttribute("include")]
      public string Include { get; set; }

      [XmlAttribute("exclude")]
      public string Exclude { get; set; }

      /// <summary>
      /// Ignore directory structure of source files, copy all files into a single directory.
      /// The default is <see langword="false"/>
      /// </summary>
      [XmlAttribute("flatten")]
      public bool Flatten { get; set; }

      /// <summary>
      /// Copy empty directories. The default is <see langword="true"/>
      /// </summary>
      [XmlAttribute("includeemptydirs")]
      public bool IncludeEmptyDirs { get; set; }

      public SourceFiles()
      {
         this.IncludeEmptyDirs = true;
      }

      public SourceFiles(string include, PackageFileKind kind = PackageFileKind.Binary) : this()
      {
         this.Include = include;
         this.FileKind = kind;
      }

      private static string[] ParsePatternArray(string array)
      {
         return string.IsNullOrEmpty(array)
                   ? new string[0]
                   : array.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
      }

      public void Resolve(string baseDir, out string[] files, out string[] directories)
      {
         if(!Directory.Exists(baseDir))
            throw new DirectoryNotFoundException("directory [" + baseDir + "] not found");

         baseDir = new DirectoryInfo(baseDir).FullName;

         //using NAnt.Core for now, will continue working on FileSet later
         DirectoryScanner scanner = new DirectoryScanner(false);
         scanner.BaseDirectory = new DirectoryInfo(baseDir);
         scanner.Excludes.AddRange(FileSet.DefaultExcludesList.ToArray());
         scanner.Excludes.AddRange(ParsePatternArray(Exclude));
         scanner.Includes.AddRange(ParsePatternArray(Include));

         scanner.Scan();

         files = new string[scanner.FileNames.Count];
         directories = new string[scanner.DirectoryNames.Count];
         
         scanner.FileNames.CopyTo(files, 0);
         scanner.DirectoryNames.CopyTo(directories, 0);
      }

      public string GetRelativeUnixPath(string baseDir, string path)
      {
         if (!Flatten)
         {
            path = path.Substring(baseDir.Length);

            if (path.StartsWith("" + Path.DirectorySeparatorChar)) path = path.Substring(1);

            path = path.Replace(Path.DirectorySeparatorChar, '/');
         }
         else
         {
            path = new FileInfo(path).Name;
         }

         switch(FileKind)
         {
            case PackageFileKind.Binary:
               path = "bin/" + path;
               break;
            case PackageFileKind.Include:
               path = "include/" + path;
               break;
            case PackageFileKind.Tools:
               path = "tools/" + path;
               break;
            case PackageFileKind.Other:
               path = "other/" + path;
               break;
         }

         return path;
      }
   }
}