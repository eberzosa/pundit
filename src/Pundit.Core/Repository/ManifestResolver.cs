using System;
using System.IO;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.Core.Repository
{
   public class ManifestResolver
   {
      /// <summary>
      /// Default package manifest file name
      /// </summary>
      public const string DefaultManifestFileName = "pundit.xml";


      public string CurrentDirectory { get; }

      public ManifestResolver(string currentDirectory)
      {
         Guard.NotNull(currentDirectory, nameof(currentDirectory));

         CurrentDirectory = currentDirectory;
      }

      public string GetManifest(string manifestFileOrPath)
      {
         var manifestPath = manifestFileOrPath != null
            ? GetFilePath(manifestFileOrPath)
            : Path.Combine(CurrentDirectory, DefaultManifestFileName);

         if (!File.Exists(manifestPath))
            throw new FileNotFoundException($"Manifest '{manifestPath}' does not exist");

         return manifestPath;
      }

      private string GetFilePath(string fileOrPath)
      {
         if (Path.IsPathRooted(fileOrPath))
            return fileOrPath;

         switch (Path.PathSeparator)
         {
            case '\\':
               fileOrPath = fileOrPath.Replace('/', '\\');
               break;

            case '/':
               fileOrPath = fileOrPath.Replace('\\', '/');
               break;

            default:
               throw new NotSupportedException($"Path separator '{Path.PathSeparator}' is not supported");
         }

         return Path.Combine(CurrentDirectory, fileOrPath);
      }
   }
}
