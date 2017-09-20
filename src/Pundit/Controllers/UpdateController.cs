using EBerzosa.CommandLineProcess;
using EBerzosa.CommandLineProcess.Utils;

namespace EBerzosa.Pundit.CommandLine.Controllers
{
   internal class UpdateController : Controller
   {
      public UpdateController(IOutput output, IInput input) 
         : base(output, input)
      {
      }
      public ExitCode Execute()
      {

         return ExitCode.NotSupported;
      }
   }
}
