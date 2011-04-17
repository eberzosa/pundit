using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NDesk.Options;
using Pundit.Core.Model;

namespace Pundit.Console.Commands
{
   class TemplateCommand : ICommand
   {
      private ILog _log = LogManager.GetLogger(typeof (TemplateCommand));

      public TemplateCommand(string[] args)
      {
      }

      public void Execute()
      {
         DevPackage dp = new DevPackage();
         dp.PackageId = "Package.Id";
         dp.Platform = "package.platform";
         dp.Author = Environment.UserName;
         dp.Description = "description goes here";
         dp.License = "license goes here";
         dp.ProjectUrl = "http://product.company.co.uk";
         dp.ReleaseNotes = "release notes for this version";
         dp.Version = Assembly.GetExecutingAssembly().GetName().Version;

         SourceFiles sampleFile =  new SourceFiles("*.h");
         sampleFile.BaseDirectory = "base/dir";
         sampleFile.Configuration = BuildConfiguration.Debug;
         sampleFile.TargetDirectory = "targetdir";
         dp.Files.Add(sampleFile);

         dp.Dependencies.Add(new PackageDependency("log4net", "1.2.10"));

         string path = Path.Combine(Environment.CurrentDirectory, Package.DefaultPackageFileName);

         _log.Info("writing package to " + path);

         using(Stream s = File.Create(path))
         {
            dp.WriteTo(s);
         }

         _log.Info("done");
      }
   }
}
