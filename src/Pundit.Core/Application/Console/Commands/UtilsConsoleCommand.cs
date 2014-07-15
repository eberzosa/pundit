using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NDesk.Options;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Application.Console.Commands
{
   class UtilsConsoleCommand : BaseConsoleCommand
   {
      public UtilsConsoleCommand(IConsoleOutput console, string currentDirectory, string[] args)
         : base(console, currentDirectory, args)
      {
      }

      public override void Execute()
      {
         string utilName = GetParameter("u:|util:", 0);

         if(string.IsNullOrEmpty(utilName))
            throw new ArgumentException("util name not specified");

         switch (utilName)
         {
            case "asminfo":
               AssemblyInfoUtil();
               break;
            case "regex":
               RegexUtil();
               break;
            default:
               throw new ArgumentException("unknown util name: " + utilName);
         }
      }

      private void RegexUtil()
      {
         string searchPatternInclude = null;
         string searchPatternExclude = null;
         string searchRegex = null;
         string replaceText = null;

         new OptionSet()
            .Add("i:|include:", i => searchPatternInclude = i)
            .Add("e:|exclude:", e => searchPatternExclude = e)
            .Add("s:|search:", s => searchRegex = s)
            .Add("r:|replace:", r => replaceText = r)
            .Parse(GetCommandLine());

         if (searchRegex == null || replaceText == null)
            throw new ArgumentException("search regex or replace text is not set");

         console.Write("search regex: [");
         console.Write(ConsoleColor.Green, searchRegex);
         console.WriteLine("]");

         console.Write("replace text: [");
         console.Write(ConsoleColor.Yellow, replaceText);
         console.WriteLine("]");

         RegexUtil(searchPatternInclude, searchPatternExclude, searchRegex, replaceText);
      }

      private void RegexUtil(string searchPatternInclude, string searchPatternExclude, string searchRegex, string replaceRegex)
      {
         console.Write("searching files...");

         List<FileInfo> files = new List<FileInfo>(PathUtils.SearchFiles(CurrentDirectory, searchPatternInclude,
                                                    searchPatternExclude));

         console.Write(" " + files.Count + " found");
         console.Write(true);

         foreach(FileInfo fi in files)
         {
            console.Write("processing ");
            console.Write(ConsoleColor.Green, fi.Name);
            console.Write("...");

            RegexUtil(fi.FullName, searchRegex, replaceRegex);

            console.Write(false);
         }
      }

      private void RegexUtil(string filePath, string searchRegex, string replaceText)
      {
         Regex rgx = new Regex(searchRegex);

         string text = File.ReadAllText(filePath);

         MatchCollection matches = rgx.Matches(text);

         for (int i = matches.Count - 1; i >= 0; i--)
         {
            string newValue = replaceText;

            Capture cap = matches[i].Captures[0];

            text = text.Remove(cap.Index, cap.Length);
            text = text.Insert(cap.Index, newValue);

            File.WriteAllText(filePath, text);
            //console.WriteLine(text);
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
         /*console.Write("Updating ");
         console.Write(ConsoleColor.Green, tagName);
         console.Write(" to ");
         console.Write(ConsoleColor.Yellow, tagValue);*/

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
            console.WriteOk();
         else
            console.WriteFail();*/

         return matches.Count > 0;
      }

      private void AssemblyInfoUtil(string searchPatternInclude, string searchPatternExclude,
         Version assemblyVersion, Version assemblyFileVersion,
         Dictionary<string, string> extraTags)
      {
         console.Write("searching ");
         console.Write(ConsoleColor.Green, searchPatternInclude);

         IEnumerable<FileInfo> files = PathUtils.SearchFiles(Environment.CurrentDirectory, searchPatternInclude,
                                                             searchPatternExclude);

         console.Write(true);
         console.Write("found ");
         console.Write(ConsoleColor.Yellow, files.Count().ToString());
         console.WriteLine(" files");
       
         foreach(var file in files)
         {
            console.Write(ConsoleColor.Green, "updating ");
            console.Write(file.FullName);

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

            console.Write(true);
         }
      }
   }
}
