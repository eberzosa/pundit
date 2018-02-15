using System;
using System.Collections.Generic;
using EBerzosa.CommandLineProcess;
using EBerzosa.Pundit.CommandLine.Controllers;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.CommandLine.Builders
{
   internal class PublishBuilder : Builder
   {
      private readonly PublishController _controller;

      public PublishBuilder(PublishController controller)
      {
         Guard.NotNull(controller, nameof(controller));
         _controller = controller;
      }

      public override void Build(IRootCommandMultiple root)
      {
         var command = root.NewSubCommand("publish", "Publishes package to a repository(ies)")
            .Build((cmd, arg, opt) =>
            {
               var package = arg.SingleValue("package", "Specificies the package to publish");
               var repository = opt.SingleValue("r", "repo", "name", "Specifies repository to publish to. Note that you have to have publishing permissions in that repo.");

               cmd.OnExecute(() => _controller.Execute(package.Value, repository.Value).ToInteger());
            });
      }

      public override void ReplaceLegacy(ref string[] args)
      {
         if (args == null || args.Length < 2 || !"publish".Equals(args[0], StringComparison.OrdinalIgnoreCase))
            return;

         ReorderArgs(ref args, "p");

         var newArgs = new List<string> {args[0]};
         for (int i = 1; i < args.Length; i++)
            newArgs.AddRange(ProcessArgsAndReplaceOldColonOption(args[i], new[]
            {
               new ProcessAndReplace {Search = "p", Transform = s => s},
               new ProcessAndReplace {Search = "r", Replace = "r"}
            }));

         args = newArgs.ToArray();
      }

      /*
      examples:
        {0} publish
        {0} publish -p:packed/*.pundit
        {0} publish -r:myrepo1
       */
   }
}
