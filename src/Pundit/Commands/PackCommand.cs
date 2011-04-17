using System;
using System.IO;
using log4net;
using NDesk.Options;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Console.Commands
{
   class PackCommand : BaseCommand
   {
      private string _packagePath;

      public PackCommand(string[] parameters) : base(parameters)
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

         Log.Debug("package: " + packagePath);
         Log.Debug("solution root: " + solutionRoot);
         Log.Debug("output folder: " + destinationFolder);

         DevPackage devPack;
         using(Stream devPackStream = File.OpenRead(packagePath))
         {
            devPack = DevPackage.FromStream(devPackStream);
         }

         if(overrideVersion != null)
         {
            Log.InfoFormat("Overriding package version {0} with {1}", devPack.Version, overrideVersion);

            devPack.Version = overrideVersion;
         }

         string destinationFile = Path.Combine(destinationFolder, PackageUtils.GetFileName(devPack));

         if(File.Exists(destinationFile))
         {
            Log.Warn(string.Format("package exists at [{0}], deleting", destinationFile));
         }

         Log.Info("creating package at [" + destinationFile + "]");

         long bytesWritten;

         using (Stream writeStream = File.Create(destinationFile))
         {
            using (var pw = new PackageWriter(solutionRoot, devPack, writeStream))
            {
               bytesWritten = pw.WriteAll();
            }
         }

         long packageSize = new FileInfo(destinationFile).Length;

         Log.Info(string.Format("Packed {0} to {1} (ratio: {2:D2}%)",
            PathUtils.FileSizeToString(bytesWritten),
            PathUtils.FileSizeToString(packageSize),
            packageSize * 100 / bytesWritten));
      }
   }
}
