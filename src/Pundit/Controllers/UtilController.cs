using EBerzosa.CommandLineProcess;
using EBerzosa.Pundit.Core.Services;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.CommandLine.Controllers
{
   internal class UtilController : Controller
   {
      private readonly ServiceFactory _serviceFactory;

      public UtilController(ServiceFactory serviceFactory, IOutput output, IInput input) 
         : base(output, input)
      {
         Guard.NotNull(serviceFactory, nameof(serviceFactory));

         _serviceFactory = serviceFactory;
      }

      public ExitCode Encrypt(string key)
      {
         var service = _serviceFactory.GetUtilService();

         Output.Success("Encrypted: " + service.EncryptNuGetApiKey(key));

         return ExitCode.Success;
      }
   }
}
