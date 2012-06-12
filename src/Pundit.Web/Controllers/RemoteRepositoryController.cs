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
      private readonly IRemoteRepository _remote;

      public RemoteRepositoryController(IRemoteRepository remote)
      {
         _remote = remote;
      }

      [HttpPost]
      public void Publish()
      {
         _remote.Publish(Request.InputStream);
      }

      [HttpGet]
      public FileResult Download(string platform, string packageId, string version)
      {
         return File(_remote.Download(platform, packageId, version), "pundit/package-zip");
      }

      [HttpGet]
      public ContentResult GetSnapshot(string delta)
      {
         RemoteSnapshot snapshot = _remote.GetSnapshot(delta);
         string xml = snapshot.ToXml();
         return Content(xml, "text/xml");
      }

   }
}
