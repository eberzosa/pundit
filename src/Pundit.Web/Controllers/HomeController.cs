using System.Web.Mvc;
using Pundit.Core.Server.Model;

namespace Pundit.Web.Controllers
{
   public class HomeController : Controller
   {
      private readonly IPackageRepository _packages;

      public HomeController(IPackageRepository packages)
      {
         _packages = packages;
      }

      public ActionResult Index()
      {
         return View();
      }
   }
}
