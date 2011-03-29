using System;
using System.IO;
using log4net;
using NDesk.Options;
using Pundit.Core.Application;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Console.Commands
{
   public class PackCommand : ICommand
   {
      private static readonly ILog Log = LogManager.GetLogger(typeof (PackCommand));
      private string[] _cmdline;

      public PackCommand(string[] parameters)
      {
         _cmdline = parameters;
      }

      private void ResolveParams(out string solutionRoot, out string packagePath, out string destinationFolder)
      {
         string pi = null;
         string po = null;

         OptionSet oset = new OptionSet()
            .Add("i:|input:", i => pi = i)
            .Add("o:|output:", o => po = o);

         oset.Parse(_cmdline);

         //resolve package path
         if (pi != null)
         {
            if (Path.IsPathRooted(pi))
               packagePath = pi;
            else
               packagePath = Path.Combine(Environment.CurrentDirectory, pi);
         }
         else
         {
            packagePath = Path.Combine(Environment.CurrentDirectory, Package.DefaultPackageFileName);
         }

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

         //validate

         if(!File.Exists(packagePath))
            throw new ArgumentException("package definition doesn't exist at [" + packagePath + "]");

         if(!Directory.Exists(destinationFolder))
            throw new ArgumentException("destination directory does not exist at [" + destinationFolder + "]");

         solutionRoot = new FileInfo(packagePath).DirectoryName;
      }

      public void Execute()
      {
         string solutionRoot, packagePath, destinationFolder;

         ResolveParams(out solutionRoot, out packagePath, out destinationFolder);

         Log.Debug("package: " + packagePath);
         Log.Debug("solution root: " + solutionRoot);
         Log.Debug("output folder: " + destinationFolder);

         DevPackage devPack;
         using(Stream devPackStream = File.OpenRead(packagePath))
         {
            devPack = DevPackage.FromStream(devPackStream);
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
            using (PackageWriter pw = new PackageWriter(solutionRoot, devPack, writeStream))
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
