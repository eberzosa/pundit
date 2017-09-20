using System;
using System.Collections.Generic;
using System.Linq;
using EBerzosa.CommandLineProcess;

namespace EBerzosa.Pundit.CommandLine.Builders
{
   internal interface IBuilder
   {
      void Build(IRootCommandMultiple root);

      void ReplaceLegacy(ref string[] args);
   }

   internal abstract class Builder : IBuilder
   {
      public abstract void Build(IRootCommandMultiple root);

      public virtual void ReplaceLegacy(ref string[] args)
      {
      }

      protected IOption BuildLocalOption(IOptionCreator optionCreator) => optionCreator.NoValue("l", "localOnly", "Use local repositories only.");

      protected IOption BuildManifestOption(IOptionCreator optionCreator)=> optionCreator.SingleValue("m", "manifest", "manifestFile", "The manifest file");

      protected IEnumerable<string> ProcessArgsAndReplaceOldColonOption(string option, IEnumerable<ProcessAndReplace> processAndReplaces)
      {
         foreach (var processAndReplace in processAndReplaces)
         {
            if (!option.StartsWith('-' + processAndReplace.Search + ':'))
               continue;

            var value = option.Substring(processAndReplace.Search.Length + 2);
            
            if (processAndReplace.CustomReplace != null)
            {
               foreach (var str in processAndReplace.CustomReplace(value))
                  yield return str;

               yield break;
            }

            if (processAndReplace.Ignore != null && processAndReplace.Ignore(value))
               yield break;

            yield return '-' + processAndReplace.Replace;

            if (processAndReplace.Transform == null)
               yield return value;
            else
            {
               var transformed = processAndReplace.Transform(value);
               if (transformed != null)
                  yield return transformed;
            }

            yield break;
         }

         yield return option;
      }

      protected ProcessAndReplace GetProcessAndReplaceDepth()
      {
         return new ProcessAndReplace
         {
            Search = "d",
            Replace = "l",
            Ignore = value => value != "0" && !"local".Equals(value, StringComparison.OrdinalIgnoreCase),
            Transform = value => null
         };
      }
   }

   internal class ProcessAndReplace
   {
      public string Search { get; set; }

      public string Replace { get; set; }

      public Func<string, IEnumerable<string>> CustomReplace { get; set; }

      public Func<string, string> Transform { get; set; }

      public Func<string, bool> Ignore { get; set; }
   }

   internal interface ISubBuilder
   {
      void Build(ISubCommand parent);
   }
}