using EBerzosa.CommandLineProcess;
using EBerzosa.CommandLineProcess.Utils;
using EBerzosa.Pundit.Core.Services;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.CommandLine.Controllers
{
   internal class PublishController : Controller
   {
      private readonly ServiceFactory _serviceFactory;

      public PublishController(ServiceFactory serviceFactory, IOutput output, IInput input) 
         : base(output, input)
      {
         _serviceFactory = serviceFactory;
      }

      public ExitCode Execute(string package, string repository)
      {
         Guard.NotNull(package, nameof(package));
         
         var service = _serviceFactory.GetPublishService();
         service.Repository = repository;

         service.Publish(package);

         return ExitCode.Success;
      }
   }
}
