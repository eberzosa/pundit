using System;
using System.Collections.Generic;
using EBerzosa.CommandLineProcess;
using EBerzosa.Pundit.CommandLine.Controllers;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.CommandLine.Builders
{
   internal class ResolveBuilder : Builder
   {
      private readonly ResolveController _controller;

      public ResolveBuilder(ResolveController controller)
      {
         Guard.NotNull(controller, nameof(controller));
         _controller = controller;
      }

      public override void Build(IRootCommandMultiple root)
      {
         IOption manifest = null;
         IOption configuration = null;
         IOption local = null;
         IOption force = null;
         IOption ping = null;
         IOption includeDeveloperPackages = null;

         var command = root.NewSubCommand("resolve", "Resolves dependencies and refresh project packages specifiend in the manifest")
            .Build((cmd, arg, opt) =>
            {
               manifest = BuildManifestOption(opt);
               configuration = BuildConfigurationOption(opt);
               local = BuildLocalOption(opt);
               force = BuildForceOption(opt);
               ping = BuildDryRunOption(opt);
               includeDeveloperPackages = BuildIncludeDeveloperOption(opt);
            })
            .OnExecute(() => _controller.Execute(manifest.Value, configuration.Value, local.HasValue, force.HasValue, ping.HasValue, includeDeveloperPackages.HasValue).ToInteger());
      }

      public override void ReplaceLegacy(ref string[] args)
      {
         if (args == null || args.Length < 2 || !"resolve".Equals(args[0], StringComparison.OrdinalIgnoreCase))
            return;

         var newArgs = new List<string> {args[0]};
         for (int i = 1; i < args.Length; i++)
         {
            newArgs.AddRange(ProcessArgsAndReplaceOldColonOption(args[i], new[]
            {
               new ProcessAndReplace {Search = "m", Replace = "m"},
               new ProcessAndReplace {Search = "c", Replace = "c"},
               GetProcessAndReplaceDepth()
            }));
         }

         args = newArgs.ToArray();
      }

      /*
examples:
  {0} resolve
  {0} resolve Pundit.Extensions.xml
  {0} resolve -c:debug
  {0} resolve -f 
       */
   }
}
