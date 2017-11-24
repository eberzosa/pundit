using System;
using System.Collections.Generic;
using EBerzosa.CommandLineProcess;
using EBerzosa.Pundit.CommandLine.Controllers;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.CommandLine.Builders
{
   internal class PackBuilder : Builder
   {
      private readonly PackController _controller;

      public PackBuilder(PackController controller)
      {
         Guard.NotNull(controller, nameof(controller));
         _controller = controller;
      }

      public override void Build(IRootCommandMultiple root)
      {
         IArgument manifest = null;
         IOption output = null;
         IOption version = null;
         IOption isDeveloperPackage = null;

         var command = root.NewSubCommand("pack", "Creates package based on manifest definition")
            .Build((cmd, arg, opt) =>
            {
               manifest = BuildManifestArgument(arg);
               //manifest = BuildManifestOption(opt);
               output = opt.SingleValue("o", "output", "directory", "Specifies the output directory for the package");
               version = opt.SingleValue("v", "version", "versionNumber", "Overrides the version number inside the manifest");
               isDeveloperPackage = opt.NoValue("p", "isDeveloperPackage", "Specifies if the package should be built as a developer package");
            })
            .OnExecute(() => _controller.Execute(manifest.Value, output.Value, version.Value, isDeveloperPackage.HasValue).ToInteger());
      }

      public override void ReplaceLegacy(ref string[] args)
      {
         if (args == null || args.Length < 2 || !"pack".Equals(args[0], StringComparison.OrdinalIgnoreCase))
            return;

         ReorderArgs(ref args, "m");

         var newArgs = new List<string> {args[0]};
         for (int i = 1; i < args.Length; i++)
            
            newArgs.AddRange(ProcessArgsAndReplaceOldColonOption(args[i], new[]
            {
               new ProcessAndReplace {Search = "f", Replace = "o"},
               new ProcessAndReplace {Search = "m", Transform = s => s}
            }));

         args = newArgs.ToArray();
      }

      /* 
      examples:
        {0} pack
        {0} pack -m:library.pundit
        {0} pack -o:c:\packages\
        {0} pack -m:../pundit.xml -o:../ -v:3.0.1.401
       */
   }
}
