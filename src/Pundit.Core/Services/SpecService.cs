using System;
using System.IO;
using System.Reflection;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Serializers;
using NuGet.Versioning;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Services
{
   public class SpecService
   {
      private readonly string _currentDirectory;
      private readonly PackageSerializerFactory _packageSerializerFactory;
      private readonly IWriter _writer;

      public SpecService(string currentDirectory, PackageSerializerFactory packageSerializerFactory, IWriter writer)
      {
         _currentDirectory = currentDirectory;
         _packageSerializerFactory = packageSerializerFactory;
         _writer = writer;
      }

      public void Execute()
      {
         var packageSpec = new PackageSpec
         {
            PackageId = "MyApplication.Tool",
            Platform = "net46",
            Author = Environment.UserName,
            Description = "This is a sample package for a imaginaty tool.",
            License = "MIT license (or anything you want)",
            ProjectUrl = "http://myapplication.myweb.com",
            ReleaseNotes = "Initial version of this imaginary tool with lots of features.",
            Version = new NuGetVersion(Assembly.GetExecutingAssembly().GetName().Version),
            Files =
            {
               new SourceFiles("*.dll")
               {
                  BaseDirectory = "baseDirectoryWhereTheFilesAre/bin/tool",
                  Configuration = BuildConfiguration.Debug,
                  TargetDirectory = "libs",
                  IncludeEmptyDirs = true
               },
               new SourceFiles("file.*")
               {
                  BaseDirectory = "baseDirectoryWhereTheFilesAre/help",
                  Configuration = BuildConfiguration.Any,
                  TargetDirectory = "help",
                  Exclude = "*.abc",
                  Flatten = true,
                  FileKind = PackageFileKind.Other
               }
            },
            Dependencies =
            {
               new PackageDependency("MyApplication.Common", VersionRange.Parse("1.0.4"))
               {
                  Scope = DependencyScope.Normal,
                  Platform = "net46"
               },
               new PackageDependency("MyApplication.Test", VersionRange.Parse("1.0.4"))
               {
                  Scope = DependencyScope.Test,
                  Platform = "net46"
               },
               new PackageDependency("MyApplication.Legacy", VersionRange.Parse("1.0.4"))
               {
                  Scope = DependencyScope.Normal,
                  Platform = "net11",
                  CreatePlatformFolder = true
               },
            }
         };

         var path = Path.Combine(_currentDirectory, PackageManifest.DefaultManifestFileName);

         _writer.BeginWrite()
            .Text($"Writing package spec to '{path}'... ");

         using (var outFileStream = File.Create(path))
            _packageSerializerFactory.GetPundit().SerializePackageSpec(packageSpec, outFileStream);

         using (var outFileStream = File.Create(path + ".nuspec"))
            _packageSerializerFactory.GetNuGetv3().SerializePackageSpec(packageSpec, outFileStream);

            _writer.Success("done")
               .EndWrite();
      }
   }
}
