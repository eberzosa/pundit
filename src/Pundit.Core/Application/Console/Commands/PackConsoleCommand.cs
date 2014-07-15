using System;
using System.IO;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Application.Console.Commands
{
   class PackConsoleCommand : BaseConsoleCommand
   {
      public PackConsoleCommand(IConsoleOutput console, string currentDirectory, string[] parameters)
         : base(console, currentDirectory, parameters)
      {
      }

      private void ResolveParams(out string solutionRoot, out string packagePath, out string destinationFolder, out Version overrideVersion)
      {
         packagePath = GetLocalManifest();

         //resolve output path
         destinationFolder = GetParameter("o:|out:");
         if (destinationFolder != null)
         {
            destinationFolder = Path.IsPathRooted(destinationFolder)
                                   ? destinationFolder
                                   : Path.Combine(CurrentDirectory, destinationFolder);
         }
         else
         {
            destinationFolder = CurrentDirectory;
         }


         //resolve override version
         string vi = GetParameter("v:|version:");
         if (vi != null)
         {
            if (!Version.TryParse(vi, out overrideVersion))
               throw new ArgumentException(string.Format(Errors.InvalidVersionFormat, vi));
         }
         else overrideVersion = null;

         //validate
         if(!Directory.Exists(destinationFolder))
            throw new ArgumentException("destination directory does not exist at [" + destinationFolder + "]");

         solutionRoot = new FileInfo(packagePath).DirectoryName;
      }

      public override void Execute()
      {
         string solutionRoot, packagePath, destinationFolder;
         Version overrideVersion;

         ResolveParams(out solutionRoot, out packagePath, out destinationFolder, out overrideVersion);

         console.WriteLine("package: " + packagePath);
         console.WriteLine("solution root: " + solutionRoot);
         console.WriteLine("output folder: " + destinationFolder);

         DevPackage devPack;
         using(Stream devPackStream = File.OpenRead(packagePath))
         {
            devPack = DevPackage.FromStreamXml(devPackStream);
         }

         if(overrideVersion != null)
         {
            console.WriteLine("Overriding package version {0} with {1}", devPack.Version, overrideVersion);

            devPack.Version = overrideVersion;
         }

         string destinationFile = Path.Combine(destinationFolder, PackageUtils.GetFileName(devPack));

         if(File.Exists(destinationFile))
         {
            console.WriteLine(ConsoleColor.Red, string.Format("package exists at [{0}], deleting", destinationFile));
         }

         console.Write("creating package at " + destinationFile + "...");

         long bytesWritten;

         using (Stream writeStream = File.Create(destinationFile))
         {
            using (var pw = new PackageWriter(solutionRoot, devPack, writeStream))
            {
               bytesWritten = pw.WriteAll();
            }
         }
         console.Write(true);

         long packageSize = new FileInfo(destinationFile).Length;

         console.WriteLine(string.Format("Packed {0} to {1} (ratio: {2:D2}%)",
            PathUtils.FileSizeToString(bytesWritten),
            PathUtils.FileSizeToString(packageSize),
            packageSize * 100 / bytesWritten));
      }
   }
}
