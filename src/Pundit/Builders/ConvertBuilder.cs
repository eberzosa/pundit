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
      }

      public void BuildNuGetToPundit(ISubCommand parent)
      {
         IArgument source = null;
         IOption output = null;
         IOption framework = null;

         parent.NewSubCommand("NuGetToPundit", "Converts between NuGet Package and Pundit Package")
            .Build((cmd, arg, opt) =>
            {
               source = arg.SingleValue("sourceFile", "The NuGet Package file to convert");
               output = opt.SingleValue("o", "output", "directory", "Specifies the output directory for the converted file(s)");
               framework = opt.SingleValue("f", "framework", "version", "The framework version to use. Default, convert all");
            })
            .OnExecute(() => _controller.NuGetToPundit(source.Value, output.Value, framework.Value).ToInteger());
      }
   }
}
