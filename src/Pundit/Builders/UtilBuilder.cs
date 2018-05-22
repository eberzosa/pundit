using System;
using EBerzosa.CommandLineProcess;
using EBerzosa.Pundit.CommandLine.Controllers;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.CommandLine.Builders
{
   internal class UtilBuilder : Builder
   {
      private readonly UtilController _controller;

      public UtilBuilder(UtilController controller)
      {
         Guard.NotNull(controller, nameof(controller));
         _controller = controller;
      }

      public override void Build(IRootCommandMultiple root)
      {
         var parent = root.NewSubCommand("util", "Utilities");
         BuildNuGetEncryptCommand(parent);
      }

      private void BuildNuGetEncryptCommand(ISubCommand parent)
      {
         parent.NewSubCommand("keyEncrypt", "Encryps a NuGet Repo Api Key")
            .Build((cmd, arg, opt) =>
            {
               var key = arg.SingleValue("key", "The key to encrypt");

               cmd.OnExecute(() => _controller.Encrypt(key.Value).ToInteger());
            });
      }
   }
}
