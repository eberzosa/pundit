using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pundit.Core.Model;
using Pundit.Core.Server.Model;

namespace Pundit.Web.Controllers
{
   public class PackagesController : Controller
   {
      private readonly IRemoteRepository _repository;
      private readonly IPackageRepository _packages;

      public PackagesController(IRemoteRepository repository, IPackageRepository packages)
      {
         _repository = repository;
         _packages = packages;
      }

      public ActionResult Index()
      {
         long totalCount;
         IEnumerable<DbPackage> list = _packages.GetPackages(0, 25, true, out totalCount);

         ViewBag.Count = totalCount;
         return View(list);
      }

      [HttpGet]
      public ActionResult Publish()
      {
         return View();
      }

      [HttpPost]
      public ActionResult Publish(HttpPostedFileBase packageFile)
      {
         if(packageFile != null)
         {
            _repository.Publish(packageFile.InputStream);
         }

         return View();
      }
   }
}
