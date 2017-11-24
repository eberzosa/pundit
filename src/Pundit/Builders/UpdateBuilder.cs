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
         IOption finalise = null;

         var command = root.NewSubCommand("update", "Checks for updates and if there is a new version performs the actual update")
            .Build((cmd, arg, opt) =>
            {
               configuration = BuildConfigurationOption(opt);
               local = BuildLocalOption(opt);
               force = BuildForceOption(opt);
               ping = BuildDryRunOption(opt);
               includeDeveloperPackages = BuildIncludeDeveloperOption(opt);
               finalise = opt.SingleValue("z", "finalise", "whatever", "Not to be used");
            })
            .OnExecute(() =>
            {
               if (finalise.HasValue && int.TryParse(finalise.Value, out var processId) && processId > 0)
                  return _controller.FinaliseUpdate(processId).ToInteger();
                  
               return _controller.Execute(configuration.Value, local.HasValue, force.HasValue, ping.HasValue, includeDeveloperPackages.HasValue)
                     .ToInteger();
            });
      }
   }
}
