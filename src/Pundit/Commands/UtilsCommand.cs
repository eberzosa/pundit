using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NDesk.Options;
using Pundit.Core;
using Pundit.Core.Utils;

namespace Pundit.Console.Commands
{
   class UtilsCommand : BaseCommand
   {
      public UtilsCommand(string[] args) : base(args)
      {
      }

      public override void Execute()
      {
         string utilName = null;

         new OptionSet().Add("u:|util:", u => utilName = u).Parse(GetCommandLine());

         if(string.IsNullOrEmpty(utilName))
            throw new ArgumentException("util name not specified");

         switch (utilName)
         {
            case "asminfo":
               AssemblyInfoUtil();
               break;
            default:
               throw new ArgumentException("unknown util name: " + utilName);
         }
      }

      private void AssemblyInfoUtil()
      {
         string searchPatternInclude = null;
         string searchPatternExclude = null;
         string assemblyVersion = null;
         string assemblyFileVersion = null;

         new OptionSet()
            .Add("i:|include:", i => searchPatternInclude = i)
            .Add("e:|exclude:", e => searchPatternExclude = e)
            .Add("av:|assembly-version:", av => assemblyVersion = av)
            .Add("fv:|file-version:", fv => assemblyFileVersion = fv)
            .Parse(GetCommandLine());

         if (string.IsNullOrEmpty(searchPatternInclude)) searchPatternInclude = "**/AssemblyInfo.cs";

         if(string.IsNullOrEmpty(assemblyVersion) && string.IsNullOrEmpty(assemblyFileVersion))
            throw new ArgumentException("nothing to update");

         AssemblyInfoUtil(searchPatternInclude, searchPatternExclude,
            assemblyVersion == null ? null : new Version(assemblyVersion),
            assemblyFileVersion == null ? null : new Version(assemblyFileVersion));

      }

      private bool UpdateVersion(string fileName, string versionAttributeName, Version versionValue)
      {
         Regex rgx = new Regex(versionAttributeName + "\\(\"(.*)\"\\)");

         string text = File.ReadAllText(fileName);

         MatchCollection matches = rgx.Matches(text);

         for (int i = matches.Count - 1; i >= 0; i-- )
         {
            string newValue = versionAttributeName + "(\"" + versionValue + "\")";

            Capture cap = matches[i].Captures[0];

            text = text.Remove(cap.Index, cap.Length);
            text = text.Insert(cap.Index, newValue);

            //File.WriteAllText(fileName, text);

         }

         return matches.Count > 0;
      }

      private void AssemblyInfoUtil(string searchPatternInclude, string searchPatternExclude, Version assemblyVersion, Version assemblyFileVersion)
      {
         GlamTerm.Write("searching ");
         GlamTerm.Write(ConsoleColor.Green, searchPatternInclude);

         IEnumerable<FileInfo> files = PathUtils.SearchFiles(Environment.CurrentDirectory, searchPatternInclude,
                                                             searchPatternExclude);

         GlamTerm.WriteOk();
         GlamTerm.Write("found ");
         GlamTerm.Write(ConsoleColor.Yellow, files.Count().ToString());
         GlamTerm.WriteLine(" files");
       
         foreach(var file in files)
         {
            GlamTerm.Write(ConsoleColor.Green, "updating ");
            GlamTerm.Write(file.FullName);

            if (assemblyVersion != null)
               UpdateVersion(file.FullName, "AssemblyVersion", assemblyVersion);

            if (assemblyFileVersion != null)
               UpdateVersion(file.FullName, "AssemblyFileVersion", assemblyFileVersion);

            GlamTerm.WriteOk();
         }
      }
   }
}
