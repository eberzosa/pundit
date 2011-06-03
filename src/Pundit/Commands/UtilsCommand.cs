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
         string assemblyCompany = null;
         string assemblyProduct = null;
         string assemblyCopyright = null;
         string assemblyDescription = null;
         string assemblyTitle = null;
         string assemblyTrademark = null;
         string assemblyCulture = null;

         new OptionSet()
            .Add("i:|include:", i => searchPatternInclude = i)
            .Add("e:|exclude:", e => searchPatternExclude = e)
            .Add("av:|assembly-version:", av => assemblyVersion = av)
            .Add("fv:|file-version:", fv => assemblyFileVersion = fv)
            .Add("assembly-company:", ac => assemblyCompany = ac)
            .Add("assembly-product:", ap => assemblyProduct = ap)
            .Add("assembly-copyright:", ac => assemblyCopyright = ac)
            .Add("assembly-description:", ad => assemblyDescription = ad)
            .Add("assembly-title:", at => assemblyTitle = at)
            .Add("assembly-trademark:", at => assemblyTrademark = at)
            .Add("assembly-culture:", ac => assemblyCulture = ac)
            .Parse(GetCommandLine());

         if (string.IsNullOrEmpty(searchPatternInclude)) searchPatternInclude = "**/AssemblyInfo.cs";

         if(string.IsNullOrEmpty(assemblyVersion) && string.IsNullOrEmpty(assemblyFileVersion))
            throw new ArgumentException("nothing to update");

         Dictionary<string, string> extraTags = new Dictionary<string, string>();
         if (!string.IsNullOrEmpty(assemblyCompany)) extraTags["AssemblyCompany"] = assemblyCompany;
         if (!string.IsNullOrEmpty(assemblyProduct)) extraTags["AssemblyProduct"] = assemblyProduct;
         if (!string.IsNullOrEmpty(assemblyCopyright)) extraTags["AssemblyCopyright"] = assemblyCopyright;
         if (!string.IsNullOrEmpty(assemblyDescription)) extraTags["AssemblyDescription"] = assemblyDescription;
         if (!string.IsNullOrEmpty(assemblyTitle)) extraTags["AssemblyTitle"] = assemblyTitle;
         if (!string.IsNullOrEmpty(assemblyTrademark)) extraTags["AssemblyTrademark"] = assemblyTrademark;
         if (!string.IsNullOrEmpty(assemblyCulture)) extraTags["AssemblyCulture"] = assemblyCulture;

         AssemblyInfoUtil(searchPatternInclude, searchPatternExclude,
            assemblyVersion == null ? null : new Version(assemblyVersion),
            assemblyFileVersion == null ? null : new Version(assemblyFileVersion),
            extraTags);

      }

      private static bool UpdateAssemblyInfoTag(string fileName, string tagName, string tagValue)
      {
         /*GlamTerm.Write("Updating ");
         GlamTerm.Write(ConsoleColor.Green, tagName);
         GlamTerm.Write(" to ");
         GlamTerm.Write(ConsoleColor.Yellow, tagValue);*/

         Regex rgx = new Regex(tagName + "\\(\"(.*)\"\\)");

         string text = File.ReadAllText(fileName);

         MatchCollection matches = rgx.Matches(text);

         for (int i = matches.Count - 1; i >= 0; i-- )
         {
            string newValue = tagName + "(\"" + tagValue + "\")";

            Capture cap = matches[i].Captures[0];

            text = text.Remove(cap.Index, cap.Length);
            text = text.Insert(cap.Index, newValue);

            File.WriteAllText(fileName, text);

         }

         /*if(matches.Count > 0)
            GlamTerm.WriteOk();
         else
            GlamTerm.WriteFail();*/

         return matches.Count > 0;
      }

      private static void AssemblyInfoUtil(string searchPatternInclude, string searchPatternExclude,
         Version assemblyVersion, Version assemblyFileVersion,
         Dictionary<string, string> extraTags)
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
               UpdateAssemblyInfoTag(file.FullName, "AssemblyVersion", assemblyVersion.ToString());

            if (assemblyFileVersion != null)
               UpdateAssemblyInfoTag(file.FullName, "AssemblyFileVersion", assemblyFileVersion.ToString());

            if (extraTags != null && extraTags.Count > 0)
            {
               foreach (var tag in extraTags)
               {
                  UpdateAssemblyInfoTag(file.FullName, tag.Key, tag.Value);
               }
            }

            GlamTerm.WriteOk();
         }
      }
   }
}
