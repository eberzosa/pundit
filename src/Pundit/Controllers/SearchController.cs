using EBerzosa.CommandLineProcess;
using EBerzosa.CommandLineProcess.Utils;
using EBerzosa.Pundit.Core.Services;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.CommandLine.Controllers
{
   internal class SearchController : Controller
   {
      private readonly ServiceFactory _serviceFactory;

      public SearchController(ServiceFactory serviceFactory, IOutput output, IInput input) 
         : base(output, input)
      {
         _serviceFactory = serviceFactory;
      }

      public ExitCode Execute(string searchText, bool localRepoOnly, bool xml)
      {
         Guard.NotNull(searchText, nameof(searchText));
         
         var service = _serviceFactory.GetSearchService();
         service.CacheRepoOnly = localRepoOnly;
         service.ToXml = xml;

         service.Search(searchText);
         
         return ExitCode.Success;
      }
   }
}
