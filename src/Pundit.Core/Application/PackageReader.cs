using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using log4net;
using Pundit.Core.Model;

namespace Pundit.Core.Application
{
   public class PackageReader : IDisposable
   {
      private static readonly ILog Log = LogManager.GetLogger(typeof (PackageReader));
      public const string LocalLibFolderName = "lib";

      private readonly ZipInputStream _zipStream;


      public PackageReader(Stream packageStream)
      {
         _zipStream = new ZipInputStream(packageStream);
      }

      public Package ReadManifest()
      {
         ZipEntry entry;

         while((entry = _zipStream.GetNextEntry()) != null)
         {
            if(entry.IsFile && entry.Name == Package.DefaultPackageFileName)
            {
               Package manifest = Package.FromStream(_zipStream);

               return manifest;
            }
         }

         return null;
      }

      private void InstallLib(string libFolder, string entryName)
      {
         Log.DebugFormat("installing lib: {0}", entryName);

         if (!Directory.Exists(libFolder)) Directory.CreateDirectory(libFolder);

         string destFile = Path.Combine(libFolder, entryName);

         using (Stream os = File.Create(destFile))
         {
            _zipStream.CopyTo(os);
         }
      }

      public void InstallTo(string rootFolder, BuildConfiguration configuration)
      {
         ZipEntry entry;
         string libFolder = Path.Combine(rootFolder, "lib");

         while((entry = _zipStream.GetNextEntry()) != null)
         {
            string binInConfig = "bin/" + configuration.ToString().ToLower() + "/";

            string entryName = entry.Name.Replace("//", "/");

            if (entryName.StartsWith(binInConfig))
            {
               string shortEntryName = entryName.Substring(binInConfig.Length);

               if (string.Empty != shortEntryName)
               {
                  InstallLib(libFolder, shortEntryName);
               }
            }
         }
      }

      public void Dispose()
      {
         _zipStream.Close();
         _zipStream.Dispose();
      }
   }
}
