using EBerzosa.CommandLineProcess;
using EBerzosa.Pundit.CommandLine.Controllers;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.CommandLine.Builders
{
   internal class SourcesBuilder : Builder
   {
      private readonly SourcesController _controller;

      public SourcesBuilder(SourcesController controller)
      {
         Guard.NotNull(controller, nameof(controller));
         _controller = controller;
      }

      public override void Build(IRootCommandMultiple root)
      {
         var command = root.NewSubCommand("sources", "Provides the ability to manage list of sources")
            .Build((cmd, arg, opt) =>
            {
               cmd.NewSubCommand("info", "Prints out information about registered repositories")
                  .OnExecute(() => _controller.Info().ToInteger());

            });
      }
   }
}
