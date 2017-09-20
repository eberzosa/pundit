using System;
using EBerzosa.CommandLineProcess;
using EBerzosa.CommandLineProcess.Utils;
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

      public ExitCode Execute(string manifest, string outputPath, string versionString)
      {
         SafeExecute(() =>
         {
            var service = _serviceFactory.GetPackService();

            if (versionString != null)
            {
               if (!Version.TryParse(versionString, out var version))
                  throw new ArgumentException($"Invalid version '{versionString}' format");

               service.Version = version;
            }

            service.ManifestFileOrPath = manifest;
            service.OutputPath = outputPath;

            service.Pack();
         });

         return ExitCode.Success;
      }
   }
}
