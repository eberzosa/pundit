using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pundit.Core;
using Pundit.Core.Model;
using Pundit.Core.Server;
using Pundit.Core.Server.Model;

namespace Pundit.Web.Controllers
{
   /// <summary>
   /// HTTP REST repository web service
   /// </summary>
   public class RemoteRepositoryController : Controller
   {
      private IConfigurationRepository _config;
      private readonly IRemoteRepository _remote;

      public RemoteRepositoryController(IConfigurationRepository config, IRemoteRepository remote)
      {
         _config = config;
         _remote = remote;
      }

      [HttpPost]
      public ActionResult Publish()
      {
         return null;
      }

      [HttpGet]
      public FileResult Download(string platform, string packageId, string version)
      {
         return File(_remote.Download(platform, packageId, version), "pundit/package-zip");
      }

      [HttpGet]
      public ContentResult GetSnapshot(string changeId)
      {
         RemoteSnapshot snapshot = _remote.GetSnapshot(changeId);
         string xml = snapshot.ToXml();
         return Content(xml, "text/xml");
      }

   }
}
