using System;
using System.IO;
using NDesk.Options;
using Pundit.Core.Application;
using Pundit.Core.Model;

namespace Pundit.Console.Commands
{
   public class PackCommand : ICommand
   {
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
         if (po != null)
         {
            if (Path.IsPathRooted(po))
               packagePath = po;
            else
               packagePath = Path.Combine(Environment.CurrentDirectory, po);
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

         System.Console.WriteLine("package: " + packagePath);
         System.Console.WriteLine("solution root: " + solutionRoot);
         System.Console.WriteLine("output folder: " + destinationFolder);

         DevPackage devPack;
         using(Stream devPackStream = File.OpenRead(packagePath))
         {
            devPack = DevPackage.FromStream(devPackStream);
         }

         string destinationFile = Path.Combine(destinationFolder, devPack.GetFileName());

         if(File.Exists(destinationFile))
         {
            using(new ColorChange(ConsoleColor.Yellow))
            {
               System.Console.WriteLine("package exists at [{0}], deleting", destinationFile);
            }
         }

         System.Console.WriteLine("creating package at [" + destinationFile + "]");

         using (Stream writeStream = File.Create(destinationFile))
         {
            using (PackageWriter pw = new PackageWriter(solutionRoot, devPack, writeStream))
            {
               pw.WriteAll();
            }
         }
      }
   }
}
