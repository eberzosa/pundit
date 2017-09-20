﻿using EBerzosa.CommandLineProcess;
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
         IArgument package = null;
         IOption repository = null;

         var command = root.NewSubCommand("publish", "Publishes package to a repository(ies)")
            .Build((cmd, arg, opt) =>
            {
               package = arg.SingleValue("package", "Specificies the package to publish");
               repository = opt.SingleValue("r", "repo", "name", "Specifies repository to publish to. Note that you have to have publishing permissions in that repo.");
            })
            .OnExecute(() => _controller.Execute(package.Value, repository.Value).ToInteger());
      }

      /*
      examples:
        {0} publish
        {0} publish -p:packed/*.pundit
        {0} publish -r:myrepo1
       */
   }
}
