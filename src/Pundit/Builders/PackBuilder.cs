using EBerzosa.CommandLineProcess;
using EBerzosa.Pundit.CommandLine.Controllers;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.CommandLine.Builders
{
   internal class PackBuilder : Builder
   {
      private readonly PackController _controller;

      public PackBuilder(PackController controller)
      {
         Guard.NotNull(controller, nameof(controller));
         _controller = controller;
      }

      public override void Build(IRootCommandMultiple root)
      {
         IOption manifest = null;
         IOption output = null;
         IOption version = null;

         var command = root.NewSubCommand("pack", "Creates package based on manifest definition")
            .Build((cmd, arg, opt) =>
            {
               manifest = BuildManifestOption(opt);
               output = opt.SingleValue("o", "output", "directory", "Specifies the output directory for the package");
               version = opt.SingleValue("v", "version", "versionNumber", "Overrides the version number inside the manifest");
            })
            .OnExecute(() => _controller.Execute(manifest.Value, output.Value, version.Value).ToInteger());
      }
      
      /* 
      examples:
        {0} pack
        {0} pack -m:library.pundit
        {0} pack -o:c:\packages\
        {0} pack -m:../pundit.xml -o:../ -v:3.0.1.401
       */
   }
}
