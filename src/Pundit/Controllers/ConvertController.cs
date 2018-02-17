﻿using EBerzosa.CommandLineProcess;
using EBerzosa.CommandLineProcess.Utils;
using EBerzosa.Pundit.Core.Services;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.CommandLine.Controllers
{
   internal class ConvertController : Controller
   {
      private readonly ServiceFactory _serviceFactory;

      public ConvertController(ServiceFactory serviceFactory, IOutput output, IInput input) 
         : base(output, input)
      {
         Guard.NotNull(serviceFactory, nameof(serviceFactory));

         _serviceFactory = serviceFactory;
      }

      public ExitCode NuGetToPundit(string sourceFile, string outputPath, string framework)
      {
         SafeExecute(() =>
         {
            var service = _serviceFactory.GetConvertService();
            
            service.SourcePath = sourceFile;
            service.DestinationFolder = outputPath;
            service.Framework = framework;

            service.NuGetToPundit();
         });

         return ExitCode.Success;
      }
   }
}