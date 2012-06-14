using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pundit.Web.Controllers
{
   [Authorize(Roles = "admins")]
   public class AuthenticatedController : Controller
   {
   }
}