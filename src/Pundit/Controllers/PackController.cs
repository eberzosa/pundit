using System;
using EBerzosa.CommandLineProcess;
using EBerzosa.CommandLineProcess.Utils;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Services;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.CommandLine.Controllers
{
   internal class PackController : Controller
   {
      private readonly ServiceFactory _serviceFactory;

      public PackController(ServiceFactory serviceFactory, IOutput output, IInput input) 
         : base(output, input)
      {
         Guard.NotNull(serviceFactory, nameof(serviceFactory));

         _serviceFactory = serviceFactory;
      }

      public ExitCode Execute(string manifest, string outputPath, string versionString, string releaseLabel)
      {
         var service = _serviceFactory.GetPackService();
            
         service.Version = versionString;
         service.ReleaseLabel = releaseLabel;

         service.ManifestFileOrPath = manifest;
         service.OutputPath = outputPath;

         service.Pack();

         return ExitCode.Success;
      }
   }
}
