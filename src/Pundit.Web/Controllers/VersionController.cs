using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pundit.Web.Controllers
{
   public class VersionController : Controller
   {
      [HttpGet]
      public ContentResult Latest()
      {
         return new ContentResult();
      }
   }
}
