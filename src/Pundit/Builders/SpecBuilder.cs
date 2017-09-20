using System;
using EBerzosa.CommandLineProcess;
using EBerzosa.Pundit.CommandLine.Controllers;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.CommandLine.Builders
{
   internal class SpecBuilder : Builder
   {
      private readonly SpecController _controller;

      public SpecBuilder(SpecController controller)
      {
         Guard.NotNull(controller, nameof(controller));
         _controller = controller;
      }


      public override void Build(IRootCommandMultiple root)
      {
         root.NewSubCommand("spec", "Creates an empty (and invalid) manifest")
            .OnExecute(() => _controller.Execute().ToInteger());
      }

      public override void ReplaceLegacy(ref string[] args)
      {
         if (args != null && args.Length >= 1 && "template".Equals(args[0], StringComparison.OrdinalIgnoreCase))
            args[0] = "spec";
      }
   }
}
