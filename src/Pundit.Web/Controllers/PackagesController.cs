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
         PackagesResult result =
            _packages.GetPackages(new PackagesQuery(0, 100)
                                     {
                                        SortOrder = PackageSortOrder.CreatedDate,
                                        SortAscending = false
                                     });

         ViewBag.Count = result.TotalCount;
         return View(result.Packages ?? new DbPackage[0]);
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
