using System;
using System.IO;
using NDesk.Options;
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

         string po = null;
         string vi = null;


         OptionSet oset = new OptionSet()
            .Add("f:|folder:", o => po = o)
            .Add("v:|version:", v => vi = v);

         oset.Parse(GetCommandLine());

         //resolve output path
         if(po != null)
         {
            if (Path.IsPathRooted(po))
               destinationFolder = po;
            else
               destinationFolder = Path.Combine(Environment.CurrentDirectory, po);
         }
         else
         {
            destinationFolder = Environment.CurrentDirectory;
         }

         //resolve override version
         if (vi != null)
         {
            if (!Version.TryParse(vi, out overrideVersion))
               throw new ArgumentException("version given [" + vi + "] is not in valid format");
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
            devPack = DevPackage.FromStream(devPackStream);
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

         console.WriteLine("creating package at [" + destinationFile + "]");

         long bytesWritten;

         using (Stream writeStream = File.Create(destinationFile))
         {
            using (var pw = new PackageWriter(solutionRoot, devPack, writeStream))
            {
               pw.BeginPackingFile += pw_BeginPackingFile;
               pw.EndPackingFile += pw_EndPackingFile;
               bytesWritten = pw.WriteAll();
            }
         }

         long packageSize = new FileInfo(destinationFile).Length;

         console.WriteLine(string.Format("Packed {0} to {1} (ratio: {2:D2}%)",
            PathUtils.FileSizeToString(bytesWritten),
            PathUtils.FileSizeToString(packageSize),
            packageSize * 100 / bytesWritten));
      }

      void pw_EndPackingFile(object sender, Core.Model.EventArguments.PackageFileEventArgs e)
      {
         console.Write(true);
      }

      void pw_BeginPackingFile(object sender, Core.Model.EventArguments.PackageFileEventArgs e)
      {
         console.Write("packing ");
         console.Write(ConsoleColor.Green, e.FileName);
         console.Write("... ");
      }
   }
}
