using System;
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
         root.NewSubCommand("update", "Checks for updates and if there is a new version performs the actual update")
            .Build((cmd, arg, opt) =>
            {
               var configuration = BuildConfigurationOption(opt);
               var local = BuildLocalOption(opt);
               var force = BuildForceOption(opt);
               var ping = BuildDryRunOption(opt);

               cmd.OnExecute(() =>
                  _controller.Execute(configuration.Value, local.HasValue, force.HasValue, ping.HasValue).ToInteger());
            });
      }
   }
}
