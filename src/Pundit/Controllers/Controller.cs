using EBerzosa.CommandLineProcess;
using EBerzosa.Pundit.CommandLine.Utils;
using EBerzosa.Pundit.Core;

namespace EBerzosa.Pundit.CommandLine.Controllers
{
    internal abstract class Controller : IController
    {
        protected IWriter Output { get; }

        protected IInput Input { get; }

        protected Controller(IOutput output, IInput input)
        {
            Output = new ConsoleWriter(output);
            Input = input;
        }
    }
}
