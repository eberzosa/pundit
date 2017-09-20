using System;
using EBerzosa.CommandLineProcess;
using EBerzosa.CommandLineProcess.Utils;
using EBerzosa.Pundit.Core.Services;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.CommandLine.Controllers
{
   internal class ResolveController : Controller
   {
     private readonly ServiceFactory _serviceFactory;

      public ResolveController(ServiceFactory serviceFactory, IOutput output, IInput input) 
         : base(output, input)
      {
         _serviceFactory = serviceFactory;
      }

      public ExitCode Execute(string manifest, string configuration, bool localOnly, bool force, bool dryRun)
      {
         SafeExecute(() =>
         {
            var service = _serviceFactory.GetResolveService();

            service.ManifestFileOrPath = manifest;

            if (configuration == null || BuildConfiguration.Release.ToString().Equals(configuration, StringComparison.OrdinalIgnoreCase))
               service.Configuration = BuildConfiguration.Release;

            else if (BuildConfiguration.Debug.ToString().Equals(configuration, StringComparison.OrdinalIgnoreCase))
               service.Configuration = BuildConfiguration.Debug;

            else if (BuildConfiguration.Any.ToString().Equals(configuration, StringComparison.OrdinalIgnoreCase))
               service.Configuration = BuildConfiguration.Any;

            else
               throw new NotSupportedException($"Configuration '{configuration}' is not supported.");

            service.LocalReposOnly = localOnly;
            service.Force = force;
            service.DryRun = dryRun;

            service.Resolve();
         });

         return ExitCode.Success;
      }
   }
}
