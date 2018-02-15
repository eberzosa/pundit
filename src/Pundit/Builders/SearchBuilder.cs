using System;
using System.Collections.Generic;
using EBerzosa.CommandLineProcess;
using EBerzosa.Pundit.CommandLine.Controllers;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.CommandLine.Builders
{
   internal class SearchBuilder : Builder
   {
      private readonly SearchController _controller;

      public SearchBuilder(SearchController controller)
      {
         Guard.NotNull(controller, nameof(controller));
         _controller = controller;
      }

      public override void Build(IRootCommandMultiple root)
      {
         root.NewSubCommand("search", "Search for packages in repositories")
            .Build((cmd, arg, opt) =>
            {
               var search = arg.SingleValue("search", "Package name or name part (case insensitive)");
               var localOnly = BuildLocalOption(opt);
               var xml = opt.NoValue("x", "xml", "Search will be formatted in XML allowing to copy-paste it as in in a manifest");

               cmd.OnExecute(() => _controller.Execute(search.Value, localOnly.HasValue, xml.HasValue).ToInteger());
            });
      }

      public override void ReplaceLegacy(ref string[] args)
      {
         if (args == null || args.Length < 2 || !"search".Equals(args[0], StringComparison.OrdinalIgnoreCase))
            return;

         var newArgs = new List<string> { args[0] };
         for (int i = 1; i < args.Length; i++)
         {
            newArgs.AddRange(ProcessArgsAndReplaceOldColonOption(args[i], new[]
            {
               new ProcessAndReplace {Search = "t", CustomReplace = value => new[] {value}},
               GetProcessAndReplaceDepth()
            }));
         }

         args = newArgs.ToArray();
      }

      /*
examples:
  {0} search log4net
  {0} search log4net -x
       */
   }
}
