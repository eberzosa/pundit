using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pundit.Core;
using Pundit.Core.Model;
using Pundit.Core.Server;

namespace Pundit.Web.Controllers
{
   /// <summary>
   /// HTTP REST repository web service
   /// </summary>
   public class RemoteRepositoryController : Controller
   {
      private static IRemoteRepository _ri;

      private static IRemoteRepository Repo
      {
         get
         {
            if (_ri == null) _ri = RemoteRepositoryServerFactory.CreateSqlDiskServer(null);

            return _ri;
         }
      }

      [HttpPost]
      public ActionResult Publish()
      {
         return null;
      }

      [HttpGet]
      public FileResult Download(string platform, string packageId, string version)
      {
         //return File()
         return null;
      }

      [HttpGet]
      public JsonResult GetSnapshot(string changeId)
      {
         return null;
      }

   }
}
