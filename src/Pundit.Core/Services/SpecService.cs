using System;
using System.IO;
using System.Reflection;
using EBerzosa.Pundit.Core.Converters;
using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Serializers;
using EBerzosa.Utils;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Services
{
   public class SpecService
   {
      private readonly IPackageSerializer _packageSerializer;
      private readonly string _currentDirectory;
      private readonly IWriter _writer;

      public SpecService(IPackageSerializer packageSerializer, string currentDirectory, IWriter writer)
      {
         Guard.NotNull(packageSerializer, nameof(packageSerializer));
         Guard.NotNull(currentDirectory, nameof(currentDirectory));
         Guard.NotNull(writer, nameof(writer));

         _packageSerializer = packageSerializer;
         _currentDirectory = currentDirectory;
         _writer = writer;
      }

      public void Execute()
      {
         var packageSpec = new PackageSpec
         {
            PackageId = "MyApplication.Tool",
            Author = Environment.UserName,
            Description = "This is a sample package for a imaginaty tool.",
            License = "MIT license (or anything you want)",
            ProjectUrl = "http://myapplication.myweb.com",
            ReleaseNotes = "Initial version of this imaginary tool with lots of features.",
            Version = new NuGet.Versioning.NuGetVersion(Assembly.GetExecutingAssembly().GetName().Version),
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
               new PackageDependency("MyApplication.Common", NuGet.Versioning.VersionRange.Parse("1.0.4"))
               {
                  Scope = DependencyScope.Normal
               },
               new PackageDependency("MyApplication.Test", NuGet.Versioning.VersionRange.Parse("1.0.4"))
               {
                  Scope = DependencyScope.Test
               },
               new PackageDependency("MyApplication.Legacy", NuGet.Versioning.VersionRange.Parse("1.0.4"))
               {
                  Scope = DependencyScope.Normal,
                  CreatePlatformFolder = true
               },
            }
         };

         var path = Path.Combine(_currentDirectory, PackageManifest.DefaultManifestFileName);

         _writer.BeginWrite()
            .Text($"Writing package spec to '{path}'... ");

         using (var outFileStream = File.Create(path))
            _packageSerializer.SerializePackageSpec(packageSpec, outFileStream);
         
            _writer.Success("done")
               .EndWrite();
      }
   }
}
