using EBerzosa.CommandLineProcess;
using EBerzosa.Pundit.CommandLine.Controllers;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.CommandLine.Builders
{
   internal class UpdateBuilder : Builder
   {
      private readonly UpdateController _controller;

      public UpdateBuilder(UpdateController controller)
      {
         Guard.NotNull(controller, nameof(controller));
         _controller = controller;
      }

      public override void Build(IRootCommandMultiple root)
      {
         var command = root.NewSubCommand("update", "Checks for updates and if there is a new version performs the actual update")
            .OnExecute(() => _controller.Execute().ToInteger());
      }
   }
}
