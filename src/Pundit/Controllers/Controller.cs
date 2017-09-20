using System;
using EBerzosa.CommandLineProcess;
using EBerzosa.CommandLineProcess.Utils;
using EBerzosa.Pundit.CommandLine.Utils;
using EBerzosa.Pundit.Core;

namespace EBerzosa.Pundit.CommandLine.Controllers
{
   internal abstract class Controller : IController
   {
      private readonly IOutput _output;

      protected IWriter Output { get; }

      protected IInput Input { get; }

      protected Controller(IOutput output, IInput input)
      {
         _output = output;

         Output = new ConsoleWriter(output);
         Input = input;
      }

      protected ExitCode SafeExecute(Action action)
      {
         try
         {
            action();
         }
         catch (Exception ex)
         {
            while (true)
            {
               _output.PrintException(ex, true);

               ex = ex.InnerException;

               if (ex == null)
                  break;

               _output.Print("===================================================== INNER EXCEPTION =====================================================", true, ConsoleColor.Red);
            }
         }

         return ExitCode.Success;
      }
   }
}
