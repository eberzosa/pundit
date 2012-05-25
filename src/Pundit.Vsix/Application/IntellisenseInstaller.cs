using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Pundit.Core.Model;
using Pundit.Vsix.AppPackage;
using Pundit.Vsix.Resources;
using Pundit.WinForms.Core;

namespace Pundit.Vsix.Application
{
   static class IntellisenseInstaller
   {
      public static void InteractiveInstallShema(IConsoleOutput console)
      {
         if(NeedsUpgrade)
         {
            console.WriteLine(VSPackage.Xsd_Start);
            bool? upgraded = InstallSchema();
            if(upgraded == false)
            {
               console.WriteLine(VSPackage.Xsd_CantUpgrade);
            }
            else if(upgraded == true)
            {
               console.WriteLine(VSPackage.Xsd_Upgraded);
            }
         }
      }

      private static string TargetXsdDirectoryName
      {
         get
         {
            string targetDir = Path.Combine(
               Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
               @"Microsoft Visual Studio 10.0\Xml\Schemas");

            return targetDir;
         }
      }

      private static string TargetXsdFileName
      {
         get
         {
            string targetFile = Path.Combine(TargetXsdDirectoryName, "pundit.xsd");

            return targetFile;
         }
      }

      private static Stream OpenSourceStream()
      {
         string resourceName = string.Format("{0}.Resources.pundit.xsd", typeof(PunditPackage).Namespace);
         return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
      }

      private static bool NeedsUpgrade
      {
         get
         {
            long srcLen;
            using(Stream src = OpenSourceStream())
            {
               srcLen = src == null ? 0 : src.Length;
            }

            string targetFile = TargetXsdFileName;
            long destLen = File.Exists(targetFile) ? new FileInfo(targetFile).Length : 0;

            return srcLen != destLen;
         }
      }

      public static bool? InstallSchema()
      {
         if(NeedsUpgrade)
         {
            using(Stream src = OpenSourceStream())
            {
               string targetName = TargetXsdFileName;
               try
               {
                  using(Stream tgt = File.Create(targetName))
                  {
                     src.CopyTo(tgt);
                  }

                  return true;
               }
               catch(UnauthorizedAccessException)
               {
                  return false;
               }
               finally
               {
                  if(File.Exists(targetName))
                  {
                     try
                     {
                        File.Delete(targetName);
                     }
                     catch
                     {
                        
                     }
                  }
               }
            }
         }
         return null;
      }
   }
}
