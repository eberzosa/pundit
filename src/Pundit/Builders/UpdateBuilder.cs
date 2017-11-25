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
         IOption configuration = null;
         IOption local = null;
         IOption force = null;
         IOption ping = null;
         IOption includeDeveloperPackages = null;

         var command = root.NewSubCommand("update", "Checks for updates and if there is a new version performs the actual update")
            .Build((cmd, arg, opt) =>
            {
               configuration = BuildConfigurationOption(opt);
               local = BuildLocalOption(opt);
               force = BuildForceOption(opt);
               ping = BuildDryRunOption(opt);
               includeDeveloperPackages = BuildIncludeDeveloperOption(opt);
            })
            .OnExecute(() => _controller.Execute(configuration.Value, local.HasValue, force.HasValue, ping.HasValue, includeDeveloperPackages.HasValue).ToInteger());
      }
   }
}
