using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NGem.Core.Utils
{
   public class FileSet
   {
      private List<string> _includes;
      private List<string> _excludes = new List<string>();

      private static readonly List<string> DefaultExcludes = new List<string>
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
         _includes = new List<string>(includePatterns);

         AddDefaultExcludes();

         if(excludePatterns != null)
         {
            _excludes.AddRange(excludePatterns);
         }
      }

      private void AddDefaultExcludes()
      {
         _excludes.Add();
      }
   }
}
