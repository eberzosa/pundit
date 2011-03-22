using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NGem.Core.Utils
{
   class FileSet
   {
      private List<string> _includes;
      private List<string> _excludes = new List<string>();
      private string _baseDirectory;

      public static readonly List<string> DefaultExcludesList = new List<string>
                                                                {
                                                                   "**/CVS",
                                                                   "**/CVS/**",
                                                                   "**/.svn",
                                                                   "**/.svn/**",
                                                                   "**/_svn",
                                                                   "**/_svn/**",
                                                                   "**/.cvsignore",
                                                                   "**/SCCS",
                                                                   "**/SCCS/**",
                                                                   "**/vssver.scc",
                                                                   "**/vssver2.scc",
                                                                   "**/_vti_cnf/**"
                                                                };

      public FileSet(string baseDirectory, string[] includePatterns, string[] excludePatterns)
      {
         _baseDirectory = baseDirectory;
         _includes = new List<string>(includePatterns);

         AddDefaultExcludes();

         if(excludePatterns != null)
         {
            _excludes.AddRange(excludePatterns);
         }
      }

      private void AddDefaultExcludes()
      {
         _excludes.AddRange(DefaultExcludesList);
      }

      private static string RegexFromPattern(string pattern)
      {
         pattern = pattern
            .Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar)
            .Replace("*", "[^\\" + Path.DirectorySeparatorChar + "]*")
            .Replace("**", "\\.*")
            .Replace(".", "\\.")
            .Replace("?", ".");

         if (!pattern.StartsWith("^")) pattern = "^" + pattern;

         if (!pattern.EndsWith("$")) pattern = pattern + "$";

         return pattern;
      }

      private static bool IsMatch(string fullPath, string pattern)
      {
         Regex rgx = new Regex(RegexFromPattern(pattern),
            RegexOptions.Singleline | RegexOptions.IgnoreCase);

         return rgx.IsMatch(fullPath);
      }

      public string[] GetAllDirectories()
      {
         //foreach()
         return null;
      }
   }
}
