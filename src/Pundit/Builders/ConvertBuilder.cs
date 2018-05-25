using EBerzosa.CommandLineProcess;
using EBerzosa.Pundit.CommandLine.Controllers;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.CommandLine.Builders
{
   internal class ConvertBuilder : Builder
   {
      private readonly ConvertController _controller;

      public ConvertBuilder(ConvertController controller)
      {
         Guard.NotNull(controller, nameof(controller));
         _controller = controller;
      }

      public override void Build(IRootCommandMultiple root)
      {
         var parent = root.NewSubCommand("convert", "Converts between NuGet and Pundit packages and Specs");

         BuildNuGetToPundit(parent);
         BuildPunditToNuGet(parent);
      }

      public void BuildNuGetToPundit(ISubCommand parent)
      {
         parent.NewSubCommand("NuGetToPundit", "Converts a NuGet Package to a Pundit Package")
            .Build((cmd, arg, opt) =>
            {
               var source = arg.SingleValue("sourceFile", "The NuGet Package file to convert");
               var output = opt.SingleValue("o", "output", "directory", "Specifies the output directory for the converted file(s)");
               var framework = opt.SingleValue("f", "framework", "version", "The framework version to use. Default, convert all");

               cmd.OnExecute(() => _controller.NuGetToPundit(source.Value, output.Value, framework.Value).ToInteger());
            });

      }

      public void BuildPunditToNuGet(ISubCommand parent)
      {
         parent.NewSubCommand("PunditToNuGet", "Converts a Pundit Package to a NuGet Package")
            .Build((cmd, arg, opt) =>
            {
               var source = arg.SingleValue("sourceFile", "The NuGet Package file to convert");
               var output = opt.SingleValue("o", "output", "directory", "Specifies the output directory for the converted file(s)");

               cmd.OnExecute(() => _controller.PunditToNuGet(source.Value, output.Value).ToInteger());
            });
      }
   }
}
