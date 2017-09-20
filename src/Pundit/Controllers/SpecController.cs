using System;
using System.IO;
using System.Reflection;
using EBerzosa.CommandLineProcess;
using EBerzosa.CommandLineProcess.Utils;
using EBerzosa.Pundit.Core.Services;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.CommandLine.Controllers
{
   internal class SpecController : Controller
   {
      private readonly ServiceFactory _serviceFactory;

      public SpecController(ServiceFactory serviceFactory, IOutput output, IInput input) 
         : base(output, input)
      {
         _serviceFactory = serviceFactory;
      }

      public ExitCode Execute()
      {
         SafeExecute(() => _serviceFactory.GetSpecService().Execute());
         
         return ExitCode.Success;
      }
   }
}
