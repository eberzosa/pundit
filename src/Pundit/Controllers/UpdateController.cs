using EBerzosa.CommandLineProcess;
using EBerzosa.CommandLineProcess.Utils;
using EBerzosa.Pundit.Core.Services;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.CommandLine.Controllers
{
   internal class UpdateController : Controller
   {
      private readonly ServiceFactory _serviceFactory;

      public UpdateController(ServiceFactory serviceFactory, IOutput output, IInput input) 
         : base(output, input)
      {
         Guard.NotNull(serviceFactory, nameof(serviceFactory));

         _serviceFactory = serviceFactory;
      }

      public ExitCode Execute(string configuration, bool localOnly, bool force, bool dryRun, bool includeDeveloperPackages)
      {
         var service = _serviceFactory.GetUpdateService();
         service.LocalReposOnly = localOnly;
         service.Force = force;
         service.DryRun = dryRun;
         service.IncludeDeveloperPackages = includeDeveloperPackages;

         service.Execute();

         return ExitCode.Success;
      }
   }
}
